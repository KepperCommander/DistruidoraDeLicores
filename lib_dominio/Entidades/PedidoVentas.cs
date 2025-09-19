using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace lib_dominio.Entidades
    {
    public class PedidosVentas
    {
        public int IdPedidoVenta { get; set; }
        public string Numero { get; set; } = string.Empty;
        public int IdCliente { get; set; }
        public int? IdEmpleado { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "ABIERTA";

       
        public Clientes Cliente { get; set; } = null!;
        public Empleados? Empleado { get; set; }
    }
}


