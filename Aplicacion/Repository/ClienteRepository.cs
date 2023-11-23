using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;

public class ClienteRepository : GenericRepository<Cliente>, ICliente
{
    protected readonly ApiContext  _context;
    
    public ClienteRepository(ApiContext context) : base (context)
    {
        _context = context;
    }

    //Consulta 1 
    public async Task<IEnumerable<object>> ClientesYPedidos()
    {
        var clientes = await (
            from c in _context.Clientes
            select new
            {
                NombreCliente = c.NombreCliente,
                CantidadPedidos = c.Pedidos.Count()
            }
        ).ToListAsync();

        return clientes;
    }

    //Consulta 9
    public async Task<IEnumerable<object>> ClientesConPedidoTardio()
    {
        var clientes = await (
            from c in _context.Clientes
            join p in _context.Pedidos on c.Id equals p.CodigoCliente
            where p.FechaEntrega > p.FechaEsperada
            select new
            {
                NombreCliente = c.NombreCliente,
                FechaDeEntrega = p.FechaEntrega,
                FechaEsperada = p.FechaEsperada
            }
        ).ToListAsync();

        return clientes;
    }

    public override async Task<IEnumerable<Cliente>> GetAllAsync()
    {
        return await _context.Clientes
            .Include(p => p.Id)
            .ToListAsync();
    }

    public override async Task<Cliente> GetByIdAsync(int id)
    {
        return await _context.Clientes
        .Include(p => p.Id)
        .FirstOrDefaultAsync(p =>  p.Id == id);
    }
}