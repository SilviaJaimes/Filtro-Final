using Dominio.Entities;

namespace API.Dtos;

public class EmpleadoDto : BaseEntity
{
    public string Nombre { get; set; }
    public string Apellido1 { get; set; }
    public string Apellido2 { get; set; }
    public string Extension { get; set; }
    public string Email { get; set; }
    public string CodigoOficina { get; set; }
    public OficinaDto Oficina { get; set; }
    public int? CodigoJefe { get; set; }
    public EmpleadoDto Jefe { get; set; }
    public string Puesto { get; set; }
}