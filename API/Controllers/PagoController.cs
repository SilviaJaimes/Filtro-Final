using API.Dtos;
using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PagoController : BaseApiController
{
    private readonly IUnitOfWork unitofwork;
    private readonly  IMapper mapper;

    public PagoController(IUnitOfWork unitofwork, IMapper mapper)
    {
        this.unitofwork = unitofwork;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<IEnumerable<PagoDto>>> Get()
    {
        var Pago = await unitofwork.Pagos.GetAllAsync();
        return mapper.Map<List<PagoDto>>(Pago);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<ActionResult<PagoDto>> Get(string id)
    {
        var Pago = await unitofwork.Pagos.GetByIdAsync(id);
        if (Pago == null){
            return NotFound();
        }
        return this.mapper.Map<PagoDto>(Pago);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pago>> Post(PagoDto PagoDto)
    {
        var Pago = this.mapper.Map<Pago>(PagoDto);
        this.unitofwork.Pagos.Add(Pago);
        await unitofwork.SaveAsync();
        if(Pago == null)
        {
            return BadRequest();
        }
        PagoDto.Id = Pago.Id;
        return CreatedAtAction(nameof(Post), new {id = PagoDto.Id}, PagoDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PagoDto>> Put(int id, [FromBody]PagoDto PagoDto){
        if(PagoDto == null)
        {
            return NotFound();
        }
        var Pago = this.mapper.Map<Pago>(PagoDto);
        unitofwork.Pagos.Update(Pago);
        await unitofwork.SaveAsync();
        return PagoDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id){
        var Pago = await unitofwork.Pagos.GetByIdAsync(id);
        if(Pago == null)
        {
            return NotFound();
        }
        unitofwork.Pagos.Remove(Pago);
        await unitofwork.SaveAsync();
        return NoContent();
    }
}