namespace lib_dominio.Entidades
{
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
}