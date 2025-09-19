using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Entidades
{
    public class PedidosCompras
    {
        public int IdPedidoCompra { get; set; }
        public string Numero { get; set; } = null!;
        public int IdProveedor { get; set; }
        public int? IdEmpleado { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "ABIERTA";

        public Proveedores? Proveedor { get; set; }
        public Empleados? Empleado { get; set; }
    }
}