namespace lib_dominio.Entidades
{
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
}