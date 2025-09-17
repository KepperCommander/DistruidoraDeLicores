using lib_dominio.Entidades;

namespace ut_presentacion.Nucleo
{
    public class EntidadesNucleo
    {
        public static Mascotas? Mascotas()
        {
            var entidad = new Mascotas();
            entidad.Nombre = "Pruebas-" + DateTime.Now.ToString("yyyyMMddhhmmss");
            entidad.Peso = 20.4m;

            return entidad;
        }

        public static Dueños? Dueños()
        {
            var entidad = new Dueños();
            entidad.Cedula = "Pruebas-" + DateTime.Now.ToString("yyyyMMddhhmmss");
            entidad.Nombre = "Pruebas-" + DateTime.Now.ToString("yyyyMMddhhmmss");

            return entidad;
        }

        public static Mascotas_Dueños? Mascotas_Dueños()
        {
            var entidad = new Mascotas_Dueños();
            entidad.Mascota = 1;
            entidad.Dueño = 1;

            return entidad;
        }
    }
}
