namespace lib_dominio.Entidades
{
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

}