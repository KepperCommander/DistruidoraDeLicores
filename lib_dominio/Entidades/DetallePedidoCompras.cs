using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Entidades
{
    public class DetallesPedidosCompras
    {
        public int IdDetallePC { get; set; }
        public int IdPedidoCompra { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public PedidosCompras PedidoCompra { get; set; } = null!;
        public Productos Producto { get; set; } = null!;
    }
}
