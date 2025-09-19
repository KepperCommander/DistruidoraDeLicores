using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class ProveedoresPrueba
    {
        private Conexion ctx = default!;
        private IDbContextTransaction? trx;
        private Proveedores? entidad;

        public TestContext TestContext { get; set; } = default!;

        [TestInitialize]
        public void SetUp()
        {
            ctx = new Conexion { StringConexion = Configuracion.ObtenerValor("StringConexion") };
            Assert.IsTrue(ctx.Database.CanConnect(), "No se puede conectar a la base de datos");
            Assert.IsNotNull(ctx.Proveedores, "DbSet Proveedores es null en el DbContext.");
            trx = ctx.Database.BeginTransaction();
        }

        [TestCleanup]
        public void TearDown()
        {
            try { trx?.Rollback(); } catch { }
            try { trx?.Dispose(); } catch { }
            try { ctx?.Dispose(); } catch { }
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
            entidad = EntidadesNucleo.Proveedores() ?? new Proveedores
            {
                RazonSocial = $"Proveedor-{Guid.NewGuid():N}".Substring(0, 20),
                NIT = Guid.NewGuid().ToString("N").Substring(0, 9),
                Email = $"prov_{Guid.NewGuid():N}@correo.com",
                Telefono = "3100000000",
                Direccion = "Dirección prueba",
                Activo = true
            };

            ctx.Proveedores!.Add(entidad);
            var afectados = ctx.SaveChanges();

            TestContext.WriteLine($"INSERT IdProveedor = {entidad.IdProveedor}, filas afectadas = {afectados}");
            Assert.IsTrue(afectados >= 1, "No se insertó ningún registro.");
            Assert.IsTrue(entidad.IdProveedor > 0, "EF no asignó IdProveedor.");

            return true;
        }

        private bool Modificar()
        {
            Assert.IsNotNull(entidad, "No hay entidad cargada para modificar.");

            entidad!.RazonSocial = $"ProveedorMod-{Guid.NewGuid():N}".Substring(0, 20);
            entidad.Activo = !entidad.Activo;

            var afectados = ctx.SaveChanges();
            TestContext.WriteLine($"UPDATE IdProveedor = {entidad.IdProveedor}, filas afectadas = {afectados}");
            Assert.IsTrue(afectados >= 1, "No se actualizó ningún registro.");

            return true;
        }

        private bool Listar()
        {
            List<Proveedores> lista = ctx.Proveedores!.AsNoTracking().ToList();
            TestContext.WriteLine($"LISTAR total = {lista.Count}");
            return lista.Count > 0;
        }
    }
}