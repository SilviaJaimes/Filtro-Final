namespace Dominio.Entities;

public class DetallePedido : BaseEntityStr
{
    public int CodigoPedido { get; set; }
    public Pedido Pedido { get; set; }
    public string CodigoProducto { get; set; }
    public Producto Producto { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnidad { get; set; }
    public short NumeroLinea { get; set; }
}