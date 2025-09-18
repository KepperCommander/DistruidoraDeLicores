using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Entidades
{
    public class PedidoVentas
    {
        public int IdPedidoVenta { get; set; }
        public string Numero { get; set; } = null!;
        public int IdCliente { get; set; }
        public int? IdEmpleado { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "ABIERTA";

        public Clientes Clientes { get; set; } = null!;
        public Empleados? Empleados { get; set; }
    }
}
