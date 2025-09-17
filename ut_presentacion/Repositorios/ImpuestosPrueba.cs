using lib_dominio.Entidades;
using lib_repositorio.Interfaces;
using lib_repositorios.Implementaciones;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class ImpuestosPrueba
    {
        private readonly IConexion? iConexion;
        private List<Impuestos>? lista;
        private Impuestos? entidad;

        public ImpuestosPrueba()
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
            this.lista = this.iConexion!.Impuestos!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
            this.entidad = EntidadesNucleo.Impuestos()!;

            this.iConexion!.Impuestos!.Add(this.entidad);
            this.iConexion!.SaveChanges();

            return true;
        }

        public bool Modificar()
        {
            this.entidad!.Nombre = "Test";

            var entry = this.iConexion!.Entry<Impuestos>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();

            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.Impuestos!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}