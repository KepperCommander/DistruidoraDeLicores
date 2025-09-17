namespace lib_dominio.Entidades
{
    public class Almacenes
    {
        public int IdAlmacen { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Direccion { get; set; }
        public bool EsPrincipal { get; set; } = false;
    }
}