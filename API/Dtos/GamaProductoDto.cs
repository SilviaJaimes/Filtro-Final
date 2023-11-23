using Dominio.Entities;

namespace API.Dtos;

public class GamaProductoDto : BaseEntityStr
{
    public string DescripcionTexto { get; set; }
    public string DescripcionHtml { get; set; }
    public string Imagen { get; set; }
}