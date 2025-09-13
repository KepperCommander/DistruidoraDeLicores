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

        
        public DbSet<Productos>? Productos { get; set; }
        public DbSet<Empleados>? Empleados { get; set; }
        public DbSet<Clientes>? Clientes { get; set; }
        public DbSet<Proveedores>? Proveedores { get; set; }
        public DbSet<PedidosVentas>? PedidosVentas { get; set; }
        public DbSet<PedidosCompras>? PedidosCompras { get; set; }
        public DbSet<CategoriasProductos>? CategoriasProductos { get; set; }
        public DbSet<Impuestos>? Impuestos { get; set; }
        public DbSet<Almacenes>? Almacenes { get; set; }
        public DbSet<InventariosProductos>? InventariosProductos { get; set; }
        public DbSet<MovimientosProductos>? MovimientosProductos { get; set; }
        public DbSet<DetallesPedidosVentas>? DetallesPedidoVentas { get; set; }
        public DbSet<DetallePedidoCompra>? DetallesPedidosCompras { get; set; }
        public DbSet<Pagos>? Pagos { get; set; }
        public DbSet<Roles>? Roles { get; set; }
    }
}
