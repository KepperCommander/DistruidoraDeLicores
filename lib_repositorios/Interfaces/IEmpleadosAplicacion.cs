using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IEmpleadosAplicacion
    {
        void Configurar(string stringConexion);

        
        Empleados? Guardar(Empleados? entidad);
        Empleados? Modificar(Empleados? entidad);
        Empleados? Borrar(Empleados? entidad);

        
        List<Empleados> Listar(int take = 50);
        List<Empleados> PorTexto(string texto, int take = 50);
        List<Empleados> PorRol(int idRol, int take = 50);
    }
}

