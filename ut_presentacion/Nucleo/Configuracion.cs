using Microsoft.Extensions.Configuration;
using lib_dominio.Nucleo; // para DatosGenerales y JsonConversor

namespace ut_presentacion.Nucleo
{
    public static class Configuracion
    {
        private static readonly IConfigurationRoot _cfg =
            new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

        public static string ObtenerValor(string clave)
        {
            var v = _cfg[clave];                       // <- lee solo raíz
            if (string.IsNullOrWhiteSpace(v))
                throw new InvalidOperationException($"No existe la clave '{clave}'.");
            return v!;
        }
    }
}

