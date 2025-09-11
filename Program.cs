using ConsoleApp.Conexion;

class Program
{
    static void Main()
    {
        // Prueba de conexión a la base de datos
        var cadenaConexion = "TU_CADENA_DE_CONEXION"; // Reemplaza por tu cadena real
        using (var db = new Conexion { Cadena_conexion = cadenaConexion })
        {
            bool puedeConectar = db.Database.CanConnect();
            Console.WriteLine(puedeConectar ? "Conexión exitosa" : "Error de conexión");
        }

        // Tu código original
        var conexionET = new ConexionET();

        Console.WriteLine("== Empleados ==");
        conexionET.CargarEmpleados();

        Console.WriteLine("\n== Productos ==");
        conexionET.CargarProductos();
    }
}
  