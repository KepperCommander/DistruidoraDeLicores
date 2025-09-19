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
    public class PedidosVentasPrueba
    {
        private Conexion ctx = default!;
        private IDbContextTransaction? trx;
        private PedidosVentas? entidad;

        public TestContext TestContext { get; set; } = default!;

        [TestInitialize]
        public void SetUp()
        {
            ctx = new Conexion { StringConexion = Configuracion.ObtenerValor("StringConexion") };
            Assert.IsTrue(ctx.Database.CanConnect(), "No se puede conectar a la base de datos");
            Assert.IsNotNull(ctx.PedidosVentas, "DbSet PedidosVentas es null en el DbContext.");
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
                Assert.IsTrue(Borrar(), "Borrar() falló");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        private bool Guardar()
        {
            // Necesitamos un cliente existente
            var cliente = ctx.Clientes!.AsNoTracking().FirstOrDefault()
                          ?? throw new Exception("No hay clientes en la BD. Inserta un cliente primero.");

            // El empleado puede ser null, pero intentamos usar uno si existe
            var empleado = ctx.Empleados!.AsNoTracking().FirstOrDefault();

            entidad = new PedidosVentas
            {
                Numero = $"PV-{Guid.NewGuid().ToString().Substring(0, 6)}",
                IdCliente = cliente.IdCliente,
                IdEmpleado = empleado?.IdEmpleado,
                Fecha = DateTime.Now,
                Estado = "ABIERTA"
            };

            ctx.PedidosVentas!.Add(entidad);
            var afectados = ctx.SaveChanges();

            TestContext.WriteLine($"INSERT IdPedidoVenta = {entidad.IdPedidoVenta}, filas afectadas = {afectados}");
            Assert.IsTrue(afectados >= 1, "No se insertó ningún pedido.");
            Assert.IsTrue(entidad.IdPedidoVenta > 0, "EF no asignó IdPedidoVenta.");

            return true;
        }

        private bool Modificar()
        {
            Assert.IsNotNull(entidad, "No hay entidad cargada para modificar.");

            entidad!.Estado = "FACTURADA"; // cambiamos estado
            var afectados = ctx.SaveChanges();

            TestContext.WriteLine($"UPDATE IdPedidoVenta = {entidad.IdPedidoVenta}, filas afectadas = {afectados}");
            Assert.IsTrue(afectados >= 1, "No se actualizó ningún pedido.");

            return true;
        }

        private bool Listar()
        {
            List<PedidosVentas> lista = ctx.PedidosVentas!
                                          .Include(x => x.Cliente)
                                          .Include(x => x.Empleado)
                                          .AsNoTracking()
                                          .ToList();

            TestContext.WriteLine($"LISTAR total = {lista.Count}");
            return lista.Count > 0;
        }

        private bool Borrar()
        {
            Assert.IsNotNull(entidad, "No hay entidad cargada para eliminar.");

            ctx.PedidosVentas!.Remove(entidad!);
            var afectados = ctx.SaveChanges();

            TestContext.WriteLine($"DELETE IdPedidoVenta = {entidad.IdPedidoVenta}, filas afectadas = {afectados}");
            Assert.IsTrue(afectados >= 1, "No se eliminó ningún pedido.");

            // Verificar que no exista (usamos AsNoTracking por seguridad)
            var eliminado = ctx.PedidosVentas!.AsNoTracking().FirstOrDefault(x => x.IdPedidoVenta == entidad!.IdPedidoVenta);
            Assert.IsNull(eliminado, "El pedido no se eliminó correctamente.");

            return true;
        }
    }
}
