using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;

public class OficinaRepository : GenericRepositoryStr<Oficina>, IOficina
{
    protected readonly ApiContext  _context;
    
    public OficinaRepository(ApiContext context) : base (context)
    {
        _context = context;
    }

    //Consulta 4
    public async Task<IEnumerable<object>> OficinaSinRepresentantes()
    {
        var oficinas = await _context.Empleados
                    .Where(e => e.Clientes.Any())
                    .Where(e => e.CodigoOficina == null)
                    .Where(e => e.Clientes.Any(c => c.Pedidos.Any(p => p.DetallePedidos.Any(dp => dp.Producto.GamaProducto.Id.Equals("Frutales")))))
                    .Select(emp => new
                    {
                        Oficina = new 
                        {
                            emp.Oficina.Id
                        }
                    }).ToListAsync();

        return oficinas;
    }

    public override async Task<IEnumerable<Oficina>> GetAllAsync()
    {
        return await _context.Oficinas
            .Include(p => p.Id)
            .ToListAsync();
    }

    public override async Task<Oficina> GetByIdAsync(string id)
    {
        return await _context.Oficinas
        .Include(p => p.Id)
        .FirstOrDefaultAsync(p =>  p.Id == id);
    }
}