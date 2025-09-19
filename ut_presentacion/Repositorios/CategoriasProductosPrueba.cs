                                                                                              using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class CategoriasProductosPrueba
    {
        private Conexion _conexion;

        [TestInitialize]
        public void Inicializar()
        {
            _conexion = new Conexion
            {
                StringConexion = "Server=localhost;Database=TuBD;Trusted_Connection=True;TrustServerCertificate=True;"
            };
        }

        [TestMethod]
        public void InsertarCategoriaProducto()
        {
            var categoria = new CategoriasProductos
            {
                Nombre = "Bebidas",
                Descripcion = "Productos líquidos alcohólicos y no alcohólicos"
            };

            _conexion.CategoriasProductos!.Add(categoria);
            _conexion.SaveChanges();

            Assert.IsTrue(categoria.IdCategoria > 0, "La categoría no se insertó correctamente.");
        }

        
        public void ConsultarCategoriaProducto()
        {
            var categoria = _conexion.CategoriasProductos!.FirstOrDefault(c => c.Nombre == "Bebidas");
            Assert.IsNotNull(categoria, "No se encontró la categoría 'Bebidas'.");
        }

       
        public void ActualizarCategoriaProducto()
        {
            var categoria = _conexion.CategoriasProductos!.FirstOrDefault(c => c.Nombre == "Bebidas");
            Assert.IsNotNull(categoria, "No existe la categoría para actualizar.");

            categoria.Descripcion = "Bebidas alcohólicas y sin alcohol (actualizado)";
            _conexion.SaveChanges();

            var categoriaActualizada = _conexion.CategoriasProductos!.FirstOrDefault(c => c.IdCategoria == categoria.IdCategoria);
            Assert.AreEqual("Bebidas alcohólicas y sin alcohol (actualizado)", categoriaActualizada!.Descripcion);
        }

       
        public void BorrarCategoriaProducto()
        {
            var categoria = _conexion.CategoriasProductos!.FirstOrDefault(c => c.Nombre == "Bebidas");
            Assert.IsNotNull(categoria, "No existe la categoría para eliminar.");

            _conexion.CategoriasProductos!.Remove(categoria);
            _conexion.SaveChanges();

            var categoriaEliminada = _conexion.CategoriasProductos!.FirstOrDefault(c => c.IdCategoria == categoria.IdCategoria);
            Assert.IsNull(categoriaEliminada, "La categoría no se eliminó correctamente.");
        }
    }
}