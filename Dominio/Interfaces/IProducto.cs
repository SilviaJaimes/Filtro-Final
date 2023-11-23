using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IProducto : IGenericRepositoryStr<Producto>
{
    //Consulta 3
    Task<IEnumerable<object>> ProductosQueNoHanAparecidoEnPedidos();

    //Consulta 5
    Task<IEnumerable<object>> TotalProductosMas3000Euros();

    //Consulta 7
    Task<string> ProductoConMásuUnidadesVendidas();

    //Consulta 8
    Task<IEnumerable<object>> VeinteProductosMasVendidos();
}