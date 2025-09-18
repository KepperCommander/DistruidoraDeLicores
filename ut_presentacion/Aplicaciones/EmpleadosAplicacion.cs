using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private Conexion ctx = default!;
        private IDbContextTransaction? trx;
        private IEmpleadosAplicacion app = default!;
        private Empleados? entidad;

        public TestContext TestContext { get; set; } = default!;

        [TestInitialize]
        public void SetUp()
        {
            ctx = new Conexion { StringConexion = Configuracion.ObtenerValor("StringConexion") };
            Assert.IsTrue(ctx.Database.CanConnect(), "No conecta: " + ctx.Database.GetConnectionString());
            Assert.IsNotNull(ctx.Empleados, "DbSet Empleados es null");

            trx = ctx.Database.BeginTransaction();
            app = new EmpleadosAplicacion(ctx);
            app.Configurar(Configuracion.ObtenerValor("StringConexion"));
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
            // Guardar
            entidad = EntidadesNucleo.Empleados() ?? EmpleadoValido();
            // Evita choque por email único
            entidad.Email = $"empleado.{Guid.NewGuid():N}@prueba.local";

            entidad = app.Guardar(entidad);
            Assert.IsNotNull(entidad);
            Assert.IsTrue(entidad!.IdEmpleado > 0);

            // Modificar
            entidad.Apellidos += " Actualizado";
            var mod = app.Modificar(entidad);
            Assert.IsNotNull(mod);

            // Listar + PorRol + PorTexto
            var lista = app.Listar();
            Assert.IsTrue(lista.Count > 0);

            var porRol = app.PorRol(entidad.IdRol);
            Assert.IsTrue(porRol.Count > 0);

            var porTexto = app.PorTexto(entidad.Apellidos.Split(' ').First());
            Assert.IsTrue(porTexto.Count > 0);

            // Borrar
            var borr = app.Borrar(entidad);
            Assert.IsNotNull(borr);
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
