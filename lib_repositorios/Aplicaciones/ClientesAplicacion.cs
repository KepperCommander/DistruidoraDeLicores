using System;
using System.Collections.Generic;
using System.Linq;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class ClientesAplicacion : IClientesAplicacion
    {
        private IConexion? IConexion;

        public void Configurar(string stringConexion)
        {
            this.IConexion = new Conexion
            {
                StringConexion = stringConexion
            };
        }

        // === Validaciones ===
        private void ValidarRequeridos(Clientes entidad)
        {
            if (string.IsNullOrWhiteSpace(entidad.RazonSocial))
                throw new Exception("El nombre o razón social del cliente es obligatorio.");

            if (string.IsNullOrWhiteSpace(entidad.NIT))
                throw new Exception("El NIT del cliente es obligatorio.");
        }

        private void ValidarNITUnico(Clientes entidad, bool esModificar = false)
        {
            var query = this.IConexion!.Clientes!
                .AsNoTracking()
                .Where(x => x.NIT.ToLower() == entidad.NIT.ToLower());

            if (esModificar)
                query = query.Where(x => x.IdCliente != entidad.IdCliente);

            if (query.Any())
                throw new Exception("Ya existe un cliente con el mismo NIT.");
        }

        // === CRUD ===

        public Clientes? Guardar(Clientes? entidad)
        {
            if (entidad == null)
                throw new Exception("No se recibió información del cliente.");

            if (entidad.IdCliente != 0)
                throw new Exception("El cliente ya existe.");

            ValidarRequeridos(entidad);
            ValidarNITUnico(entidad);

            this.IConexion!.Clientes!.Add(entidad);
            this.IConexion.SaveChanges();

            return entidad;
        }

        public Clientes? Modificar(Clientes? entidad)
        {
            if (entidad == null)
                throw new Exception("No se recibió información del cliente.");

            if (entidad.IdCliente == 0)
                throw new Exception("No se puede modificar un cliente sin ID.");

            ValidarRequeridos(entidad);
            ValidarNITUnico(entidad, esModificar: true);

            var original = this.IConexion!.Clientes!.Find(entidad.IdCliente);
            if (original == null)
                throw new Exception("El cliente no existe.");

            original.RazonSocial = entidad.RazonSocial;
            original.NIT = entidad.NIT;
            original.Email = entidad.Email;
            original.Telefono = entidad.Telefono;
            original.Direccion = entidad.Direccion;
            original.Activo = entidad.Activo;

            this.IConexion.SaveChanges();
            return original;
        }

        public Clientes? Borrar(Clientes? entidad)
        {
            if (entidad == null)
                throw new Exception("No se recibió información del cliente.");

            var encontrado = this.IConexion!.Clientes!.Find(entidad.IdCliente);
            if (encontrado == null)
                throw new Exception("El cliente no existe.");

            this.IConexion.Clientes.Remove(encontrado);
            this.IConexion.SaveChanges();

            return encontrado;
        }

        // === Consultas ===

        public List<Clientes> Listar(int take = 50)
        {
            return this.IConexion!.Clientes!
                .AsNoTracking()
                .OrderBy(x => x.RazonSocial)
                .Take(take)
                .ToList();
        }

        public List<Clientes> PorTexto(string texto, int take = 50)
        {
            texto = texto?.ToLower() ?? "";
            return this.IConexion!.Clientes!
                .AsNoTracking()
                .Where(x => x.RazonSocial.ToLower().Contains(texto)
                         || x.NIT.ToLower().Contains(texto)
                         || (x.Email ?? "").ToLower().Contains(texto))
                .OrderBy(x => x.RazonSocial)
                .Take(take)
                .ToList();
        }

        public Clientes? PorId(int id)
        {
            return this.IConexion!.Clientes!
                .AsNoTracking()
                .FirstOrDefault(x => x.IdCliente == id);
        }
    }
}
