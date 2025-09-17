using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IMascotas_DueñosAplicacion
    {
        void Configurar(string StringConexion);
        List<Mascotas_Dueños> Listar();
        Mascotas_Dueños? Guardar(Mascotas_Dueños? entidad);
        Mascotas_Dueños? Modificar(Mascotas_Dueños? entidad);
        Mascotas_Dueños? Borrar(Mascotas_Dueños? entidad);
    }
}