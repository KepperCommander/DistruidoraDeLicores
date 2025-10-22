using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IAlmacenesAplicacion
    {
        void Configurar(string stringConexion);

        Almacenes? Guardar(Almacenes? entidad);
        Almacenes? Modificar(Almacenes? entidad);
        Almacenes? Borrar(Almacenes? entidad);

        List<Almacenes> Listar(int take = 50);
        List<Almacenes> PorTexto(string texto, int take = 50);
        List<Almacenes> PorTipo(bool esPrincipal, int take = 50);
    }
}
