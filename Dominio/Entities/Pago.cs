namespace Dominio.Entities;

public class Pago : BaseEntityStr
{
    public int CodigoCliente { get; set; }
    public Cliente Cliente { get; set; }
    public string FormaPago { get; set; }
    public DateOnly FechaPago { get; set; }
    public decimal Total { get; set; }
}