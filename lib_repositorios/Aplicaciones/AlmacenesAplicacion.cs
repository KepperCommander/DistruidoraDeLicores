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
    public class AlmacenesAplicacion : IAlmacenesAplicacion
    {
        private IConexion? IConexion = null;

        public AlmacenesAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string stringConexion)
        {
            this.IConexion!.StringConexion = stringConexion;
        }

        // 🔹 Validaciones de campos obligatorios
        private static void ValidarRequeridos(Almacenes a)
        {
            if (string.IsNullOrWhiteSpace(a.Nombre))
                throw new Exception("lbNombreRequerido");
        }

        // 🔹 Validar que el nombre del almacén sea único
        private void ValidarNombreUnico(Almacenes entidad, bool esModificar)
        {
            var nombre = entidad.Nombre.Trim().ToLower();

            var query = this.IConexion!.Almacenes!
                        .AsNoTracking()
                        .Where(x => x.Nombre.ToLower() == nombre);

            if (esModificar)
                query = query.Where(x => x.IdAlmacen != entidad.IdAlmacen);

            if (query.Any())
                throw new Exception("lbNombreDuplicado");
        }

        // 🔹 Crear nuevo almacén
        public Almacenes? Guardar(Almacenes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdAlmacen != 0)
                throw new Exception("lbYaSeGuardo");

            ValidarRequeridos(entidad);
            ValidarNombreUnico(entidad, esModificar: false);

            this.IConexion!.Almacenes!.Add(entidad);
            this.IConexion.SaveChanges();

            return entidad;
        }

        // 🔹 Modificar almacén existente
        public Almacenes? Modificar(Almacenes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdAlmacen == 0)
                throw new Exception("lbNoSeGuardo");

            ValidarRequeridos(entidad);
            ValidarNombreUnico(entidad, esModificar: true);

            var entry = this.IConexion!.Entry(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();

            return entidad;
        }

        // 🔹 Eliminar almacén
        public Almacenes? Borrar(Almacenes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdAlmacen == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Almacenes!.Remove(entidad);
            this.IConexion.SaveChanges();

            return entidad;
        }

        // 🔹 Listar almacenes (por defecto, máximo 50)
        public List<Almacenes> Listar(int take = 50)
        {
            return this.IConexion!.Almacenes!
                .AsNoTracking()
                .OrderBy(x => x.Nombre)
                .Take(take)
                .ToList();
        }

        // 🔹 Buscar almacenes por texto
        public List<Almacenes> PorTexto(string texto, int take = 50)
        {
            texto = (texto ?? "").Trim();
            if (texto.Length == 0) return Listar(take);

            return this.IConexion!.Almacenes!
                .AsNoTracking()
                .Where(x =>
                    x.Nombre.Contains(texto) ||
                    (x.Direccion ?? "").Contains(texto))
                .OrderBy(x => x.Nombre)
                .Take(take)
                .ToList();
        }

        // 🔹 Listar almacenes principales o secundarios
        public List<Almacenes> PorTipo(bool esPrincipal, int take = 50)
        {
            return this.IConexion!.Almacenes!
                .AsNoTracking()
                .Where(x => x.EsPrincipal == esPrincipal)
                .OrderBy(x => x.Nombre)
                .Take(take)
                .ToList();
        }
    }
}
