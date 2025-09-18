using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Entidades
{
    public class Roles
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; } = null!;
        public bool EsActivo { get; set; } = true;

        public ICollection<Empleados> Empleados { get; set; } = new List<Empleados>();
    }
}