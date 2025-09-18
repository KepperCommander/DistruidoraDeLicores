using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Entidades
{
    public class InventariosProductos
    {
        public int IdInventarioProd { get; set; }
        public int IdProducto { get; set; }
        public int IdAlmacen { get; set; }
        public int CantidadUnidades { get; set; }

        public Productos Producto { get; set; } = null!;
        public Almacenes Almacenes { get; set; } = null!;
    }
}
