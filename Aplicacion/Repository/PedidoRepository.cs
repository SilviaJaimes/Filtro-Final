using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;

public class PedidoRepository : GenericRepository<Pedido>, IPedido
{
    protected readonly ApiContext  _context;
    
    public PedidoRepository(ApiContext context) : base (context)
    {
        _context = context;
    }

    
    //Consulta 2
    public async Task<IEnumerable<Object>> PedidosSinEntregarATiempo()
    {
        var pedidos = await (
            from p in _context.Pedidos
            where p.FechaEsperada < p.FechaEntrega
            select new
            {
                CodigoPedido = p.Id,
                CodigoCliente = p.CodigoCliente,
                FechaEsperada = p.FechaEsperada,
                FechaEntrega = p.FechaEntrega
            }).ToListAsync();

        return pedidos;
    }

    public override async Task<IEnumerable<Pedido>> GetAllAsync()
    {
        return await _context.Pedidos
            .Include(p => p.Id)
            .ToListAsync();
    }

    public override async Task<Pedido> GetByIdAsync(int id)
    {
        return await _context.Pedidos
        .Include(p => p.Id)
        .FirstOrDefaultAsync(p =>  p.Id == id);
    }
}