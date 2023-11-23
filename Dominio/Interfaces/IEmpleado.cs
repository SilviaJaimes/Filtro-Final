using Dominio.Entities;

namespace Dominio.Interfaces;

public interface IEmpleado : IGenericRepository<Empleado>
{
    //Consulta 6
    Task<IEnumerable<object>> EmpleadosQueNoSeanRepresentantesDeClientes();
}