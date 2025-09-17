using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IDueñosAplicacion
    {
        void Configurar(string StringConexion);
        List<Dueños> Listar();
        Dueños? Guardar(Dueños? entidad);
        Dueños? Modificar(Dueños? entidad);
        Dueños? Borrar(Dueños? entidad);
    }
}