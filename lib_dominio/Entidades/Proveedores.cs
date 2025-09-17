namespace lib_dominio.Entidades
{
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
}