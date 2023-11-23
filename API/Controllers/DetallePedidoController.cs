using API.Dtos;
using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class DetallePedidoController : BaseApiController
{
    private readonly IUnitOfWork unitofwork;
    private readonly  IMapper mapper;

    public DetallePedidoController(IUnitOfWork unitofwork, IMapper mapper)
    {
        this.unitofwork = unitofwork;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<DetallePedidoDto>>> Get()
    {
        var DetallePedido = await unitofwork.DetallePedidos.GetAllAsync();
        return mapper.Map<List<DetallePedidoDto>>(DetallePedido);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<ActionResult<DetallePedidoDto>> Get(string id)
    {
        var DetallePedido = await unitofwork.DetallePedidos.GetByIdAsync(id);
        if (DetallePedido == null){
            return NotFound();
        }
        return this.mapper.Map<DetallePedidoDto>(DetallePedido);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DetallePedido>> Post(DetallePedidoDto DetallePedidoDto)
    {
        var DetallePedido = this.mapper.Map<DetallePedido>(DetallePedidoDto);
        this.unitofwork.DetallePedidos.Add(DetallePedido);
        await unitofwork.SaveAsync();
        if(DetallePedido == null)
        {
            return BadRequest();
        }
        DetallePedidoDto.Id = DetallePedido.Id;
        return CreatedAtAction(nameof(Post), new {id = DetallePedidoDto.Id}, DetallePedidoDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DetallePedidoDto>> Put(int id, [FromBody]DetallePedidoDto DetallePedidoDto){
        if(DetallePedidoDto == null)
        {
            return NotFound();
        }
        var DetallePedido = this.mapper.Map<DetallePedido>(DetallePedidoDto);
        unitofwork.DetallePedidos.Update(DetallePedido);
        await unitofwork.SaveAsync();
        return DetallePedidoDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id){
        var DetallePedido = await unitofwork.DetallePedidos.GetByIdAsync(id);
        if(DetallePedido == null)
        {
            return NotFound();
        }
        unitofwork.DetallePedidos.Remove(DetallePedido);
        await unitofwork.SaveAsync();
        return NoContent();
    }
}