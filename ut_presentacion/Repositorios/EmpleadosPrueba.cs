using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using lib_repositorios.Implementaciones;

using ut_presentacion.Nucleo; // Configuracion y (opcional) EntidadesNucleo

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class EmpleadosPrueba
    {
        private Conexion ctx = default!;
        private IDbContextTransaction? trx;
        private Empleados? entidad;

        public TestContext TestContext { get; set; } = default!;

        [TestInitialize]
        public void SetUp()
        {
            // Configura la conexión a la BD de TU entorno (DISTRIBUIDORA_2)
            ctx = new Conexion
            {
                StringConexion = Configuracion.ObtenerValor("StringConexion") // debe apuntar a DISTRIBUIDORA_2
            };

            // Sanity checks de arranque
            Assert.IsTrue(ctx.Database.CanConnect(), "No se puede conectar: " + ctx.Database.GetConnectionString());
            Assert.IsNotNull(ctx.Empleados, "DbSet Empleados es null en el DbContext.");

            // Crea el esquema si estás usando una BD de pruebas vacía
            // (en productivo normalmente NO se usa EnsureCreated)
            // ctx.Database.EnsureCreated();

            // Ejecutar el test dentro de una transacción y revertir al final
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
                // Paso 1: Guardar
                Assert.IsTrue(Guardar(), "Guardar() falló");

                // Paso 2: Modificar
                Assert.IsTrue(Modificar(), "Modificar() falló");

                // Paso 3: Listar
                Assert.IsTrue(Listar(), "Listar() falló");
            }
            catch (Exception ex)
            {
                // Mostrar error completo (mensaje + stack + inner exceptions)
                Assert.Fail(ex.ToString());
            }
        }

        private bool Guardar()
        {
            // Usa tu factoría si ya la tienes; si no, crea una entidad válida aquí
            entidad = EntidadesNucleo.Empleados() ?? EmpleadoValido();

            // Asegura datos que cumplan tu esquema SQL:
            // - IdRol NOT NULL y existente (tu script inserta 5 roles, usa 1..5)
            if (entidad.IdRol <= 0) entidad.IdRol = 1;

            // - Email UNIQUE (evitar colisiones)
            if (string.IsNullOrWhiteSpace(entidad.Email))
                entidad.Email = $"empleado.{Guid.NewGuid():N}@prueba.local";

            ctx.Empleados!.Add(entidad);
            var afectados = ctx.SaveChanges();

            TestContext.WriteLine($"INSERT IdEmpleado = {entidad.IdEmpleado}, filas afectadas = {afectados}");

            Assert.IsTrue(afectados >= 1, "No se insertó ningún registro.");
            Assert.IsTrue(entidad.IdEmpleado > 0, "EF no asignó IdEmpleado.");

            return true;
        }

        private bool Modificar()
        {
            Assert.IsNotNull(entidad, "No hay entidad cargada para modificar.");

            entidad!.Apellidos += " Actualizado";
            entidad.Activo = !entidad.Activo;

            // La entidad está trackeada por el mismo DbContext que la insertó.
            // No es necesario forzar EntityState.Modified.
            var afectados = ctx.SaveChanges();

            TestContext.WriteLine($"UPDATE IdEmpleado = {entidad.IdEmpleado}, filas afectadas = {afectados}");
            Assert.IsTrue(afectados >= 1, "No se actualizó ningún registro.");

            return true;
        }

        private bool Listar()
        {
            List<Empleados> lista = ctx.Empleados!.AsNoTracking().ToList();
            TestContext.WriteLine($"LISTAR total = {lista.Count}");
            return lista.Count > 0;
        }

        // Limpieza manual si alguna vez quieres borrar el registro explícitamente.
        // En esta versión no es necesario porque hacemos ROLLBACK en TestCleanup.
        private bool Borrar()
        {
            if (entidad is null) return true;
            ctx.Empleados!.Remove(entidad);
            var afectados = ctx.SaveChanges();
            TestContext.WriteLine($"DELETE IdEmpleado = {entidad.IdEmpleado}, filas afectadas = {afectados}");
            return afectados >= 1;
        }

        // Fábrica mínima para garantizar compatibilidad con el esquema SQL
        private static Empleados EmpleadoValido()
        {
            return new Empleados
            {
                Nombres = "EmpleadoPrueba",
                Apellidos = "Unit",
                Email = $"empleado.{Guid.NewGuid():N}@prueba.local",
                Telefono = "3000000000",
                IdRol = 1,                // Debe existir en Roles
                FechaIngreso = DateTime.Today,
                Activo = true
            };
        }
    }
}
