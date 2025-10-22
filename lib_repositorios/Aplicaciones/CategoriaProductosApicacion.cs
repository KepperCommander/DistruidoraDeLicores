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
    public class CategoriasProductosAplicacion : ICategoriasProductosAplicacion
    {
        private IConexion? IConexion = null;

        public CategoriasProductosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string stringConexion)
        {
            this.IConexion!.StringConexion = stringConexion;
        }

        // 🔹 Validar campos requeridos
        private static void ValidarRequeridos(CategoriasProductos c)
        {
            if (string.IsNullOrWhiteSpace(c.Nombre))
                throw new Exception("lbNombreRequerido");
        }

        // 🔹 Validar nombre único
        private void ValidarNombreUnico(CategoriasProductos entidad, bool esModificar)
        {
            var nombre = entidad.Nombre.Trim().ToLower();

            var query = this.IConexion!.CategoriasProductos!
                        .AsNoTracking()
                        .Where(x => x.Nombre.ToLower() == nombre);

            if (esModificar)
                query = query.Where(x => x.IdCategoria != entidad.IdCategoria);

            if (query.Any())
                throw new Exception("lbNombreDuplicado");
        }

        // 🔹 Guardar nueva categoría
        public CategoriasProductos? Guardar(CategoriasProductos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdCategoria != 0)
                throw new Exception("lbYaSeGuardo");

            ValidarRequeridos(entidad);
            ValidarNombreUnico(entidad, esModificar: false);

            this.IConexion!.CategoriasProductos!.Add(entidad);
            this.IConexion.SaveChanges();

            return entidad;
        }

        // 🔹 Modificar categoría existente
        public CategoriasProductos? Modificar(CategoriasProductos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdCategoria == 0)
                throw new Exception("lbNoSeGuardo");

            ValidarRequeridos(entidad);
            ValidarNombreUnico(entidad, esModificar: true);

            var entry = this.IConexion!.Entry(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();

            return entidad;
        }

        // 🔹 Borrar categoría
        public CategoriasProductos? Borrar(CategoriasProductos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdCategoria == 0)
                throw new Exception("lbNoSeGuardo");

            // Se podría validar que no tenga productos asociados antes de eliminar
            var tieneProductos = this.IConexion!.Productos!
                                    .AsNoTracking()
                                    .Any(p => p.IdCategoria == entidad.IdCategoria);

            if (tieneProductos)
                throw new Exception("lbCategoriaConProductos");

            this.IConexion!.CategoriasProductos!.Remove(entidad);
            this.IConexion.SaveChanges();

            return entidad;
        }

        // 🔹 Listar categorías
        public List<CategoriasProductos> Listar(int take = 50)
        {
            return this.IConexion!.CategoriasProductos!
                .AsNoTracking()
                .OrderBy(x => x.Nombre)
                .Take(take)
                .ToList();
        }

        // 🔹 Buscar por texto
        public List<CategoriasProductos> PorTexto(string texto, int take = 50)
        {
            texto = (texto ?? "").Trim();
            if (texto.Length == 0) return Listar(take);

            return this.IConexion!.CategoriasProductos!
                .AsNoTracking()
                .Where(x =>
                    x.Nombre.Contains(texto) ||
                    (x.Descripcion ?? "").Contains(texto))
                .OrderBy(x => x.Nombre)
                .Take(take)
                .ToList();
        }
    }
}
