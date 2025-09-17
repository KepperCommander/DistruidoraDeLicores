namespace lib_dominio.Entidades
{
    public class DetallesPedidosCompras
    {
        public int IdDetallePC { get; set; }
        public int IdPedidoCompra { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public PedidoCompra PedidoCompra { get; set; } = null!;
        public Producto Producto { get; set; } = null!;
    }
}