using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public partial class Conexion : DbContext, IConexion
    {
        public string? StringConexion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(StringConexion!, o => { });
            // optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

       
        public DbSet<Roles>? Roles { get; set; }
        public DbSet<Empleados>? Empleados { get; set; }
        public DbSet<Proveedores>? Proveedores { get; set; }
        public DbSet<Clientes>? Clientes { get; set; }
        public DbSet<CategoriasProductos>? CategoriasProductos { get; set; }
        public DbSet<Impuestos>? Impuestos { get; set; }
        public DbSet<Productos>? Productos { get; set; }
        public DbSet<Almacenes>? Almacenes { get; set; }
        public DbSet<InventariosProductos>? InventariosProductos { get; set; }
        public DbSet<MovimientosProductos>? MovimientosProductos { get; set; }
        public DbSet<PedidosCompras>? PedidosCompras { get; set; }
        public DbSet<DetallesPedidosCompras>? DetallesPedidosCompras { get; set; }
        public DbSet<PedidosVentas>? PedidosVentas { get; set; }
        public DbSet<DetallesPedidosVentas>? DetallesPedidosVentas { get; set; }
        public DbSet<Pagos>? Pagos { get; set; }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.HasDefaultSchema("dbo");

            
            model.Entity<Roles>(e =>
            {
                e.ToTable("Roles", "dbo");
                e.HasKey(x => x.IdRol);
                e.Property(x => x.IdRol).ValueGeneratedOnAdd();
                e.Property(x => x.Nombre).IsRequired().HasMaxLength(50);
                e.HasIndex(x => x.Nombre).IsUnique();
                e.Property(x => x.EsActivo).IsRequired().HasDefaultValue(true);
            });

            
            model.Entity<Empleados>(e =>
            {
                e.ToTable("Empleados", "dbo");
                e.HasKey(x => x.IdEmpleado);
                e.Property(x => x.IdEmpleado).ValueGeneratedOnAdd();
                e.Property(x => x.Nombres).IsRequired().HasMaxLength(100);
                e.Property(x => x.Apellidos).IsRequired().HasMaxLength(100);
                e.Property(x => x.Email).HasMaxLength(150);
                e.HasIndex(x => x.Email).IsUnique();
                e.Property(x => x.Telefono).HasMaxLength(30);
                e.Property(x => x.FechaIngreso).HasColumnType("date").IsRequired();
                e.Property(x => x.Activo).IsRequired();

                e.HasOne<Roles>()
                 .WithMany()
                 .HasForeignKey(x => x.IdRol)
                 .HasConstraintName("FK_Empleados_Roles");

                
                e.Ignore("RolesIdRol");
            });

            
            model.Entity<Proveedores>(e =>
            {
                e.ToTable("Proveedores");
                e.HasKey(x => x.IdProveedor);
                e.Property(x => x.IdProveedor).ValueGeneratedOnAdd();
                e.Property(x => x.RazonSocial).IsRequired().HasMaxLength(100);
                e.HasIndex(x => x.RazonSocial).IsUnique();
                e.Property(x => x.NIT).IsRequired().HasMaxLength(20);
                e.Property(x => x.Email).HasMaxLength(150);
                e.HasIndex(x => x.Email).IsUnique();
                e.Property(x => x.Telefono).HasMaxLength(30);
                e.Property(x => x.Direccion).HasMaxLength(200);
                e.Property(x => x.Activo).IsRequired();
            
            });

           
            model.Entity<Clientes>(e =>
            {
                e.ToTable("Clientes", "dbo");
                e.HasKey(x => x.IdCliente);
                e.Property(x => x.IdCliente).ValueGeneratedOnAdd();
                e.Property(x => x.RazonSocial).IsRequired().HasMaxLength(150);
                e.Property(x => x.NIT).IsRequired().HasMaxLength(30);
                e.HasIndex(x => x.NIT).IsUnique();
                e.Property(x => x.Email).HasMaxLength(150);
                e.Property(x => x.Telefono).HasMaxLength(30);
                e.Property(x => x.Direccion).HasMaxLength(200);
                e.Property(x => x.Activo).IsRequired().HasDefaultValue(true);
            });

            
            model.Entity<CategoriasProductos>(e =>
            {
                e.ToTable("CategoriasProductos", "dbo");
                e.HasKey(x => x.IdCategoria);
                e.Property(x => x.IdCategoria).ValueGeneratedOnAdd();
                e.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
                e.HasIndex(x => x.Nombre).IsUnique();
                e.Property(x => x.Descripcion).HasMaxLength(250);
            });

            
            model.Entity<Impuestos>(e =>
            {
                e.ToTable("Impuestos", "dbo");
                e.HasKey(x => x.IdImpuesto);
                e.Property(x => x.IdImpuesto).ValueGeneratedOnAdd();
                e.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
                e.HasIndex(x => x.Nombre).IsUnique();
                e.Property(x => x.Porcentaje).HasColumnType("decimal(5,2)").IsRequired();
                e.HasCheckConstraint("CK_Impuestos_Porcentaje", "[Porcentaje] BETWEEN 0 AND 100");
            });


            model.Entity<Productos>(e =>
            {
                e.ToTable("Productos", "dbo");
                e.HasKey(x => x.IdProducto);
                e.Property(x => x.IdProducto).ValueGeneratedOnAdd();

                e.Property(x => x.Codigo).IsRequired().HasMaxLength(40);
                e.HasIndex(x => x.Codigo).IsUnique();
                e.Property(x => x.Nombre).IsRequired().HasMaxLength(150);
                e.Property(x => x.VolumenML).IsRequired();
                e.Property(x => x.GradAlcoholico).HasColumnType("decimal(5,2)").IsRequired();
                e.Property(x => x.PrecioLista).HasColumnType("decimal(12,2)").IsRequired();
                e.Property(x => x.Activo).IsRequired();

                // Relación con CategoriasProductos (usa la colección explícita)
                e.HasOne(p => p.Categoria)
                 .WithMany(c => c.Productos)
                 .HasForeignKey(p => p.IdCategoria)
                 .HasConstraintName("FK_Prod_Cat");

                // Relación con Impuestos (usa la colección explícita)
                e.HasOne(p => p.Impuesto)
                 .WithMany(i => i.Productos)
                 .HasForeignKey(p => p.IdImpuesto)
                 .HasConstraintName("FK_Prod_Imp");
            });





            model.Entity<Almacenes>(e =>
            {
                e.ToTable("Almacenes", "dbo");
                e.HasKey(x => x.IdAlmacen);
                e.Property(x => x.IdAlmacen).ValueGeneratedOnAdd();
                e.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
                e.HasIndex(x => x.Nombre).IsUnique();
                e.Property(x => x.Direccion).HasMaxLength(200);
                e.Property(x => x.EsPrincipal).IsRequired().HasDefaultValue(false);
            });

            
            model.Entity<InventariosProductos>(e =>
            {
                e.ToTable("InventariosProductos", "dbo");
                e.HasKey(x => x.IdInventarioProd);
                e.Property(x => x.IdInventarioProd).ValueGeneratedOnAdd();
                e.Property(x => x.CantidadUnidades).IsRequired();
                e.HasCheckConstraint("CK_InvProd_CantidadUnidades", "[CantidadUnidades] >= 0");

                e.HasIndex(x => new { x.IdProducto, x.IdAlmacen }).IsUnique()
                 .HasDatabaseName("UQ_InvProd");

                e.HasOne<Productos>()
                 .WithMany()
                 .HasForeignKey(x => x.IdProducto)
                 .HasConstraintName("FK_InvProd_Prod");

                e.HasOne<Almacenes>()
                 .WithMany()
                 .HasForeignKey(x => x.IdAlmacen)
                 .HasConstraintName("FK_InvProd_Alm");
            });

           
            model.Entity<MovimientosProductos>(e =>
            {
                e.ToTable("MovimientosProductos");
                e.HasKey(x => x.IdMovProd);
                e.Property(x => x.IdMovProd).ValueGeneratedOnAdd();
                e.Property(x => x.Fecha).IsRequired();
                e.Property(x => x.Tipo).IsRequired().HasMaxLength(20);
                e.Property(x => x.CantidadUnidades).IsRequired();
                e.Property(x => x.Referencia).HasMaxLength(100);

                e.HasOne(x => x.Productos)
                    .WithMany()
                    .HasForeignKey(x => x.IdProducto);

                e.HasOne(x => x.Almacenes)
                    .WithMany()
                    .HasForeignKey(x => x.IdAlmacen);
            });

            
            model.Entity<PedidosCompras>(e =>
            {
                e.ToTable("PedidosCompras");
                e.HasKey(x => x.IdPedidoCompra);
                e.Property(x => x.IdPedidoCompra).ValueGeneratedOnAdd();
                e.Property(x => x.Numero).IsRequired().HasMaxLength(20);
                e.HasIndex(x => x.Numero).IsUnique();
                e.Property(x => x.Fecha).IsRequired();
                e.Property(x => x.Estado).IsRequired().HasMaxLength(20);

                e.HasOne(x => x.Proveedor)
                    .WithMany()
                    .HasForeignKey(x => x.IdProveedor);

                e.HasOne(x => x.Empleado)
                    .WithMany()
                    .HasForeignKey(x => x.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });



            // ===== DetallesPedidosCompras =====
            model.Entity<DetallesPedidosCompras>(e =>
            {
                e.ToTable("DetallesPedidosCompras", "dbo");
                e.HasKey(x => x.IdDetallePC);
                e.Property(x => x.IdDetallePC).ValueGeneratedOnAdd();

                e.Property(x => x.Cantidad).IsRequired();
                e.Property(x => x.PrecioUnitario).HasColumnType("decimal(12,2)").IsRequired();

                // Enlaza la navegación PedidoCompra con la FK REAL IdPedidoCompra
                e.HasOne(d => d.PedidoCompra)      // (tipo PedidosCompras o PedidoCompras, según tu clase)
                 .WithMany()                       // o .WithMany(pc => pc.Detalles) si agregas la colección
                 .HasForeignKey(d => d.IdPedidoCompra)
                 .HasConstraintName("FK_DPC_PC");

                // Enlaza la navegación Producto con la FK REAL IdProducto
                e.HasOne(d => d.Producto)
                 .WithMany()                       // o .WithMany(p => p.DetallesPedidosCompras) si defines colección
                 .HasForeignKey(d => d.IdProducto)
                 .HasConstraintName("FK_DPC_Prod");

                // Solo si antes probaste cosas y quedaron sombras, limpia una vez:
                e.Ignore("PedidosComprasIdPedidoCompra");
                e.Ignore("ProductosIdProducto");
                e.Ignore("PedidoComprasIdPedidoCompra"); // por si el tipo se llama distinto
            });


            // ===== PedidosVentas =====
            // PedidosVentas
            model.Entity<PedidosVentas>(e =>
            {
                e.ToTable("PedidosVentas", "dbo");
                e.HasKey(x => x.IdPedidoVenta);
                e.Property(x => x.IdPedidoVenta).ValueGeneratedOnAdd();

                e.Property(x => x.Numero).IsRequired().HasMaxLength(30);
                e.HasIndex(x => x.Numero).IsUnique();
                e.Property(x => x.Estado).IsRequired().HasMaxLength(20);

                // Relación con Clientes usando la colección inversa
                e.HasOne(x => x.Cliente)
                 .WithMany(c => c.PedidosVentas)        // <— importante
                 .HasForeignKey(x => x.IdCliente)
                 .HasConstraintName("FK_PV_Cliente");

                // Relación con Empleados usando la colección inversa (si existe)
                e.HasOne(x => x.Empleado)
                 .WithMany(emp => emp.PedidosVentas)    // <— importante
                 .HasForeignKey(x => x.IdEmpleado)
                 .HasConstraintName("FK_PV_Emp");
            });



            // ===== DetallesPedidosVentas =====
            model.Entity<DetallesPedidosVentas>(e =>
            {
                e.ToTable("DetallesPedidosVentas", "dbo");
                e.HasKey(x => x.IdDetallePV);
                e.Property(x => x.IdDetallePV).ValueGeneratedOnAdd();

                e.Property(x => x.Cantidad).IsRequired();
                e.Property(x => x.PrecioUnitario).HasColumnType("decimal(12,2)").IsRequired();

                e.HasOne(x => x.PedidosVenta)
                 .WithMany() // o .WithMany(p => p.Detalles) si agregas la colección en PedidosVentas
                 .HasForeignKey(x => x.IdPedidoVenta)
                 .HasConstraintName("FK_DPV_PV");

                e.HasOne(x => x.Producto)
                 .WithMany()
                 .HasForeignKey(x => x.IdProducto)
                 .HasConstraintName("FK_DPV_Prod");

                e.HasOne(x => x.Impuesto)
                 .WithMany()
                 .HasForeignKey(x => x.IdImpuesto)
                 .HasConstraintName("FK_DPV_Imp");
            });





            model.Entity<Pagos>(e =>
            {
                e.ToTable("Pagos", "dbo");
                e.HasKey(x => x.IdPago);
                e.Property(x => x.IdPago).ValueGeneratedOnAdd();
                e.Property(x => x.Fecha).IsRequired();
                e.Property(x => x.Monto).HasColumnType("decimal(12,2)").IsRequired();
                e.Property(x => x.Medio).IsRequired().HasMaxLength(30);
                e.Property(x => x.Referencia).HasMaxLength(100);
                e.HasCheckConstraint("CK_Pagos_Monto", "[Monto] > 0");
                e.HasCheckConstraint("CK_Pagos_Medio", "[Medio] IN ('EFECTIVO','TRANSFERENCIA','TARJETA','OTRO')");

                e.HasOne<PedidosVentas>()
                 .WithMany()
                 .HasForeignKey(x => x.IdPedidoVenta)
                 .HasConstraintName("FK_Pagos_PV");
            });
        }
    }
}

