using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class Almacenes : IAlmacenes
    {
        private IConexion? IConexion = null;

        public Almacenes(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Dueños? Borrar(Dueños? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            // Operaciones

            this.IConexion!.Dueños!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Dueños? Guardar(Dueños? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.Id != 0)
                throw new Exception("lbYaSeGuardo");

            // Operaciones

            this.IConexion!.Dueños!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Dueños> Listar()
        {
            return this.IConexion!.Dueños!.Take(20).ToList();
        }

        public Dueños? Modificar(Dueños? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.Id == 0)
                throw new Exception("lbNoSeGuardo");

            // Operaciones

            var entry = this.IConexion!.Entry<Dueños>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}
