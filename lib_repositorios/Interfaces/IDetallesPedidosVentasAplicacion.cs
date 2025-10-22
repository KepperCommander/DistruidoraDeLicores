using System.Collections.Generic;
using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IDetallesPedidosVentasAplicacion
    {
        void Configurar(string stringConexion);

        DetallesPedidosVentas? Guardar(DetallesPedidosVentas? entidad);
        DetallesPedidosVentas? Modificar(DetallesPedidosVentas? entidad);
        DetallesPedidosVentas? Borrar(DetallesPedidosVentas? entidad);

        List<DetallesPedidosVentas> Listar(int take = 100);
        List<DetallesPedidosVentas> PorTexto(string texto, int take = 100);
        List<DetallesPedidosVentas> PorPedido(int idPedidoVenta);
        DetallesPedidosVentas? PorId(int idDetallePV);
    }
}
