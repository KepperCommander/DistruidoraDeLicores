using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Entidades
{
    public class Productos
    {
        public int IdProducto { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public int IdCategoria { get; set; }
        public int? IdImpuesto { get; set; }
        public int VolumenML { get; set; }
        public decimal GradAlcoholico { get; set; }
        public decimal PrecioLista { get; set; }
        public bool Activo { get; set; } = true;

        public CategoriasProductos Categoria { get; set; } = null!;
        public Impuestos? Impuesto { get; set; }
    }
}