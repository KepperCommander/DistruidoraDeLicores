using System.Collections.Generic;
using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IDetallesPedidosComprasAplicacion
    {
        void Configurar(string stringConexion);

        DetallesPedidosCompras? Guardar(DetallesPedidosCompras? entidad);
        DetallesPedidosCompras? Modificar(DetallesPedidosCompras? entidad);
        DetallesPedidosCompras? Borrar(DetallesPedidosCompras? entidad);

        List<DetallesPedidosCompras> Listar(int take = 100);
        List<DetallesPedidosCompras> PorPedido(int idPedidoCompra);
        List<DetallesPedidosCompras> PorProducto(int idProducto);
        DetallesPedidosCompras? PorId(int idDetallePC);
    }
}
