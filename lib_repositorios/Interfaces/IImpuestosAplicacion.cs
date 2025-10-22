using System.Collections.Generic;
using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IImpuestosAplicacion
    {
        void Configurar(string stringConexion);

        Impuestos? Guardar(Impuestos? entidad);
        Impuestos? Modificar(Impuestos? entidad);
        Impuestos? Borrar(Impuestos? entidad);

        List<Impuestos> Listar(int take = 100);
        List<Impuestos> PorTexto(string texto, int take = 100);
        Impuestos? PorId(int idImpuesto);
    }
}
