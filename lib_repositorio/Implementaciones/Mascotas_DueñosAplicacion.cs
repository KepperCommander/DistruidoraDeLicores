using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class Mascotas_DueñosAplicacion : IMascotas_DueñosAplicacion
    {
        private IConexion? IConexion = null;

        public Mascotas_DueñosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Mascotas_Dueños? Borrar(Mascotas_Dueños? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            // Operaciones
            entidad._Dueño = null;
            entidad._Mascota = null;

            this.IConexion!.Mascotas_Dueños!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Mascotas_Dueños? Guardar(Mascotas_Dueños? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");

            // Operaciones
            entidad._Dueño = null;
            entidad._Mascota = null;

            this.IConexion!.Mascotas_Dueños!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Mascotas_Dueños> Listar()
        {
            return this.IConexion!.Mascotas_Dueños!.Take(20).ToList();
        }

        public Mascotas_Dueños? Modificar(Mascotas_Dueños? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            // Operaciones
            entidad._Dueño = null;
            entidad._Mascota = null;

            var entry = this.IConexion!.Entry<Mascotas_Dueños>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}
