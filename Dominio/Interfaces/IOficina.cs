using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IOficina : IGenericRepositoryStr<Oficina>
{
    //Consulta 4
    Task<IEnumerable<object>> OficinaSinRepresentantes();
}