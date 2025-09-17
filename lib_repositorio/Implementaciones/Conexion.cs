using lib_dominio.Entidades;
using lib_repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public partial class Conexion : DbContext, IConexion
    {
        public string? StringConexion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.StringConexion!, p => { });
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        public DbSet<Productos>? Productos { get; set; }
        public DbSet<Empleados>? Empleados { get; set; }
        public DbSet<Clientes>? Clientes { get; set; }
        public DbSet<Proveedores>? Proveedores { get; set; }
        public DbSet<PedidosVentas>? PedidosVentas { get; set; }
        public DbSet<PedidosCompras>? PedidosCompras { get; set; }
        public DbSet<CategoriasProductos>? CategoriasProducto { get; set; }
        public DbSet<Impuestos>? Impuestos { get; set; }
        public DbSet<Almacenes>? Almacenes { get; set; }
        public DbSet<InventariosProductos>? InventariosProductos { get; set; }
        public DbSet<MovimientosProductos>? MovimientosProductos { get; set; }
        public DbSet<DetallesPedidosVentas>? DetallesPedidoVentas { get; set; }
        public DbSet<DetallesPedidosCompras>? DetallesPedidosCompras { get; set; }
        public DbSet<Pagos>? Pagos { get; set; }
        public DbSet<Roles>? Roles { get; set; }
    }
}
