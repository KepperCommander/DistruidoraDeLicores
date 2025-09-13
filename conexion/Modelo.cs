public class Roles
{
    public int IdRol { get; set; }
    public string Nombre { get; set; } = null!;
    public bool EsActivo { get; set; } = true;

    public ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}

public class Empleados
{
    public int IdEmpleado { get; set; }
    public string Nombres { get; set; } = null!;
    public string Apellidos { get; set; } = null!;
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public int IdRol { get; set; }
    public DateTime FechaIngreso { get; set; } = DateTime.Today;
    public bool Activo { get; set; } = true;

    public Role Rol { get; set; } = null!;
}

public class Proveedores
{
    public int IdProveedor { get; set; }
    public string RazonSocial { get; set; } = null!;
    public string NIT { get; set; } = null!;
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public bool Activo { get; set; } = true;
}

// Models/Cliente.cs
public class Clientes
{
    public int IdCliente { get; set; }
    public string RazonSocial { get; set; } = null!;
    public string NIT { get; set; } = null!;
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public bool Activo { get; set; } = true;
}

// Models/CategoriaProducto.cs
public class CategoriasProductos
{
    public int IdCategoria { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }

    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}

// Models/Impuesto.cs
public class Impuestos
{
    public int IdImpuesto { get; set; }
    public string Nombre { get; set; } = null!;
    public decimal Porcentaje { get; set; }

    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}

// Models/Producto.cs
public class Productos
{
    public int IdProducto { get; set; }
    public string SKU { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public int IdCategoria { get; set; }
    public int? IdImpuesto { get; set; }
    public int VolumenML { get; set; }
    public decimal GradAlcoholico { get; set; }
    public decimal PrecioLista { get; set; }
    public bool Activo { get; set; } = true;

    public CategoriaProducto Categoria { get; set; } = null!;
    public Impuesto? Impuesto { get; set; }
}

// Models/Almacen.cs
public class Almacenes
{
    public int IdAlmacen { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Direccion { get; set; }
    public bool EsPrincipal { get; set; } = false;
}

public class InventariosProductos
{
    public int IdInventarioProd { get; set; }
    public int IdProducto { get; set; }
    public int IdAlmacen { get; set; }
    public int CantidadUnidades { get; set; }

    public Producto Producto { get; set; } = null!;
    public Almacen Almacen { get; set; } = null!;
}

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

public class PedidosCompras
{
    public int IdPedidoCompra { get; set; }
    public string Numero { get; set; } = null!;
    public int IdProveedor { get; set; }
    public int? IdEmpleado { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string Estado { get; set; } = "ABIERTA";

    public Proveedor Proveedor { get; set; } = null!;
    public Empleado? Empleado { get; set; }
}

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

public class PedidosVentas
{
    public int IdPedidoVenta { get; set; }
    public string Numero { get; set; } = null!;
    public int IdCliente { get; set; }
    public int? IdEmpleado { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string Estado { get; set; } = "ABIERTA";

    public Cliente Cliente { get; set; } = null!;
    public Empleado? Empleado { get; set; }
}

public class DetallesPedidosVentas
{
    public int IdDetallePV { get; set; }
    public int IdPedidoVenta { get; set; }
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }s
    public decimal PrecioUnitario { get; set; }
    public int? IdImpuesto { get; set; }

    public PedidoVenta PedidoVenta { get; set; } = null!;
    public Producto Producto { get; set; } = null!;
    public Impuesto? Impuesto { get; set; }
}

public class Pagos
{
    public int IdPago { get; set; }
    public int IdPedidoVenta { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public decimal Monto { get; set; }
    public string Medio { get; set; } = null!;
    public string? Referencia { get; set; }

    public PedidoVenta PedidoVenta { get; set; } = null!;
}


