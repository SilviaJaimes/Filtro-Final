using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;

public class EmpleadoRepository : GenericRepository<Empleado>, IEmpleado
{
    protected readonly ApiContext  _context;
    
    public EmpleadoRepository(ApiContext context) : base (context)
    {
        _context = context;
    }

    //Consulta 6 
    public async Task<IEnumerable<object>> EmpleadosQueNoSeanRepresentantesDeClientes()
    {
        var empleados = await (
            from e in _context.Empleados
            join c in _context.Clientes on e.Id equals c.CodigoEmpleado into Grupo
            where !Grupo.Any()
            select new
            {
                Nombre = e.Nombre,
                PrimerApellido = e.Apellido1,
                SegundoApellido = e.Apellido2,
                Puesto = e.Puesto,
                TelefonoOficina = e.Oficina.Telefono
            }
        ).ToListAsync();

        return empleados;
    }

    public override async Task<IEnumerable<Empleado>> GetAllAsync()
    {
        return await _context.Empleados
            .Include(p => p.Id)
            .ToListAsync();
    }

    public override async Task<Empleado> GetByIdAsync(int id)
    {
        return await _context.Empleados
        .Include(p => p.Id)
        .FirstOrDefaultAsync(p =>  p.Id == id);
    }
}