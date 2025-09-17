namespace lib_dominio.Entidades
{
    public class MovimientosProductos
    {
        public int IdMovProd { get; set; }
        public int IdProducto { get; set; }
        public int IdAlmacen { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Tipo { get; set; } = null!;
        public int CantidadUnidades { get; set; }
        public string? Referencia { get; set; }

        public Producto Producto { get; set; } = null!;
        public Almacen Almacen { get; set; } = null!;
    }
}