using System.Collections.Generic;
using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IInventariosProductosAplicacion
    {
        void Configurar(string stringConexion);

        InventariosProductos? Guardar(InventariosProductos? entidad);
        InventariosProductos? Modificar(InventariosProductos? entidad);
        InventariosProductos? Borrar(InventariosProductos? entidad);

        List<InventariosProductos> Listar(int take = 100);
        List<InventariosProductos> PorTexto(string texto, int take = 100);
        InventariosProductos? PorId(int idInventario);
        List<InventariosProductos> PorAlmacen(int idAlmacen, int take = 100);
        InventariosProductos? PorProducto(int idProducto);
    }
}
