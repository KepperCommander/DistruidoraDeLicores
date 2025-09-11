using Microsoft.EntityFrameworkCore;
using ConsoleApp.Models;

namespace ConsoleApp.Conexion
{
    public class Conexion : DbContext
    {
        public string? Cadena_conexion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured && !string.IsNullOrWhiteSpace(Cadena_conexion))
            {
                optionsBuilder.UseSqlServer(Cadena_conexion, opt => opt.EnableRetryOnFailure());
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
        }

        // Aquí defines los DbSet según tus entidades reales
        public DbSet<Producto>? Productos { get; set; }
        public DbSet<Empleado>? Empleados { get; set; }
        public DbSet<Cliente>? Clientes { get; set; }
        public DbSet<Proveedor>? Proveedores { get; set; }
        public DbSet<PedidoVenta>? PedidosVentas { get; set; }
        public DbSet<PedidoCompra>? PedidosCompras { get; set; }
        public DbSet<CategoriaProducto>? CategoriasProducto { get; set; }
        public DbSet<Impuesto>? Impuestos { get; set; }
        public DbSet<Almacen>? Almacenes { get; set; }
        public DbSet<InventarioProducto>? InventariosProductos { get; set; }
        public DbSet<MovimientoProducto>? MovimientosProductos { get; set; }
        public DbSet<DetallePedidoVenta>? DetallesPedidoVentas { get; set; }
        public DbSet<DetallePedidoCompra>? DetallesPedidosCompras { get; set; }
        public DbSet<Pago>? Pagos { get; set; }
        public DbSet<Rol>? Roles { get; set; }
    }
}
