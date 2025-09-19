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
    public class PagosPrueba
    {
        private Conexion ctx = default!;
        private IDbContextTransaction? trx;
        private Pagos? entidad;

        public TestContext TestContext { get; set; } = default!;

        [TestInitialize]
        public void SetUp()
        {
            ctx = new Conexion { StringConexion = Configuracion.ObtenerValor("StringConexion") };
            Assert.IsTrue(ctx.Database.CanConnect(), "No se puede conectar a la base de datos.");
            Assert.IsNotNull(ctx.Pagos, "DbSet<Pagos> es null.");
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
        public void EjecutarFlujoPagos()
        {
            try
            {
                Guardar();
                Modificar();
                Listar();
                Borrar();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        private void Guardar()
        {
            var pedido = ctx.PedidosVentas!.AsNoTracking().FirstOrDefault()
                         ?? throw new Exception("No hay pedidos de venta. Inserta uno primero.");

            entidad = new Pagos
            {
                IdPedidoVenta = pedido.IdPedidoVenta,
                Fecha = DateTime.Now,
                Monto = 150000,
                Medio = "EFECTIVO",
                Referencia = "PRUEBA-" + Guid.NewGuid().ToString().Substring(0, 6)
            };

            ctx.Pagos!.Add(entidad);
            var afectados = ctx.SaveChanges();

            TestContext.WriteLine($"INSERT IdPago = {entidad.IdPago}, filas afectadas = {afectados}");
            Assert.IsTrue(afectados >= 1, "No se insertó ningún pago.");
            Assert.IsTrue(entidad.IdPago > 0, "EF no asignó IdPago.");
        }

        private void Modificar()
        {
            Assert.IsNotNull(entidad, "Entidad null en Modificar().");

            entidad!.Medio = "TRANSFERENCIA";
            entidad.Monto = 200000;

            var afectados = ctx.SaveChanges();

            TestContext.WriteLine($"UPDATE IdPago = {entidad.IdPago}, filas afectadas = {afectados}");
            Assert.IsTrue(afectados >= 1, "No se actualizó el pago.");
        }

        private void Listar()
        {
            List<Pagos> lista = ctx.Pagos!
                                   .Include(x => x.PedidosVentas)
                                   .AsNoTracking()
                                   .ToList();

            TestContext.WriteLine($"LISTAR total = {lista.Count}");
            Assert.IsTrue(lista.Count > 0, "La lista de pagos está vacía.");
        }

        private void Borrar()
        {
            Assert.IsNotNull(entidad, "Entidad null en Borrar().");

            ctx.Pagos!.Remove(entidad!);
            var afectados = ctx.SaveChanges();

            TestContext.WriteLine($"DELETE IdPago = {entidad.IdPago}, filas afectadas = {afectados}");
            Assert.IsTrue(afectados >= 1, "No se eliminó el pago.");

            var eliminado = ctx.Pagos!.AsNoTracking().FirstOrDefault(x => x.IdPago == entidad!.IdPago);
            Assert.IsNull(eliminado, "El pago no se eliminó correctamente.");
        }
    }
}