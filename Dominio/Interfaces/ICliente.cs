using Dominio.Entities;

namespace Dominio.Interfaces;

public interface ICliente : IGenericRepository<Cliente>
{
    //Consulta 1
    Task<IEnumerable<object>> ClientesYPedidos();

    //Consulta 9
    Task<IEnumerable<object>> ClientesConPedidoTardio();
}