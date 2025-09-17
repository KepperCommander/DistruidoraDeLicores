using lib_dominio.Entidades;
using lib_repositorio.Interfaces;
using lib_repositorios.Implementaciones;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class InventariosProductosPrueba
    {
        private readonly IConexion? iConexion;
        private List<InventariosProductos>? lista;
        private InventariosProductos? entidad;

        public InventariosProductosPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, Guardar());
            Assert.AreEqual(true, Modificar());
            Assert.AreEqual(true, Listar());
            Assert.AreEqual(true, Borrar());
        }

        public bool Listar()
        {
            this.lista = this.iConexion!.InventariosProductos!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
            this.entidad = EntidadesNucleo.InventariosProductos()!;

            this.iConexion!.InventariosProductos!.Add(this.entidad);
            this.iConexion!.SaveChanges();

            return true;
        }

        public bool Modificar()
        {
            this.entidad!.Nombre = "Test";

            var entry = this.iConexion!.Entry<InventariosProductos>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();

            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.InventariosProductos!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}