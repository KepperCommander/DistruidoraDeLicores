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
        private readonly IConexion iConexion;
        private List<Empleados>? lista;
        private Empleados? entidad;

        public EmpleadosPrueba()
        {
            iConexion = new Conexion { StringConexion = Configuracion.ObtenerValor("StringConexion") };
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
            lista = iConexion.Empleados!.AsNoTracking().Take(50).ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
            entidad = EntidadesNucleo.Empleados() ?? new Empleados
            {
                Nombres = "EmpleadoPrueba",
                Apellidos = "Unit",
                Email = $"empleado.{Guid.NewGuid():N}@prueba.local", // evita UNIQUE
                Telefono = "3000000000",
                IdRol = 1,                  // FK válida (1..5 en tu seed)
                FechaIngreso = DateTime.Today,
                Activo = true
            };

            if (entidad.IdRol <= 0) entidad.IdRol = 1;
            if (string.IsNullOrWhiteSpace(entidad.Email))
                entidad.Email = $"empleado.{Guid.NewGuid():N}@prueba.local";

            iConexion.Empleados!.Add(entidad);
            iConexion.SaveChanges();
            return entidad.IdEmpleado > 0;
        }

        public bool Modificar()
        {
            entidad!.Apellidos += " Actualizado";
            entidad.Activo = !entidad.Activo;

            // si usas NoTracking global, esto lo hace robusto
            var entry = iConexion.Entry(entidad);
            entry.State = EntityState.Modified;

            iConexion.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            if (entidad is null) return true;
            iConexion.Empleados!.Remove(entidad);
            iConexion.SaveChanges();
            return true;
        }
    }
}
