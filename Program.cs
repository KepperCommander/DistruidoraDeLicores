using ConsoleApp.Conexion;

class Program
{
    static void Main()
    {
        var cadenaConexion = "Server=localhost;Database=DISTRIBUIDORA_2;Trusted_Connection=True;";
        using (var db = new Conexion { Cadena_conexion = cadenaConexion })
        {
            bool puedeConectar = db.Database.CanConnect();
            Console.WriteLine(puedeConectar ? "Conexión exitosa" : "Error de conexión");
        }

        
        var conexionET = new ConexionET();

        Console.WriteLine("== Empleados ==");
        conexionET.CargarEmpleados();

        Console.WriteLine("\n== Productos ==");
        conexionET.CargarProductos();
    }
}
  
