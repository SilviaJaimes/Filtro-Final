using Dominio.Entities;

namespace API.Dtos;

public class PagoDto : BaseEntityStr
{
    public int CodigoCliente { get; set; }
    public ClienteDto Cliente { get; set; }
    public string FormaPago { get; set; }
    public DateOnly FechaPago { get; set; }
    public decimal Total { get; set; }
}