using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;

public class GamaProductoRepository : GenericRepositoryStr<GamaProducto>, IGamaProducto
{
    protected readonly ApiContext  _context;
    
    public GamaProductoRepository(ApiContext context) : base (context)
    {
        _context = context;
    }

    //Consulta 10
    public async Task<IEnumerable<object>> GamasPorCliente()
    {
        var gamas = await (
            from c in _context.Clientes
            select new
            {
                NombreCliente = c.NombreCliente,
                Gamas = (from p in _context.Pedidos
                        join dp in _context.DetallePedidos on p.Id equals dp.CodigoPedido
                        where p.CodigoCliente == c.Id
                        where p.Estado == "Entregado"
                        select new {
                            Nombre = dp.Producto.GamaProducto.Id
                        })
                        .Distinct()
                        .ToList()
            }
        ).ToListAsync();

        return gamas;
    }

    public override async Task<IEnumerable<GamaProducto>> GetAllAsync()
    {
        return await _context.GamaProductos
            .Include(p => p.Id)
            .ToListAsync();
    }

    public override async Task<GamaProducto> GetByIdAsync(string id)
    {
        return await _context.GamaProductos
        .Include(p => p.Id)
        .FirstOrDefaultAsync(p =>  p.Id == id);
    }
}