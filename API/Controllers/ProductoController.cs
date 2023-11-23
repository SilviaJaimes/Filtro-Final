using API.Dtos;
using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductoController : BaseApiController
{
    private readonly IUnitOfWork unitofwork;
    private readonly  IMapper mapper;

    public ProductoController(IUnitOfWork unitofwork, IMapper mapper)
    {
        this.unitofwork = unitofwork;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProductoDto>>> Get()
    {
        var Producto = await unitofwork.Productos.GetAllAsync();
        return mapper.Map<List<ProductoDto>>(Producto);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductoDto>> Get(string id)
    {
        var Producto = await unitofwork.Productos.GetByIdAsync(id);
        if (Producto == null){
            return NotFound();
        }
        return this.mapper.Map<ProductoDto>(Producto);
    }

    [HttpGet("consulta-3")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> ProductosQueNoHanAparecidoEnPedidos()
    {
        var entidad = await unitofwork.Productos.ProductosQueNoHanAparecidoEnPedidos();
        var dto = mapper.Map<IEnumerable<object>>(entidad);
        return Ok(dto);
    }

    [HttpGet("consulta-5")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> TotalProductosMas3000Euros()
    {
        var entidad = await unitofwork.Productos.TotalProductosMas3000Euros();
        var dto = mapper.Map<IEnumerable<object>>(entidad);
        return Ok(dto);
    }

    [HttpGet("consulta-7")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> ProductoConMásuUnidadesVendidas()
    {
        var entidad = await unitofwork.Productos.ProductoConMásuUnidadesVendidas();
        var dto = mapper.Map<string>(entidad);
        return Ok(dto);
    }

    [HttpGet("consulta-8")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> VeinteProductosMasVendidos()
    {
        var entidad = await unitofwork.Productos.VeinteProductosMasVendidos();
        var dto = mapper.Map<IEnumerable<object>>(entidad);
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Producto>> Post(ProductoDto ProductoDto)
    {
        var Producto = this.mapper.Map<Producto>(ProductoDto);
        this.unitofwork.Productos.Add(Producto);
        await unitofwork.SaveAsync();
        if(Producto == null)
        {
            return BadRequest();
        }
        ProductoDto.Id = Producto.Id;
        return CreatedAtAction(nameof(Post), new {id = ProductoDto.Id}, ProductoDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductoDto>> Put(int id, [FromBody]ProductoDto ProductoDto){
        if(ProductoDto == null)
        {
            return NotFound();
        }
        var Producto = this.mapper.Map<Producto>(ProductoDto);
        unitofwork.Productos.Update(Producto);
        await unitofwork.SaveAsync();
        return ProductoDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id){
        var Producto = await unitofwork.Productos.GetByIdAsync(id);
        if(Producto == null)
        {
            return NotFound();
        }
        unitofwork.Productos.Remove(Producto);
        await unitofwork.SaveAsync();
        return NoContent();
    }
}