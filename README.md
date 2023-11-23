
# Filtro final jardinería 🌿

Este proyecto proporciona una API que permite llevar el control, gestión y registro de todos los productos y servicios de una jardinería.

## Características 🌟

- Registro de usuarios.
- Autenticación con usuario y contraseña.
- Generación y utilización del token.
- CRUD completo para cada entidad.
- Vista de las consultas requeridas.

## Uso 🕹

Una vez que el proyecto esté en marcha, puedes acceder a los diferentes endpoints disponibles:

## Desarrollo de los Endpoints requeridos⌨️

#### 1. Devuelve el listado de clientes indicando el nombre del cliente y cuántos pedidos ha realizado:  

Endpoint: `http://localhost:5020/api/cliente/consulta-1`  

Método: `GET`  

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

#### 2. Devuelve un listado con el código del pedido, código de cliente, fecha esperada y fecha entregada de los pedidos que no han sido entregados a tiempo:  

Endpoint: `http://localhost:5020/api/pedido/consulta-2`  

Método: `GET`  

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

#### 3. Devuelve un listado de los productos que nunca han aparecido en un pedido. El resultado debe mostrar el nombre, la descripción y la imagen del producto:  

Endpoint: `http://localhost:5020/api/producto/consulta-3`  

Método: `GET`  

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

#### 4. Devuelve las oficinas donde no trabajan ninguno de los empleados que hayan sido los representantes de ventas de algún cliente que haya realizado la compra de algún producto de la gama Frutales:  

Endpoint: `http://localhost:5020/api/oficina/consulta-4`  

Método: `GET`  

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

#### 5. Lista de las ventas totales de los productos que hayan facturado más de 3000 euros. Se mostrará el nombre, unidades vendidas, total facturado y total facturado con impuestos (21% IVA):  

Endpoint: `http://localhost:5020/api/producto/consulta-5`  

Método: `GET`  

    public async Task<IEnumerable<object>> TotalProductosMas3000Euros()
    {
        var totalProductos = await _context.DetallePedidos
            .Include(dp => dp.Producto)
            .GroupBy(
                dp => new { dp.CodigoProducto, dp.Producto.Nombre, dp.Producto.PrecioVenta },
                (key, group) => new
                {
                    key.CodigoProducto,
                    key.Nombre,
                    key.PrecioVenta,
                    Total = group.Sum(dp => (float)dp.Cantidad * (float)dp.Producto.PrecioVenta)
                })
            .Where(result => result.Total * 1.21 > 3000)
            .ToListAsync();

        var productos = totalProductos
            .Select(item => new
            {
                item.Nombre,
                UnidadesVendidas = _context.DetallePedidos
                    .Where(dp => dp.CodigoProducto == item.CodigoProducto)
                    .Sum(dp => dp.Cantidad),
                item.Total,
                TotalConImpuestos = item.Total * 1.21
            })
            .ToList();

        return productos;
    }

#### 6. Devuelve el nombre, apellidos, puesto y teléfono de la oficina de aquellos empleados que no sean representante de ventas de ningún cliente:  

Endpoint: `http://localhost:5020/api/empleado/consulta-6`  

Método: `GET`  

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

#### 7. Devuelve el nombre del producto del que se han vendido más unidades:  

Endpoint: `http://localhost:5020/api/producto/consulta-7`  

Método: `GET`  

    public async Task<string> ProductoConMásuUnidadesVendidas()
    {
        var productoMásVendido = await _context.Productos
            .Where(p => p.DetallePedidos.Any())
            .Where(p => p.CantidadStock == p.DetallePedidos.Max(p => p.Cantidad))
            .Select(p => p.Nombre)
            .FirstOrDefaultAsync();

        return productoMásVendido;
    }

#### 8. Devuelve un listado de los 20 productos más vendidos y el número total de unidades que se han vendido de cada uno. El listado deberá estar ordenado por el número total de unidades vendidas:  

Endpoint: `http://localhost:5020/api/producto/consulta-8`  

Método: `GET`  

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

#### 9. Devuelve el nombre de los clientes a los que no se les ha entregado a tiempo un pedido:  

Endpoint: `http://localhost:5020/api/cliente/consulta-9`  

Método: `GET`  

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

#### 10. Devuelve un listado de las diferentes gamas de producto que ha comprado cada cliente:  

Endpoint: `http://localhost:5020/api/gamaProducto/consulta-10`  

Método: `GET`  

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

## Desarrollo ⌨️
Este proyecto utiliza varias tecnologías y patrones, incluidos:

Patrón Repository y Unit of Work para la gestión de datos.

AutoMapper para el mapeo entre entidades y DTOs.

## Agradecimientos 🎁

A todas las librerías y herramientas utilizadas en este proyecto.

A ti, por considerar el uso de este sistema.

⌨️ con ❤️ por Silvia.
