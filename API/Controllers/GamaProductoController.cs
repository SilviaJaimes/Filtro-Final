using API.Dtos;
using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class GamaProductoController : BaseApiController
{
    private readonly IUnitOfWork unitofwork;
    private readonly  IMapper mapper;

    public GamaProductoController(IUnitOfWork unitofwork, IMapper mapper)
    {
        this.unitofwork = unitofwork;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<GamaProductoDto>>> Get()
    {
        var GamaProducto = await unitofwork.GamaProductos.GetAllAsync();
        return mapper.Map<List<GamaProductoDto>>(GamaProducto);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GamaProductoDto>> Get(string id)
    {
        var GamaProducto = await unitofwork.GamaProductos.GetByIdAsync(id);
        if (GamaProducto == null){
            return NotFound();
        }
        return this.mapper.Map<GamaProductoDto>(GamaProducto);
    }

    [HttpGet("consulta-10")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> GamasPorCliente()
    {
        var entidad = await unitofwork.GamaProductos.GamasPorCliente();
        var dto = mapper.Map<IEnumerable<object>>(entidad);
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GamaProducto>> Post(GamaProductoDto GamaProductoDto)
    {
        var GamaProducto = this.mapper.Map<GamaProducto>(GamaProductoDto);
        this.unitofwork.GamaProductos.Add(GamaProducto);
        await unitofwork.SaveAsync();
        if(GamaProducto == null)
        {
            return BadRequest();
        }
        GamaProductoDto.Id = GamaProducto.Id;
        return CreatedAtAction(nameof(Post), new {id = GamaProductoDto.Id}, GamaProductoDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GamaProductoDto>> Put(int id, [FromBody]GamaProductoDto GamaProductoDto){
        if(GamaProductoDto == null)
        {
            return NotFound();
        }
        var GamaProducto = this.mapper.Map<GamaProducto>(GamaProductoDto);
        unitofwork.GamaProductos.Update(GamaProducto);
        await unitofwork.SaveAsync();
        return GamaProductoDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id){
        var GamaProducto = await unitofwork.GamaProductos.GetByIdAsync(id);
        if(GamaProducto == null)
        {
            return NotFound();
        }
        unitofwork.GamaProductos.Remove(GamaProducto);
        await unitofwork.SaveAsync();
        return NoContent();
    }
}