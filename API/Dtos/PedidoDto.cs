using Dominio.Entities;

namespace API.Dtos;

public class PedidoDto : BaseEntity
{
    public DateOnly FechaPedido { get; set; }
    public DateOnly FechaEsperada { get; set; }
    public DateOnly? FechaEntrega { get; set; }
    public string Estado { get; set; }
    public string Comentario { get; set; }
    public int CodigoCliente { get; set; }
    public ClienteDto Cliente { get; set; }
}