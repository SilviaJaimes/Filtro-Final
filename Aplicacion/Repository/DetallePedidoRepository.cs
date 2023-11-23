using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;

public class DetallePedidoRepository : GenericRepositoryStr<DetallePedido>, IDetallePedido
{
    protected readonly ApiContext  _context;
    
    public DetallePedidoRepository(ApiContext context) : base (context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<DetallePedido>> GetAllAsync()
    {
        return await _context.DetallePedidos
            .Include(p => p.Id)
            .ToListAsync();
    }

    public override async Task<DetallePedido> GetByIdAsync(string id)
    {
        return await _context.DetallePedidos
        .Include(p => p.Id)
        .FirstOrDefaultAsync(p =>  p.Id == id);
    }
}