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
            entidad.IdEmpleado = 1;
            entidad.Nombres = "Juan";
            entidad.Apellidos = "Pérez";
            entidad.Email = "juan.perez@correo.com";
            entidad.Telefono = "123456789";
            entidad.IdRol = 2;
            entidad.FechaIngreso = DateTime.Now;
            entidad.Activo = true;

            return entidad;
        }
    }
}