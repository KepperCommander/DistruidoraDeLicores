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
    public class RolesPrueba
    {
        private Conexion? ctx;
        private IDbContextTransaction? trx;
        private Roles? entidad;

        public TestContext TestContext { get; set; } = default!;

        [TestMethod]
        public void Ejecutar()
        {
            try
            {
                ctx = new Conexion
                {
                    StringConexion = Configuracion.ObtenerValor("StringConexion")
                };

                Assert.IsTrue(ctx.Database.CanConnect(), "No se puede conectar: " + ctx.Database.GetConnectionString());
                Assert.IsNotNull(ctx.Set<Roles>(), "DbSet Roles es null en el DbContext.");

              
                trx = ctx.Database.BeginTransaction();

   
                Assert.IsTrue(Guardar(), "Guardar() falló");
                Assert.IsTrue(Modificar(), "Modificar() falló");
                Assert.IsTrue(Listar(), "Listar() falló");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
            finally
            {
                try { trx?.Rollback(); } catch { }
                try { trx?.Dispose(); } catch { }
                try { ctx?.Dispose(); } catch { }
            }
        }

        private bool Guardar()
        {
            entidad = RolValido();


            if (ctx!.Set<Roles>().Any(r => r.Nombre == entidad.Nombre))
                entidad.Nombre = $"Rol-{Guid.NewGuid():N}".Substring(0, 20);

            ctx.Set<Roles>().Add(entidad);
            var afectados = ctx.SaveChanges();

            TestContext.WriteLine($"INSERT IdRol = {entidad.IdRol}, filas afectadas = {afectados}");

            Assert.IsTrue(afectados >= 1, "No se insertó ningún registro.");
            Assert.IsTrue(entidad.IdRol > 0, "EF no asignó IdRol.");
            return true;
        }

        private bool Modificar()
        {
            Assert.IsNotNull(entidad, "No hay entidad cargada para modificar.");

            entidad!.EsActivo = !entidad.EsActivo;
            entidad.Nombre += " Mod";

            var afectados = ctx!.SaveChanges();

            TestContext.WriteLine($"UPDATE IdRol = {entidad.IdRol}, filas afectadas = {afectados}");
            Assert.IsTrue(afectados >= 1, "No se actualizó ningún registro.");
            return true;
        }

        private bool Listar()
        {
            List<Roles> lista = ctx!.Set<Roles>().AsNoTracking().ToList();
            TestContext.WriteLine($"LISTAR total = {lista.Count}");
            return lista.Count > 0;
        }

        // CREATE TABLE Roles (IdRol INT IDENTITY PK, Nombre NVARCHAR(50) NOT NULL UNIQUE, EsActivo BIT NOT NULL DEFAULT 1)
        private static Roles RolValido()
        {
            return new Roles
            {
                Nombre = $"Rol-{Guid.NewGuid():N}".Substring(0, 20), 
                EsActivo = true
            };
        }
    }
}
