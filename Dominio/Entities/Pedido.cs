namespace Dominio.Entities;

public class Pedido : BaseEntity
{
    public DateOnly FechaPedido { get; set; }
    public DateOnly FechaEsperada { get; set; }
    public DateOnly? FechaEntrega { get; set; }
    public string Estado { get; set; }
    public string Comentario { get; set; }
    public int CodigoCliente { get; set; }
    public Cliente Cliente { get; set; }

    public ICollection<DetallePedido> DetallePedidos { get; set; }
}