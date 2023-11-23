using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IGamaProducto : IGenericRepositoryStr<GamaProducto>
{
    //Consulta 10
    Task<IEnumerable<object>> GamasPorCliente();
}