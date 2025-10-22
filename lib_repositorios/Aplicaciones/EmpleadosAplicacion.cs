using System;
using System.Collections.Generic;
using System.Linq;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class EmpleadosAplicacion : IEmpleadosAplicacion
    {
        private readonly IConexion _conexion;

        public EmpleadosAplicacion(IConexion conexion)
        {
            _conexion = conexion;
        }

        public void Configurar(string stringConexion)
        {
            _conexion.StringConexion = stringConexion;
        }

        // 🔹 Validar campos requeridos
        private static void ValidarRequeridos(Empleados e)
        {
            if (string.IsNullOrWhiteSpace(e.Nombres))
                throw new Exception("lbNombreRequerido");

            if (string.IsNullOrWhiteSpace(e.Apellidos))
                throw new Exception("lbApellidosRequerido");
        }

        // 🔹 Validar email único
        private void ValidarEmailUnico(Empleados entidad, bool esModificar)
        {
            if (string.IsNullOrWhiteSpace(entidad.Email))
                return;

            var email = entidad.Email.Trim().ToLower();

            var query = _conexion.Empleados!
                .AsNoTracking()
                .Where(x => x.Email != null && x.Email.ToLower() == email);

            if (esModificar)
                query = query.Where(x => x.IdEmpleado != entidad.IdEmpleado);

            if (query.Any())
                throw new Exception("lbEmailDuplicado");
        }

        // 🔹 Guardar nuevo empleado
        public Empleados Guardar(Empleados entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdEmpleado != 0)
                throw new Exception("lbYaSeGuardo");

            ValidarRequeridos(entidad);
            ValidarEmailUnico(entidad, false);

            _conexion.Empleados!.Add(entidad);
            _conexion.SaveChanges();

            return entidad;
        }

        // 🔹 Modificar empleado existente
        public Empleados Modificar(Empleados entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdEmpleado == 0)
                throw new Exception("lbNoSeGuardo");

            ValidarRequeridos(entidad);
            ValidarEmailUnico(entidad, true);

            var entry = _conexion.Entry(entidad);
            entry.State = EntityState.Modified;
            _conexion.SaveChanges();

            return entidad;
        }

        // 🔹 Borrar empleado
        public Empleados Borrar(Empleados entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdEmpleado == 0)
                throw new Exception("lbNoSeGuardo");

            var tienePedidos = _conexion.PedidosVentas!
                .AsNoTracking()
                .Any(p => p.IdEmpleado == entidad.IdEmpleado);

            if (tienePedidos)
                throw new Exception("lbEmpleadoConPedidos");

            _conexion.Empleados!.Remove(entidad);
            _conexion.SaveChanges();

            return entidad;
        }

        // 🔹 Listar empleados
        public List<Empleados> Listar(int take = 50)
        {
            return _conexion.Empleados!
                .AsNoTracking()
                .OrderBy(e => e.Nombres)
                .Take(take)
                .ToList();
        }

        // 🔹 Buscar por texto
        public List<Empleados> PorTexto(string texto, int take = 50)
        {
            texto = (texto ?? "").Trim();

            if (texto.Length == 0)
                return Listar(take);

            return _conexion.Empleados!
                .AsNoTracking()
                .Where(e =>
                    e.Nombres.Contains(texto) ||
                    e.Apellidos.Contains(texto) ||
                    (e.Email ?? "").Contains(texto) ||
                    (e.Telefono ?? "").Contains(texto))
                .OrderBy(e => e.Nombres)
                .Take(take)
                .ToList();
        }

        // 🔹 Buscar empleados por rol
        public List<Empleados> PorRol(int idRol, int take = 50)
        {
            return _conexion.Empleados!
                .AsNoTracking()
                .Where(e => e.IdRol == idRol)
                .OrderBy(e => e.Nombres)
                .Take(take)
                .ToList();
        }

        // 🔹 Buscar empleado por ID
        public Empleados? PorId(int idEmpleado)
        {
            return _conexion.Empleados!
                .AsNoTracking()
                .FirstOrDefault(e => e.IdEmpleado == idEmpleado);
        }
    }
}
