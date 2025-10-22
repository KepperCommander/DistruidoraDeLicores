using System;
using System.Collections.Generic;
using System.Linq;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class InventariosProductosAplicacion : IInventariosProductosAplicacion
    {
        private IConexion? IConexion = null;

        public InventariosProductosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string stringConexion)
        {
            this.IConexion!.StringConexion = stringConexion;
        }

        // 🔹 Validar campos requeridos
        private static void ValidarRequeridos(InventariosProductos i)
        {
            if (i.IdProducto <= 0)
                throw new Exception("lbProductoRequerido");
            if (i.IdAlmacen <= 0)
                throw new Exception("lbAlmacenRequerido");
            if (i.CantidadUnidades < 0)
                throw new Exception("lbCantidadInvalida");
        }

        // 🔹 Validar que no haya duplicados del mismo producto en el mismo almacén
        private void ValidarDuplicado(InventariosProductos entidad, bool esModificar)
        {
            var query = this.IConexion!.InventariosProductos!
                        .AsNoTracking()
                        .Where(x => x.IdProducto == entidad.IdProducto && x.IdAlmacen == entidad.IdAlmacen);

            if (esModificar)
                query = query.Where(x => x.IdInventarioProd != entidad.IdInventarioProd);

            if (query.Any())
                throw new Exception("lbInventarioDuplicado");
        }

        // 🔹 Guardar nuevo registro de inventario
        public InventariosProductos? Guardar(InventariosProductos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdInventarioProd != 0)
                throw new Exception("lbYaSeGuardo");

            ValidarRequeridos(entidad);
            ValidarDuplicado(entidad, esModificar: false);

            this.IConexion!.InventariosProductos!.Add(entidad);
            this.IConexion.SaveChanges();

            return entidad;
        }

        // 🔹 Modificar inventario existente
        public InventariosProductos? Modificar(InventariosProductos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdInventarioProd == 0)
                throw new Exception("lbNoSeGuardo");

            ValidarRequeridos(entidad);
            ValidarDuplicado(entidad, esModificar: true);

            var entry = this.IConexion!.Entry(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();

            return entidad;
        }

        // 🔹 Borrar registro de inventario
        public InventariosProductos? Borrar(InventariosProductos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.IdInventarioProd == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.InventariosProductos!.Remove(entidad);
            this.IConexion.SaveChanges();

            return entidad;
        }

        // 🔹 Listar inventarios
        public List<InventariosProductos> Listar(int take = 100)
        {
            return this.IConexion!.InventariosProductos!
                .AsNoTracking()
                .Include(x => x.Producto)
                .Include(x => x.Almacenes)
                .OrderBy(x => x.IdInventarioProd)
                .Take(take)
                .ToList();
        }

        // 🔹 Buscar por producto o almacén
        public List<InventariosProductos> PorTexto(string texto, int take = 100)
        {
            texto = (texto ?? "").Trim();
            if (texto.Length == 0) return Listar(take);

            return this.IConexion!.InventariosProductos!
                .AsNoTracking()
                .Include(x => x.Producto)
                .Include(x => x.Almacenes)
                .Where(x =>
                    x.Producto.Nombre.Contains(texto) ||
                    x.Almacenes.Nombre.Contains(texto))
                .OrderBy(x => x.IdInventarioProd)
                .Take(take)
                .ToList();
        }

        // 🔹 Buscar por Id
        public InventariosProductos? PorId(int idInventario)
        {
            return this.IConexion!.InventariosProductos!
                .AsNoTracking()
                .Include(x => x.Producto)
                .Include(x => x.Almacenes)
                .FirstOrDefault(x => x.IdInventarioProd == idInventario);
        }

        // 🔹 Buscar por almacén
        public List<InventariosProductos> PorAlmacen(int idAlmacen, int take = 100)
        {
            return this.IConexion!.InventariosProductos!
                .AsNoTracking()
                .Include(x => x.Producto)
                .Where(x => x.IdAlmacen == idAlmacen)
                .Take(take)
                .ToList();
        }

        // 🔹 Buscar por producto
        public InventariosProductos? PorProducto(int idProducto)
        {
            return this.IConexion!.InventariosProductos!
                .AsNoTracking()
                .FirstOrDefault(x => x.IdProducto == idProducto);
        }
    }
}
