using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IPedido : IGenericRepository<Pedido>
{
    //Consulta 2
    Task<IEnumerable<Object>> PedidosSinEntregarATiempo();
}