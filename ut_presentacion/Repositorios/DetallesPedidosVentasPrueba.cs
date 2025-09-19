using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class DetallesPedidosVentasPrueba
    {
        private readonly IConexion iConexion;
        private List<DetallesPedidosVentas>? lista;
        private DetallesPedidosVentas? entidad;

        public DetallesPedidosVentasPrueba()
        {
            iConexion = new Conexion { StringConexion = Configuracion.ObtenerValor("StringConexion") };
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, Guardar());
            Assert.AreEqual(true, Modificar());
            Assert.AreEqual(true, Listar());
            Assert.AreEqual(true, Borrar());
        }

        public bool Listar()
        {
            lista = iConexion.DetallesPedidosVentas!.AsNoTracking().Take(50).ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
            // FKs: PedidoVenta + Producto (y opcionalmente un Impuesto)
            var pv = iConexion.PedidosVentas!.AsNoTracking().FirstOrDefault();
            var prod = iConexion.Productos!.AsNoTracking().FirstOrDefault();
            var imp = iConexion.Impuestos!.AsNoTracking().FirstOrDefault(); // puede ser null

            if (pv is null || prod is null)
                Assert.Inconclusive("No hay pedidoVenta o algun producto en la base  para probar DetallesPedidosVentas.");

            entidad = new DetallesPedidosVentas
            {
                IdPedidoVenta = pv!.IdPedidoVenta,
                IdProducto = prod!.IdProducto,
                Cantidad = 2,
                PrecioUnitario = Math.Max(1000m, (prod.PrecioLista > 0 ? prod.PrecioLista : 1000m)),
                IdImpuesto = imp?.IdImpuesto // puedes dejar null si quieres probar sin impuesto
            };

            iConexion.DetallesPedidosVentas!.Add(entidad);
            iConexion.SaveChanges();
            return entidad.IdDetallePV > 0;
        }

        public bool Modificar()
        {
            entidad!.Cantidad += 1;
            entidad.PrecioUnitario = entidad.PrecioUnitario + 500m;

            var entry = iConexion.Entry(entidad);
            entry.State = EntityState.Modified;
            iConexion.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            if (entidad is null) return true;
            iConexion.DetallesPedidosVentas!.Remove(entidad);
            iConexion.SaveChanges();
            return true;
        }
    }
}
