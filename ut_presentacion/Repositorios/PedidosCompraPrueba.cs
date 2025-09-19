using System;
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
    public class PedidosComprasPrueba
    {
        private Conexion ctx = default!;
        private IDbContextTransaction? trx;
        private PedidosCompras? entidad;

        [TestInitialize]
        public void SetUp()
        {
            ctx = new Conexion { StringConexion = Configuracion.ObtenerValor("StringConexion") };
            Assert.IsTrue(ctx.Database.CanConnect());
            Assert.IsNotNull(ctx.PedidosCompras);
            trx = ctx.Database.BeginTransaction();
        }

        [TestCleanup]
        public void TearDown()
        {
            try { trx?.Rollback(); } catch { }
            try { ctx?.Dispose(); } catch { }
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.IsTrue(Guardar());
            Assert.IsTrue(Modificar());
            Assert.IsTrue(Listar());
        }

        private bool Guardar()
        {
            entidad = EntidadesNucleo.PedidoCompras() ?? PedidoValido();
            entidad.Numero = $"PC-{Guid.NewGuid():N}".Substring(0, 10);

            ctx.PedidosCompras!.Add(entidad);
            return ctx.SaveChanges() >= 1 && entidad.IdPedidoCompra > 0;
        }

        private bool Modificar()
        {
            entidad!.Estado = "CERRADA";
            return ctx.SaveChanges() >= 1;
        }

        private bool Listar()
        {
            return ctx.PedidosCompras!.AsNoTracking().Any();
        }

        private static PedidosCompras PedidoValido() => new PedidosCompras
        {
            Numero = $"PC-{Guid.NewGuid():N}".Substring(0, 10),
            IdProveedor = 1,
            IdEmpleado = 1,
            Fecha = DateTime.Now,
            Estado = "ABIERTA"
        };
    }
}