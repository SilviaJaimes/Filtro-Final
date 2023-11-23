using API.Dtos;
using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class OficinaController : BaseApiController
{
    private readonly IUnitOfWork unitofwork;
    private readonly  IMapper mapper;

    public OficinaController(IUnitOfWork unitofwork, IMapper mapper)
    {
        this.unitofwork = unitofwork;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<IEnumerable<OficinaDto>>> Get()
    {
        var Oficina = await unitofwork.Oficinas.GetAllAsync();
        return mapper.Map<List<OficinaDto>>(Oficina);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<ActionResult<OficinaDto>> Get(string id)
    {
        var Oficina = await unitofwork.Oficinas.GetByIdAsync(id);
        if (Oficina == null){
            return NotFound();
        }
        return this.mapper.Map<OficinaDto>(Oficina);
    }

    [HttpGet("consulta-4")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> OficinaSinRepresentantes()
    {
        var entidad = await unitofwork.Oficinas.OficinaSinRepresentantes();
        var dto = mapper.Map<IEnumerable<object>>(entidad);
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Oficina>> Post(OficinaDto OficinaDto)
    {
        var Oficina = this.mapper.Map<Oficina>(OficinaDto);
        this.unitofwork.Oficinas.Add(Oficina);
        await unitofwork.SaveAsync();
        if(Oficina == null)
        {
            return BadRequest();
        }
        OficinaDto.Id = Oficina.Id;
        return CreatedAtAction(nameof(Post), new {id = OficinaDto.Id}, OficinaDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OficinaDto>> Put(int id, [FromBody]OficinaDto OficinaDto){
        if(OficinaDto == null)
        {
            return NotFound();
        }
        var Oficina = this.mapper.Map<Oficina>(OficinaDto);
        unitofwork.Oficinas.Update(Oficina);
        await unitofwork.SaveAsync();
        return OficinaDto;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id){
        var Oficina = await unitofwork.Oficinas.GetByIdAsync(id);
        if(Oficina == null)
        {
            return NotFound();
        }
        unitofwork.Oficinas.Remove(Oficina);
        await unitofwork.SaveAsync();
        return NoContent();
    }
}