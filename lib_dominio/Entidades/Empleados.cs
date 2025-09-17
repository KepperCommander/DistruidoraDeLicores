using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
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
}