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
        public string SKU { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public int IdCategoria { get; set; }
        public int? IdImpuesto { get; set; }
        public int VolumenML { get; set; }
        public decimal GradAlcoholico { get; set; }
        public decimal PrecioLista { get; set; }
        public bool Activo { get; set; } = true;

        public CategoriasProductos CategoriasProductos { get; set; } = null!;
        public Impuestos? Impuestos { get; set; }
    }
}