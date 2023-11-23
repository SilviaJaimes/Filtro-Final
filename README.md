
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

En esta consulta se seleccione la tabla clientes en la cual se trae el nombre del cliente en el `select`, luego se ingresa a la tabla de pedidos en donde se encuentra la cantidad de pedidos y se hace un `.Count()` que cuenta la cantidad de pedidos totales por cliente.

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

Aquí se ingresa a la tabla de pedidos y se hace un `where` en donde se cumpla la condición de que la fecha esperada sea menos a la fecha de entrega, esto traerá los pedidos que no fueron entregados antes o en la fecha esperada, los datos que devuelve el `select` son: el código del pedido, el código del cliente, la fecha esperada y la fecha de entrega.

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

Esta consulta se inicia entrando a la tabla de productos con el `from`, luego se hace un `join` a la tabla de detalle pedido en la cual se realiza un grupo que luego se selecciona en el `where`, en donde hace la condición de que no haya algún dato en el código del producto en la tabla detalle pedido, esto retornará un `select` con los datos del código del producto y el nombre de este.

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

Aquí se inglesa a la tabla empleados en donde realiza una serie de condiciones con el `Where`, la primera es que en la tabla clientes exista el codigo del empleado, es decir que sea representante de algún cliente, luego se condiciona que el código de oficina en empleados no sea nulo, y por último se ingresa a las diferentes tablas de pedido, detalle pedido y producto en la cual se selecciona que la gama sea "Frutales", esto al final retorna un `Select` que traerá el Id de la oficina.

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

En esta consulta se ingresa a la tabla de datelle pedidos, luego hago un `include` a la tabla de producto y hago un grupo en el cual uno el código del producto, nombre y el precio de venta, luego selecciono toda la información mencionada anteriormente y hago el total con `Sum`, en este convierto tanto la cantidad como el precio de vente a `float`. Luego hago otro select con el total del resultado anterior y selecciono el nombre del producto, las unidades vendidas y aquí hago un `where` en donde verifico que el código del producto sea igual al código que se encuentra en la tabla de detalle pedido, saco el total, más el total con impuestos, en donde pongo el total normal y lo multiplico por 1.21(21%) y retorno lo pedido en la consulta. 

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

Aquí ingreso a la tabla de empleados, hago un join en donde relaciono el Id del empleado con el código del empleado que se encuentra en la tabla de cliente y realizo un grupo, luego hago un `where` en donde verifico que la información no este en el grupo con el ! y `.Any()`, esto retornará el nombre, el primer y segundo apellido, el puesto y el teléfono de oficina en la cual se encuentra el representante de ventas.

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

En esta consulta ingreso a la tabla productos y hago un `Where` que verifique que en la tabla pedidos este relaciono el dato del código del producto con el Id de este, luego hago otro en donde comparo que la cantidad de stock sea igual a la de detalle pedidos, y que sque el máximo de la cantidad de productos vendidos, hago un `Select` en donde traigo el nombre y luego hago que traiga el primer dato con mayor cantidad vendida.

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

Aquí ingreso a la tabla de datlle pedidos, luego hago un grpo con el código del producto y selecciono el código de este y la cantidad vendida, en esta hago una suma de la cantidad que se encuentra en el grupo con `Sum`, luego lo ordeno de forma descendente en donde se ordena según la cantidad vendida. Al final selecciono los primeros 20 y los meto en una lista.

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

En esta consulta se ingresa a la tabla clientes, se hace un `join` a la tabla de pedidos en donde se relaciona con el código del cliente, luego antes de realizar el `select`, se hace un `where` en donde específico que me traiga los datos en los que la fecha de entrega es mayor a la fecha esperada, esto traerá el nombre de los clientes que tengan estos datos según el `where`, junto con ambas fechas. 

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

Aquí comienzo ingresando a la tabla clientes, luego selecciono la información necesaria, empezando por el nombre del cliente, luego las gamas, haga realizo una subconsulta en donde selecciono la tabala pedidos y hago un `join` con la tabla de detalle pedidos, luego verifico con un `where` en donde el código del cliente en pedido tiene que ser igual al Id del cliente, realizo otro en donde el estado del pedido sea igual a "Entregado", ya que esto me ayudará a traer la información exacta de los productos comprados, hago un select en donde trae el nombre de la gama, para esto ingreso a detalle pedido, luego a producto y al final a gama producto que es en donde voy a seleccionar el nombre de la gama(Id), pongo el `.Distinc()` que me traerá solo gamas diferentes y al final los uno en una lista.

## Desarrollo ⌨️
Este proyecto utiliza varias tecnologías y patrones, incluidos:

Patrón Repository y Unit of Work para la gestión de datos.

AutoMapper para el mapeo entre entidades y DTOs.

## Agradecimientos 🎁

A todas las librerías y herramientas utilizadas en este proyecto.

A ti, por considerar el uso de este sistema.

⌨️ con ❤️ por Silvia.
