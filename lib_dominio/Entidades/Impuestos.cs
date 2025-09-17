namespace lib_dominio.Entidades
{
    public class Impuestos
    {
        public int IdImpuesto { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Porcentaje { get; set; }

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}