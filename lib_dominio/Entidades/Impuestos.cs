using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Entidades
{
    public class Impuestos
    {
        public int IdImpuesto { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Porcentaje { get; set; }

        public ICollection<Productos> Productos { get; set; } = new List<Productos>();
    }
}