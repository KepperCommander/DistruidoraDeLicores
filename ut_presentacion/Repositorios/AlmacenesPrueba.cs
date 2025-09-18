using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class AlmacenesPrueba
    {
        private Conexion ctx = default!;
        private IDbContextTransaction? trx;
        private Almacenes? entidad;

        public TestContext TestContext { get; set; } = default!;

        [TestInitialize]
        public void SetUp()
        {
            ctx = new Conexion
            {
                StringConexion = Configuracion.ObtenerValor("StringConexion") // DISTRIBUIDORA_2
            };

            Assert.IsTrue(ctx.Database.CanConnect(), "No se puede conectar: " + ctx.Database.GetConnectionString());
            Assert.IsNotNull(ctx.Almacenes, "DbSet Almacenes es null en el DbContext.");

            // Todo dentro de transacción para no dejar basura (rollback en TestCleanup)
            trx = ctx.Database.BeginTransaction();
        }

        [TestCleanup]
        public void TearDown()
        {
            try { trx?.Rollback(); } catch { /* ignore */ }
            try { trx?.Dispose(); } catch { /* ignore */ }
            try { ctx?.Dispose(); } catch { /* ignore */ }
        }

        [TestMethod]
        public void Ejecutar()
        {
            try
            {
                Assert.IsTrue(Guardar(), "Guardar() falló");
                Assert.IsTrue(Modificar(), "Modificar() falló");
                Assert.IsTrue(Listar(), "Listar() falló");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        private bool Guardar()
        {
            entidad = EntidadesNucleo.Almacenes() ?? AlmacenValido();

            // Forzar nombre único para no chocar con el UNIQUE del script
            entidad.Nombre = $"Bodega-{Guid.NewGuid():N}".Substring(0, 20);

            ctx.Almacenes!.Add(entidad);
            var afectados = ctx.SaveChanges();

            TestContext.WriteLine($"INSERT IdAlmacen = {entidad.IdAlmacen}, filas afectadas = {afectados}");
            Assert.IsTrue(afectados >= 1, "No se insertó ningún registro.");
            Assert.IsTrue(entidad.IdAlmacen > 0, "EF no asignó IdAlmacen.");

            return true;
        }


        private bool Modificar()
        {
            Assert.IsNotNull(entidad, "No hay entidad cargada para modificar.");

            entidad!.Direccion = "Zona Industrial #1 - Mod";
            entidad.EsPrincipal = !entidad.EsPrincipal;

            var afectados = ctx.SaveChanges();

            TestContext.WriteLine($"UPDATE IdAlmacen = {entidad.IdAlmacen}, filas afectadas = {afectados}");
            Assert.IsTrue(afectados >= 1, "No se actualizó ningún registro.");

            return true;
        }

        private bool Listar()
        {
            List<Almacenes> lista = ctx.Almacenes!.AsNoTracking().ToList();
            TestContext.WriteLine($"LISTAR total = {lista.Count}");
            return lista.Count > 0;
        }

        // (No se usa porque hacemos ROLLBACK, pero te lo dejo por si lo quieres invocar)
        private bool Borrar()
        {
            if (entidad is null) return true;
            ctx.Almacenes!.Remove(entidad);
            var afectados = ctx.SaveChanges();
            TestContext.WriteLine($"DELETE IdAlmacen = {entidad.IdAlmacen}, filas afectadas = {afectados}");
            return afectados >= 1;
        }

        // Fábrica mínima consistente con tu esquema SQL:
        //  - Nombre: NOT NULL + UNIQUE
        //  - Direccion: NULL
        //  - EsPrincipal: NOT NULL (DEFAULT 0)
        private static Almacenes AlmacenValido()
        {
            return new Almacenes
            {
                Nombre = $"Bodega-{Guid.NewGuid():N}".Substring(0, 20),
                Direccion = "Zona Industrial #1",
                EsPrincipal = false
            };
        }
    }
}
