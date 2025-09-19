using System;
using System.Linq;
using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class EmpleadosAplicacionPrueba
    {
        [TestMethod]
        public void Ejecutar()
        {
            Conexion? ctx = null;
            IDbContextTransaction? trx = null;
            IEmpleadosAplicacion? app = null;
            Empleados? entidad = null;

            try
            {
                
                ctx = new Conexion { StringConexion = Configuracion.ObtenerValor("StringConexion") };
                Assert.IsTrue(ctx.Database.CanConnect(), "No conecta: " + ctx.Database.GetConnectionString());
                Assert.IsNotNull(ctx.Empleados, "DbSet Empleados es null");
                trx = ctx.Database.BeginTransaction();

                app = new EmpleadosAplicacion(ctx);
                app.Configurar(Configuracion.ObtenerValor("StringConexion"));

                
                entidad = EntidadesNucleo.Empleados() ?? EmpleadoValido();
                entidad.Email = $"empleado.{Guid.NewGuid():N}@prueba.local"; // evita UNIQUE
                entidad = app.Guardar(entidad);
                Assert.IsNotNull(entidad);
                Assert.IsTrue(entidad!.IdEmpleado > 0);

                entidad.Apellidos += " Actualizado";
                Assert.IsNotNull(app.Modificar(entidad));

                
                Assert.IsTrue(app.Listar().Count > 0);
                Assert.IsTrue(app.PorRol(entidad.IdRol).Count > 0);
                Assert.IsTrue(app.PorTexto(entidad.Apellidos.Split(' ').First()).Count > 0);

                
                Assert.IsNotNull(app.Borrar(entidad));
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                var root = ex.GetBaseException();  // SqlException
                Assert.Fail(root.Message);         // p.ej.: Invalid column name 'Email'
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

        private static Empleados EmpleadoValido() => new Empleados
        {
            Nombres = "EmpleadoPrueba",
            Apellidos = "Aplicacion",
            Email = $"empleado.{Guid.NewGuid():N}@prueba.local",
            Telefono = "3000000000",
            IdRol = 1,
            FechaIngreso = DateTime.Today,
            Activo = true
        };
    }
}

