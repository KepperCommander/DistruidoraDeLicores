using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace lib_repositorios.Implementaciones
{
    public partial class Conexion : DbContext, IConexion
    {
        public string? StringConexion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.StringConexion!, p => { });
            //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        public DbSet<Empleados>? Empleados { get; set; }
        public DbSet<Almacenes>? Almacenes { get; set; }
        public DbSet<Roles>? Roles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            modelBuilder.HasDefaultSchema("dbo");

         
            modelBuilder.Entity<Empleados>(e =>
            {
                e.ToTable("Empleados", "dbo");
                e.HasKey(x => x.IdEmpleado);
                e.Property(x => x.IdEmpleado).ValueGeneratedOnAdd();

                e.Property(x => x.Nombres).IsRequired().HasMaxLength(100);
                e.Property(x => x.Apellidos).IsRequired().HasMaxLength(100);
                e.Property(x => x.Email).HasMaxLength(150);
                e.HasIndex(x => x.Email).IsUnique();
                e.Property(x => x.Telefono).HasMaxLength(30);
                e.Property(x => x.FechaIngreso).IsRequired();
                e.Property(x => x.Activo).IsRequired();

                
                e.HasOne<Roles>()             
                 .WithMany()
                 .HasForeignKey(x => x.IdRol)
                 .HasConstraintName("FK_Empleados_Roles");


               
                e.Ignore("RolesIdRol");
            });

            
            modelBuilder.Entity<Roles>(e =>
            {
                e.ToTable("Roles", "dbo");
                e.HasKey(x => x.IdRol);
                e.Property(x => x.IdRol).ValueGeneratedOnAdd();
                e.Property(x => x.Nombre).IsRequired().HasMaxLength(50);
                e.HasIndex(x => x.Nombre).IsUnique();
                e.Property(x => x.EsActivo).IsRequired().HasDefaultValue(true);
            });



            modelBuilder.Entity<Almacenes>(e =>
            {
                e.ToTable("Almacenes");
                e.HasKey(x => x.IdAlmacen);             
                e.Property(x => x.IdAlmacen).ValueGeneratedOnAdd();

                e.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
                e.HasIndex(x => x.Nombre).IsUnique();     
                e.Property(x => x.Direccion).HasMaxLength(200);
                e.Property(x => x.EsPrincipal).IsRequired();
            });

            
        }

    }
}