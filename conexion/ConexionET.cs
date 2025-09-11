using ConsoleApp.Models;

namespace ConsoleApp.Conexion
{
    public class ConexionET
    {
        private readonly string cadena_conexion = "Server=localhost;Database=db_distribuidora_licores;Integrated Security=True;TrustServerCertificate=True;";

        public void CargarProductos()
        {
            using var conexion = new Conexion { Cadena_conexion = cadena_conexion };

            var productos = conexion.Productos!.ToList();

            foreach (var p in productos)
            {
                Console.WriteLine($"{p.IdProducto} - {p.Nombre} - {p.SKU} - {p.PrecioLista:C}");
            }
        }

        public void CargarEmpleados()
        {
            using var conexion = new Conexion { Cadena_conexion = cadena_conexion };

            var empleados = conexion.Empleados!.ToList();

            foreach (var e in empleados)
            {
                Console.WriteLine($"{e.IdEmpleado} - {e.Nombres} {e.Apellidos} - {e.Email}");
            }
        }

        
    }
}
