using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class ProductosPrueba
    {
        private readonly IConexion? iConexion;
        private List<Productos>? lista;
        private Productos? entidad;

        public ProductosPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, Guardar(), "Guardar() falló");
            Assert.AreEqual(true, Modificar(), "Modificar() falló");
            Assert.AreEqual(true, Listar(), "Listar() falló");
            Assert.AreEqual(true, Borrar(), "Borrar() falló");
        }

        public bool Guardar()
        {
          
            
            entidad = new Productos
            {
                Codigo = $"SKU-{Guid.NewGuid():N}".Substring(0, 20), 
                Nombre = "Producto Prueba",
                IdCategoria = 1,             
                IdImpuesto = null,
                VolumenML = 750,
                GradAlcoholico = 12.00m,
                PrecioLista = 10000m,
                Activo = true
            };

            iConexion!.Productos!.Add(entidad);
            iConexion.SaveChanges();

            return entidad.IdProducto > 0;
        }

        public bool Modificar()
        {
            
            Assert.IsNotNull(entidad, "No hay entidad cargada para modificar.");

            
            entidad!.Nombre = entidad.Nombre + " - Editado";
            entidad.PrecioLista += 500m;
            entidad.Activo = !entidad.Activo;

            
            iConexion!.SaveChanges();
            return true;
        }

        public bool Listar()
        {
            lista = iConexion!.Productos!.AsNoTracking().Take(50).ToList();
            return lista.Count > 0;
        }

        public bool Borrar()
        {
            if (entidad is null) return true;
            iConexion!.Productos!.Remove(entidad);
            iConexion.SaveChanges();
            return true;
        }
    }
}
