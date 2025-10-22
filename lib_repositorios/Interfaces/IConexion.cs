using lib_dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace lib_repositorios.Interfaces
{
    public interface IConexion
    {
        string? StringConexion { get; set; }

        DbSet<Empleados>? Empleados { get; set; }
        DbSet<Almacenes>? Almacenes { get; set; }
        DbSet<CategoriasProductos>? CategoriasProductos { get; set; }
        DbSet<Productos>? Productos { get; set; }
        DbSet<PedidosCompras>? PedidosCompras { get; set; }     
        DbSet<PedidosVentas>? PedidosVentas { get; set; }

        DbSet<Clientes>? Clientes { get; set; }
        DbSet<Impuestos>? Impuestos { get; set; }

        DbSet<DetallesPedidosCompras>? DetallesPedidosCompras { get; set; }
        DbSet<DetallesPedidosVentas>? DetallesPedidosVentas { get; set; }
        DbSet<InventariosProductos>? InventariosProductos { get; set; }

        DbSet<Roles>? Roles { get; set; }
        

        EntityEntry<T> Entry<T>(T entity) where T : class;
        int SaveChanges();
    }
}
