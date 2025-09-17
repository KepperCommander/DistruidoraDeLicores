using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;

namespace ut_presentacion.Nucleo
{
    public class EntidadesNucleo
    {
        public static Empleados? Empleados()
        {
            var entidad = new Empleados();
            entidad.Nombres = "Juan";
            entidad.Apellidos = "Pruebas-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            entidad.Email = $"empleado_{Guid.NewGuid():N}@empresa.com"; // <- único SIEMPRE
            entidad.Telefono = "3000000000";
            entidad.IdRol = 1;                       // existe en tu script (Roles 1..5)
            entidad.FechaIngreso = DateTime.Today;
            entidad.Activo = true;
            return entidad;
        }
    }
}
