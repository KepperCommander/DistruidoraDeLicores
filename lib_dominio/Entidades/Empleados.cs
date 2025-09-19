using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace lib_dominio.Entidades
{
    public class Empleados
    {
        public int IdEmpleado { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefono { get; set; }

        public int IdRol { get; set; }       

        public DateTime FechaIngreso { get; set; } = DateTime.Today;
        public bool Activo { get; set; } = true;

        public ICollection<PedidosVentas> PedidosVentas { get; set; } = new List<PedidosVentas>();
    }

}

