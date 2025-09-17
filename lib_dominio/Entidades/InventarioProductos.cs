namespace lib_dominio.Entidades
{
	public class InventariosProductos
	{
		public int IdInventarioProd { get; set; }
		public int IdProducto { get; set; }
		public int IdAlmacen { get; set; }
		public int CantidadUnidades { get; set; }

		public Producto Producto { get; set; } = null!;
		public Almacen Almacen { get; set; } = null!;
	}
}