namespace lib_dominio.Entidades
{
    public class Roles
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; } = null!;
        public bool EsActivo { get; set; } = true;

        public ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
    }
}