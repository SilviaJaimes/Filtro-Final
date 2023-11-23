using API.Dtos;
using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PedidoController : BaseApiController
{
    private readonly IUnitOfWork unitofwork;
    private readonly  IMapper mapper;

    public PedidoController(IUnitOfWork unitofwork, IMapper mapper)
    {
        this.unitofwork = unitofwork;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<PedidoDto>>> Get()
    {
        var Pedido = await unitofwork.Pedidos.GetAllAsync();
        return mapper.Map<List<PedidoDto>>(Pedido);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<ActionResult<PedidoDto>> Get(int id)
    {
        var Pedido = await unitofwork.Pedidos.GetByIdAsync(id);
        if (Pedido == null){
            return NotFound();
        }
        return this.mapper.Map<PedidoDto>(Pedido);
    }

    [HttpGet("consulta-2")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> PedidosSinEntregarATiempo()
    {
        var entidad = await unitofwork.Pedidos.PedidosSinEntregarATiempo();
        var dto = mapper.Map<IEnumerable<object>>(entidad);
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pedido>> Post(PedidoDto PedidoDto)
    {
        var Pedido = this.mapper.Map<Pedido>(PedidoDto);
        this.unitofwork.Pedidos.Add(Pedido);
        await unitofwork.SaveAsync();
        if(Pedido == null)
        {
            return BadRequest();
        }
        PedidoDto.Id = Pedido.Id;
        return CreatedAtAction(nameof(Post), new {id = PedidoDto.Id}, PedidoDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PedidoDto>> Put(int id, [FromBody]PedidoDto PedidoDto){
        if(PedidoDto == null)
        {
            return NotFound();
        }
        var Pedido = this.mapper.Map<Pedido>(PedidoDto);
        unitofwork.Pedidos.Update(Pedido);
        await unitofwork.SaveAsync();
        return PedidoDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id){
        var Pedido = await unitofwork.Pedidos.GetByIdAsync(id);
        if(Pedido == null)
        {
            return NotFound();
        }
        unitofwork.Pedidos.Remove(Pedido);
        await unitofwork.SaveAsync();
        return NoContent();
    }
}