using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Entidades
{
    public class DetallesPedidosVentas
    {
        public int IdDetallePV { get; set; }
        public int IdPedidoVenta { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int? IdImpuesto { get; set; }

        public PedidosVentas PedidosVenta { get; set; } = null!;
        public Productos Producto { get; set; } = null!;
        public Impuestos? Impuesto { get; set; }
    }


}