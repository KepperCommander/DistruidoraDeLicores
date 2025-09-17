using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class EmpleadosPrueba
    {
        private readonly IConexion? iConexion;
        private List<Empleados>? lista;
        private Empleados? entidad;

        public EmpleadosPrueba()
        {
            iConexion = new Conexion();
            iConexion.StringConexion =
                Configuracion.ObtenerValor("StringConexion"); // debe apuntar a DISTRIBUIDORA_2
        }

        [TestMethod]
        public void Ejecutar()
        {
            try
            {
                // sanity checks
                Assert.IsNotNull(iConexion, "iConexion es null.");
                var ctx = (Conexion)iConexion!;
                Assert.IsTrue(ctx.Database.CanConnect(), "No se puede conectar a la BD: " + ctx.Database.GetConnectionString());
                Assert.IsNotNull(iConexion!.Empleados, "DbSet Empleados no está configurado en el DbContext (es null).");

                Assert.IsTrue(Guardar(), "Guardar() falló");
                Assert.IsTrue(Modificar(), "Modificar() falló");
                Assert.IsTrue(Listar(), "Listar() falló");
            }
            catch (Exception ex)
            {
                // muestra la causa real en el panel de pruebas
                Assert.Fail(ex.GetType().Name + ": " + (ex.InnerException?.Message ?? ex.Message));
            }
            finally
            {
                try { Borrar(); } catch { /* cleanup best-effort */ }
            }
        }


        public bool Guardar()
        {
            this.entidad = EntidadesNucleo.Empleados()!;
            this.iConexion!.Empleados!.Add(this.entidad);
            this.iConexion!.SaveChanges();
            // aquí EF ya devuelve IdEmpleado asignado
            return this.entidad.IdEmpleado > 0;
        }

        public bool Modificar()
        {
            entidad!.Apellidos = entidad.Apellidos + " Actualizado";
            entidad.Activo = !entidad.Activo;

            var entry = iConexion!.Entry(entidad);   // esto ya la adjunta si no lo está
            entry.State = EntityState.Modified;

            iConexion.SaveChanges();
            return true;
        }


        public bool Listar()
        {
            this.lista = this.iConexion!.Empleados!.ToList();
            return this.lista.Count > 0;
        }

        public bool Borrar()
        {
            if (this.entidad is null) return true;
            this.iConexion!.Empleados!.Remove(this.entidad);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}
