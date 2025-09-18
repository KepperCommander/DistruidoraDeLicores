using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class EmpleadosAplicacion : IEmpleadosAplicacion
    {
        private IConexion? IConexion = null;

        public EmpleadosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string stringConexion)
        {
            this.IConexion!.StringConexion = stringConexion;
        }

        private static void ValidarRequeridos(Empleados e)
        {
            if (string.IsNullOrWhiteSpace(e.Nombres))
                throw new Exception("lbNombresRequeridos");
            if (string.IsNullOrWhiteSpace(e.Apellidos))
                throw new Exception("lbApellidosRequeridos");
            if (e.IdRol <= 0)
                throw new Exception("lbRolInvalido");
        }

        private void ValidarEmailUnico(Empleados entidad, bool esModificar)
        {
            
            if (string.IsNullOrWhiteSpace(entidad.Email))
                return;

            var email = entidad.Email.Trim();
            var query = this.IConexion!.Empleados!.AsNoTracking()
                         .Where(x => x.Email != null && x.Email.ToLower() == email.ToLower());

            if (esModificar)
                query = query.Where(x => x.IdEmpleado != entidad.IdEmpleado);

            if (query.Any())
                throw new Exception("lbEmailDuplicado");
        }

        
        public Empleados? Guardar(Empleados? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdEmpleado != 0)
                throw new Exception("lbYaSeGuardo");

            ValidarRequeridos(entidad);
            ValidarEmailUnico(entidad, esModificar: false);

            
            if (entidad.FechaIngreso == default) entidad.FechaIngreso = DateTime.Today;
            

            this.IConexion!.Empleados!.Add(entidad);
            this.IConexion!.SaveChanges();
            return entidad;
        }

        public Empleados? Modificar(Empleados? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdEmpleado == 0)
                throw new Exception("lbNoSeGuardo");

            ValidarRequeridos(entidad);
            ValidarEmailUnico(entidad, esModificar: true);

            var entry = this.IConexion!.Entry(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Empleados? Borrar(Empleados? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdEmpleado == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Empleados!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        
        public List<Empleados> Listar(int take = 50)
        {
            return this.IConexion!.Empleados!
                .AsNoTracking()
                .OrderBy(x => x.Apellidos).ThenBy(x => x.Nombres)
                .Take(take)
                .ToList();
        }

        public List<Empleados> PorTexto(string texto, int take = 50)
        {
            texto = (texto ?? "").Trim();
            if (texto.Length == 0) return Listar(take);

            return this.IConexion!.Empleados!
                .AsNoTracking()
                .Where(x =>
                    (x.Nombres + " " + x.Apellidos).Contains(texto) ||
                    (x.Email ?? "").Contains(texto) ||
                    (x.Telefono ?? "").Contains(texto))
                .OrderBy(x => x.Apellidos).ThenBy(x => x.Nombres)
                .Take(take)
                .ToList();
        }

        public List<Empleados> PorRol(int idRol, int take = 50)
        {
            if (idRol <= 0) return new List<Empleados>();

            return this.IConexion!.Empleados!
                .AsNoTracking()
                .Where(x => x.IdRol == idRol)
                .OrderBy(x => x.Apellidos).ThenBy(x => x.Nombres)
                .Take(take)
                .ToList();
        }
    }
}
