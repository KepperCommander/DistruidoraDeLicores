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
    public class MovimientoProductosPrueba
    {
        private Conexion ctx = default!;
        private IDbContextTransaction? trx;
        private MovimientosProductos? entidad;

        [TestInitialize]
        public void SetUp()
        {
            ctx = new Conexion { StringConexion = Configuracion.ObtenerValor("StringConexion") };
            Assert.IsTrue(ctx.Database.CanConnect());
            Assert.IsNotNull(ctx.MovimientosProductos);
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
            entidad = EntidadesNucleo.MovimientoProductos() ?? MovimientoValido();
            ctx.MovimientosProductos!.Add(entidad);
            return ctx.SaveChanges() >= 1 && entidad.IdMovProd > 0;
        }

        private bool Modificar()
        {
            entidad!.Referencia = "Ref actualizada";
            return ctx.SaveChanges() >= 1;
        }

        private bool Listar()
        {
            return ctx.MovimientosProductos!.AsNoTracking().Any();
        }

        private static MovimientosProductos MovimientoValido() => new MovimientosProductos
        {
            IdProducto = 1,
            IdAlmacen = 1,
            Fecha = DateTime.Now,
            Tipo = "ENTRADA",
            CantidadUnidades = 50,
            Referencia = "Prueba inicial"
        };
    }
}

