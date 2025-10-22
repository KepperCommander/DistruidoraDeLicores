using System;
using System.Collections.Generic;
using System.Linq;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class ImpuestosAplicacion : IImpuestosAplicacion
    {
        private IConexion? IConexion = null;

        public ImpuestosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string stringConexion)
        {
            this.IConexion!.StringConexion = stringConexion;
        }

        // 🔹 Validar campos requeridos
        private static void ValidarRequeridos(Impuestos i)
        {
            if (string.IsNullOrWhiteSpace(i.Nombre))
                throw new Exception("lbNombreRequerido");
            if (i.Porcentaje <= 0)
                throw new Exception("lbPorcentajeRequerido");
        }

        // 🔹 Validar nombre único
        private void ValidarNombreUnico(Impuestos entidad, bool esModificar)
        {
            var nombre = entidad.Nombre.Trim().ToLower();

            var query = this.IConexion!.Impuestos!
                        .AsNoTracking()
                        .Where(x => x.Nombre.ToLower() == nombre);

            if (esModificar)
                query = query.Where(x => x.IdImpuesto != entidad.IdImpuesto);

            if (query.Any())
                throw new Exception("lbNombreDuplicado");
        }

        // 🔹 Guardar nuevo impuesto
        public Impuestos? Guardar(Impuestos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdImpuesto != 0)
                throw new Exception("lbYaSeGuardo");

            ValidarRequeridos(entidad);
            ValidarNombreUnico(entidad, esModificar: false);

            this.IConexion!.Impuestos!.Add(entidad);
            this.IConexion.SaveChanges();

            return entidad;
        }

        // 🔹 Modificar impuesto existente
        public Impuestos? Modificar(Impuestos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdImpuesto == 0)
                throw new Exception("lbNoSeGuardo");

            ValidarRequeridos(entidad);
            ValidarNombreUnico(entidad, esModificar: true);

            var entry = this.IConexion!.Entry(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();

            return entidad;
        }

        // 🔹 Borrar impuesto
        public Impuestos? Borrar(Impuestos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdImpuesto == 0)
                throw new Exception("lbNoSeGuardo");

            // Validar si el impuesto está asociado a productos
            var tieneProductos = this.IConexion!.Productos!
                .AsNoTracking()
                .Any(p => p.IdImpuesto == entidad.IdImpuesto);

            if (tieneProductos)
                throw new Exception("lbImpuestoConProductos");

            this.IConexion!.Impuestos!.Remove(entidad);
            this.IConexion.SaveChanges();

            return entidad;
        }

        // 🔹 Listar impuestos
        public List<Impuestos> Listar(int take = 100)
        {
            return this.IConexion!.Impuestos!
                .AsNoTracking()
                .OrderBy(x => x.Nombre)
                .Take(take)
                .ToList();
        }

        // 🔹 Buscar por texto
        public List<Impuestos> PorTexto(string texto, int take = 100)
        {
            texto = (texto ?? "").Trim();
            if (texto.Length == 0) return Listar(take);

            return this.IConexion!.Impuestos!
                .AsNoTracking()
                .Where(x =>
                    x.Nombre.Contains(texto))
                .OrderBy(x => x.Nombre)
                .Take(take)
                .ToList();
        }

        // 🔹 Buscar por Id
        public Impuestos? PorId(int idImpuesto)
        {
            return this.IConexion!.Impuestos!
                .AsNoTracking()
                .FirstOrDefault(x => x.IdImpuesto == idImpuesto);
        }
    }
}
