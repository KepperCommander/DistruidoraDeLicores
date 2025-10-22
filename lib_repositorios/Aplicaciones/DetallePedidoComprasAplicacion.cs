using System;
using System.Collections.Generic;
using System.Linq;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class DetallesPedidosComprasAplicacion : IDetallesPedidosComprasAplicacion
    {
        private IConexion? IConexion;

        public void Configurar(string stringConexion)
        {
            this.IConexion = new Conexion
            {
                StringConexion = stringConexion
            };
        }

        // 🔹 Validar datos requeridos
        private static void ValidarRequeridos(DetallesPedidosCompras entidad)
        {
            if (entidad.IdPedidoCompra <= 0)
                throw new Exception("El pedido de compra es obligatorio.");

            if (entidad.IdProducto <= 0)
                throw new Exception("El producto es obligatorio.");

            if (entidad.Cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor que cero.");

            if (entidad.PrecioUnitario <= 0)
                throw new Exception("El precio unitario debe ser mayor que cero.");
        }

        // 🔹 Validar que el detalle no se duplique (pedido + producto)
        private void ValidarDuplicado(DetallesPedidosCompras entidad, bool esModificar)
        {
            var query = IConexion!.DetallesPedidosCompras!
                .AsNoTracking()
                .Where(x => x.IdPedidoCompra == entidad.IdPedidoCompra &&
                            x.IdProducto == entidad.IdProducto);

            if (esModificar)
                query = query.Where(x => x.IdDetallePC != entidad.IdDetallePC);

            if (query.Any())
                throw new Exception("Ya existe un detalle para este producto en el pedido.");
        }

        // 🔹 Guardar nuevo detalle
        public DetallesPedidosCompras Guardar(DetallesPedidosCompras entidad)
        {
            if (entidad == null)
                throw new Exception("No se recibió información del detalle.");

            if (entidad.IdDetallePC != 0)
                throw new Exception("El detalle ya existe.");

            ValidarRequeridos(entidad);
            ValidarDuplicado(entidad, false);

            IConexion!.DetallesPedidosCompras!.Add(entidad);
            IConexion.SaveChanges();

            return entidad;
        }

        // 🔹 Modificar detalle existente
        public DetallesPedidosCompras Modificar(DetallesPedidosCompras entidad)
        {
            if (entidad == null)
                throw new Exception("No se recibió información del detalle.");

            if (entidad.IdDetallePC == 0)
                throw new Exception("No se puede modificar un detalle sin ID.");

            ValidarRequeridos(entidad);
            ValidarDuplicado(entidad, true);

            var entry = IConexion!.Entry(entidad);
            entry.State = EntityState.Modified;
            IConexion.SaveChanges();

            return entidad;
        }

        // 🔹 Borrar detalle
        public DetallesPedidosCompras Borrar(DetallesPedidosCompras entidad)
        {
            if (entidad == null)
                throw new Exception("No se recibió información del detalle.");

            if (entidad.IdDetallePC == 0)
                throw new Exception("No se puede eliminar un detalle sin ID.");

            IConexion!.DetallesPedidosCompras!.Remove(entidad);
            IConexion.SaveChanges();

            return entidad;
        }

        // 🔹 Listar detalles (limitado)
        public List<DetallesPedidosCompras> Listar(int take = 100)
        {
            return IConexion!.DetallesPedidosCompras!
                .AsNoTracking()
                .Include(x => x.Producto)
                .Include(x => x.PedidoCompra)
                .OrderBy(x => x.IdDetallePC)
                .Take(take)
                .ToList();
        }

        // 🔹 Obtener detalles por pedido
        public List<DetallesPedidosCompras> PorPedido(int idPedidoCompra)
        {
            return IConexion!.DetallesPedidosCompras!
                .AsNoTracking()
                .Include(x => x.Producto)
                .Where(x => x.IdPedidoCompra == idPedidoCompra)
                .OrderBy(x => x.Producto.Nombre)
                .ToList();
        }

        // 🔹 Obtener detalles por producto
        public List<DetallesPedidosCompras> PorProducto(int idProducto)
        {
            return IConexion!.DetallesPedidosCompras!
                .AsNoTracking()
                .Include(x => x.PedidoCompra)
                .Where(x => x.IdProducto == idProducto)
                .OrderByDescending(x => x.IdPedidoCompra)
                .ToList();
        }

        // 🔹 Buscar por ID
        public DetallesPedidosCompras? PorId(int idDetallePC)
        {
            return IConexion!.DetallesPedidosCompras!
                .AsNoTracking()
                .Include(x => x.Producto)
                .Include(x => x.PedidoCompra)
                .FirstOrDefault(x => x.IdDetallePC == idDetallePC);
        }
    }
}
