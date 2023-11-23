using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository;

public class ProductoRepository : GenericRepositoryStr<Producto>, IProducto
{
    protected readonly ApiContext  _context;
    
    public ProductoRepository(ApiContext context) : base (context)
    {
        _context = context;
    }

    //Consulta 3
    public async Task<IEnumerable<object>> ProductosQueNoHanAparecidoEnPedidos()
    {
        var productos = await (
            from p in _context.Productos
            join dp in _context.DetallePedidos on p.Id equals dp.CodigoProducto into Grupo
            where !Grupo.Any()
            select new
            {
                CodigoProducto = p.Id,
                Nombre = p.Nombre
            }
        ).ToListAsync();

        return productos;
    }

    //Consulta 5
    public async Task<IEnumerable<object>> TotalProductosMas3000Euros()
    {
        var totalProductos = await _context.DetallePedidos
            .Include(dp => dp.Producto)
            .GroupBy(
                dp => new { dp.CodigoProducto, dp.Producto.Nombre, dp.Producto.PrecioVenta },
                (Principal, GrupoSuma) => new
                {
                    Principal.CodigoProducto,
                    Principal.Nombre,
                    Principal.PrecioVenta,
                    Total = GrupoSuma.Sum(dp => (float)dp.Cantidad * (float)dp.Producto.PrecioVenta)
                })
            .Where(result => result.Total * 1.21 > 3000)
            .ToListAsync();

        var productos = totalProductos
            .Select(p => new
            {
                p.Nombre,
                UnidadesVendidas = _context.DetallePedidos
                    .Where(dp => dp.CodigoProducto == p.CodigoProducto)
                    .Sum(dp => dp.Cantidad),
                p.Total,
                TotalConImpuestos = p.Total * 1.21
            })
            .ToList();

        return productos;
    }

    //Consulta 7
    public async Task<string> ProductoConMásuUnidadesVendidas()
    {
        var productoMásVendido = await _context.Productos
            .Where(p => p.DetallePedidos.Any())
            .Where(p => p.CantidadStock == p.DetallePedidos.Max(p => p.Cantidad))
            .Select(p => p.Nombre)
            .FirstOrDefaultAsync();

        return productoMásVendido;
    }

    //Consulta 8
    public async Task<IEnumerable<object>> VeinteProductosMasVendidos()
    {
        var prductoConMayorVentas = await (
            from dp in _context.DetallePedidos
            group dp by dp.CodigoProducto into grupo
            select new
            {
                CodigoProducto = grupo.Key,
                CantidadVendida = grupo.Sum(dp => dp.Cantidad)
            }
            into resultado
            orderby resultado.CantidadVendida descending
            select resultado
        )
        .Take(20)
        .ToListAsync();

        return prductoConMayorVentas;
    }

    public override async Task<IEnumerable<Producto>> GetAllAsync()
    {
        return await _context.Productos
            .Include(p => p.Id)
            .ToListAsync();
    }

    public override async Task<Producto> GetByIdAsync(string id)
    {
        return await _context.Productos
        .Include(p => p.Id)
        .FirstOrDefaultAsync(p =>  p.Id == id);
    }
}