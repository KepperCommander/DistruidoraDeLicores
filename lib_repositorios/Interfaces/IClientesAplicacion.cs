using System.Collections.Generic;
using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IClientesAplicacion
    {
        void Configurar(string stringConexion);

        Clientes? Guardar(Clientes? entidad);
        Clientes? Modificar(Clientes? entidad);
        Clientes? Borrar(Clientes? entidad);

        List<Clientes> Listar(int take = 50);
        List<Clientes> PorTexto(string texto, int take = 50);
        Clientes? PorId(int id);
    }
}
