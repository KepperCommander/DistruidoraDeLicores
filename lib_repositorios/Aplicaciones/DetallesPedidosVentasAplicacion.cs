using System;
using System.Collections.Generic;
using System.Linq;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class DetallesPedidosVentasAplicacion : IDetallesPedidosVentasAplicacion
    {
        private IConexion? IConexion = null;

        public DetallesPedidosVentasAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string stringConexion)
        {
            this.IConexion!.StringConexion = stringConexion;
        }

        // 🔹 Validar campos requeridos
        private static void ValidarRequeridos(DetallesPedidosVentas d)
        {
            if (d.IdPedidoVenta <= 0)
                throw new Exception("lbPedidoVentaRequerido");
            if (d.IdProducto <= 0)
                throw new Exception("lbProductoRequerido");
            if (d.Cantidad <= 0)
                throw new Exception("lbCantidadRequerida");
            if (d.PrecioUnitario <= 0)
                throw new Exception("lbPrecioUnitarioRequerido");
        }

        // 🔹 Guardar nuevo detalle
        public DetallesPedidosVentas? Guardar(DetallesPedidosVentas? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdDetallePV != 0)
                throw new Exception("lbYaSeGuardo");

            ValidarRequeridos(entidad);

            this.IConexion!.DetallesPedidosVentas!.Add(entidad);
            this.IConexion.SaveChanges();

            return entidad;
        }

        // 🔹 Modificar detalle existente
        public DetallesPedidosVentas? Modificar(DetallesPedidosVentas? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdDetallePV == 0)
                throw new Exception("lbNoSeGuardo");

            ValidarRequeridos(entidad);

            var entry = this.IConexion!.Entry(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();

            return entidad;
        }

        // 🔹 Borrar detalle
        public DetallesPedidosVentas? Borrar(DetallesPedidosVentas? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdDetallePV == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.DetallesPedidosVentas!.Remove(entidad);
            this.IConexion.SaveChanges();

            return entidad;
        }

        // 🔹 Listar detalles
        public List<DetallesPedidosVentas> Listar(int take = 100)
        {
            return this.IConexion!.DetallesPedidosVentas!
                .AsNoTracking()
                .Include(x => x.Producto)
                .Include(x => x.Impuesto)
                .Include(x => x.PedidosVenta)
                .OrderBy(x => x.IdDetallePV)
                .Take(take)
                .ToList();
        }

        // 🔹 Buscar por texto (nombre del producto o impuesto)
        public List<DetallesPedidosVentas> PorTexto(string texto, int take = 100)
        {
            texto = (texto ?? "").Trim();
            if (texto.Length == 0) return Listar(take);

            return this.IConexion!.DetallesPedidosVentas!
                .AsNoTracking()
                .Include(x => x.Producto)
                .Include(x => x.Impuesto)
                .Include(x => x.PedidosVenta)
                .Where(x =>
                    x.Producto.Nombre.Contains(texto) ||
                    (x.Impuesto != null && x.Impuesto.Nombre.Contains(texto)))
                .OrderBy(x => x.IdDetallePV)
                .Take(take)
                .ToList();
        }

        // 🔹 Buscar por pedido
        public List<DetallesPedidosVentas> PorPedido(int idPedidoVenta)
        {
            return this.IConexion!.DetallesPedidosVentas!
                .AsNoTracking()
                .Include(x => x.Producto)
                .Include(x => x.Impuesto)
                .Where(x => x.IdPedidoVenta == idPedidoVenta)
                .OrderBy(x => x.IdDetallePV)
                .ToList();
        }

        // 🔹 Buscar por Id
        public DetallesPedidosVentas? PorId(int idDetallePV)
        {
            return this.IConexion!.DetallesPedidosVentas!
                .AsNoTracking()
                .Include(x => x.Producto)
                .Include(x => x.Impuesto)
                .Include(x => x.PedidosVenta)
                .FirstOrDefault(x => x.IdDetallePV == idDetallePV);
        }
    }
}
