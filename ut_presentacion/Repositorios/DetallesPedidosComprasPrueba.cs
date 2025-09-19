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
    public class DetallesPedidosComprasPrueba
    {
        private readonly IConexion iConexion;
        private List<DetallesPedidosCompras>? lista;
        private DetallesPedidosCompras? entidad;

        public DetallesPedidosComprasPrueba()
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
            lista = iConexion.DetallesPedidosCompras!.AsNoTracking().Take(50).ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
            // FK: PedidoCompra + Producto ya sembrados por tu script
            var pc = iConexion.PedidosCompras!.AsNoTracking().FirstOrDefault();
            var prod = iConexion.Productos!.AsNoTracking().FirstOrDefault();

            if (pc is null || prod is null)
                Assert.Inconclusive("No hay PedidoCompra o Producto en la BD para probar DetallesPedidosCompras.");

            entidad = new DetallesPedidosCompras
            {
                IdPedidoCompra = pc!.IdPedidoCompra,
                IdProducto = prod!.IdProducto,
                Cantidad = 3,
                PrecioUnitario = Math.Max(1000m, (prod.PrecioLista > 0 ? prod.PrecioLista : 1000m))
            };

            iConexion.DetallesPedidosCompras!.Add(entidad);
            iConexion.SaveChanges();
            return entidad.IdDetallePC > 0;
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
            iConexion.DetallesPedidosCompras!.Remove(entidad);
            iConexion.SaveChanges();
            return true;
        }
    }
}
