namespace lib_dominio.Entidades
{
    public class DetallesPedidosVentas
    {
        public int IdDetallePV { get; set; }
        public int IdPedidoVenta { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        s
    public decimal PrecioUnitario { get; set; }
        public int? IdImpuesto { get; set; }

        public PedidoVenta PedidoVenta { get; set; } = null!;
        public Producto Producto { get; set; } = null!;
        public Impuesto? Impuesto { get; set; }
    }

}