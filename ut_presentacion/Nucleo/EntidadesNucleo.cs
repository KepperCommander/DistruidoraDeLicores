using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;



namespace ut_presentacion.Nucleo
{
    public static class EntidadesNucleo
    {
        public static Roles? Roles()
        {
            return new Roles
            {
                Nombre = "Administrador",
                EsActivo = true
            };
        }

        public static Empleados? Empleados()
        {
            return new Empleados
            {
                Nombres = "EmpleadoPrueba",
                Apellidos = "Unit",
                // Siembra por defecto 5 roles (1..5) en tu script; usa uno existente
                IdRol = 1,
                // Evita choque con UNIQUE
                Email = $"empleado.{Guid.NewGuid():N}@prueba.local",
                Telefono = "3000000000",
                Activo = true,
                // FechaIngreso tiene DEFAULT en SQL; puedes enviar DateTime.Today sin problema
                FechaIngreso = DateTime.Today
            };
        }

        public static Almacenes? Almacenes()
        {
            return new Almacenes
            {
                Nombre = "Bodega Central",
                Direccion = "Zona Industrial #1",
                EsPrincipal = true
            };
        }

        public static Proveedores? Proveedores()
        {
            return new Proveedores
            {
                RazonSocial = "Proveedor A",
                NIT = Guid.NewGuid().ToString("N").Substring(0, 9),
                Email = $"prov_{Guid.NewGuid():N}@correo.com",
                Telefono = "3101111111",
                Direccion = "Cra 1 #10-20",
                Activo = true
            };
        }

        public static Clientes? Clientes()
        {
            return new Clientes
            {
                RazonSocial = "Cliente A",
                NIT = Guid.NewGuid().ToString("N").Substring(0, 9),
                Email = $"cli_{Guid.NewGuid():N}@correo.com",
                Telefono = "3201111111",
                Direccion = "Cll 10 #20-30",
                Activo = true
            };
        }

        public static CategoriasProductos? CategoriaProductos()
        {
            return new CategoriasProductos
            {
                Nombre = "Cerveza",
                Descripcion = "Cervezas nacionales e importadas"
            };
        }

        public static Impuestos? Impuestos()
        {
            return new Impuestos
            {
                Nombre = "IVA 19%",
                Porcentaje = 19.0m
            };
        }

        public static Productos? Productos()
        {
            return new Productos
            {
                SKU = "P001",
                Nombre = "Cerveza Águila",
                IdCategoria = 1,   // debe existir en CategoriaProductos
                IdImpuesto = 1,    // debe existir en Impuestos
                VolumenML = 330,
                GradAlcoholico = 4.5m,
                PrecioLista = 2500,
                Activo = true
            };
        }

        

        public static InventariosProductos? InventarioProductos()
        {
            return new InventariosProductos
            {
                IdProducto = 1,
                IdAlmacen = 1,
                CantidadUnidades = 500
            };
        }

        public static MovimientosProductos? MovimientoProductos()
        {
            return new MovimientosProductos
            {
                IdProducto = 1,
                IdAlmacen = 1,
                Fecha = DateTime.Now,
                Tipo = "ENTRADA",
                CantidadUnidades = 100,
                Referencia = "Compra inicial"
            };
        }

        public static PedidosCompras? PedidoCompras()
        {
            return new PedidosCompras
            {
                Numero = "PC001",
                IdProveedor = 1,
                IdEmpleado = 1,
                Fecha = DateTime.Now,
                Estado = "ABIERTA"
            };
        }

        public static DetallesPedidosCompras? DetallePedidoCompras()
        {
            return new DetallesPedidosCompras
            {
                IdPedidoCompra = 1,
                IdProducto = 1,
                Cantidad = 100,
                PrecioUnitario = 2000
            };
        }

        public static PedidoVentas? PedidoVentas()
        {
            return new PedidoVentas
            {
                Numero = "PV001",
                IdCliente = 1,
                IdEmpleado = 1,
                Fecha = DateTime.Now,
                Estado = "ABIERTA"
            };
        }

        public static DetallesPedidosVentas? DetallePedidoVentas()
        {
            return new DetallesPedidosVentas
            {
                IdPedidoVenta = 1,
                IdProducto = 1,
                Cantidad = 10,
                PrecioUnitario = 2500,
                IdImpuesto = 1
            };
        }

        public static Pagos? Pagos()
        {
            return new Pagos
            {
                IdPedidoVenta = 1,
                Fecha = DateTime.Now,
                Monto = 25000,
                Medio = "EFECTIVO",
                Referencia = "Caja001"
            };
        }
    }
}
