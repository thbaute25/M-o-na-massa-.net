using MaoNaMassa.API.Helpers;
using MaoNaMassa.Application.DTOs.Input;
using MaoNaMassa.Application.DTOs.Output;
using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaoNaMassa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AulasController : ControllerBase
{
    private readonly IAulaRepository _repository;

    public AulasController(IAulaRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Busca todas as aulas de um curso
    /// </summary>
    [HttpGet("curso/{cursoId}")]
    public async Task<ActionResult<IEnumerable<AulaResponse>>> GetByCursoId(Guid cursoId)
    {
        var aulas = await _repository.GetByCursoIdAsync(cursoId);
        var responses = aulas.Select(a => MapToResponse(a)).ToList();

        return Ok(new { data = responses });
    }

    /// <summary>
    /// Busca uma aula por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<AulaResponse>> GetById(Guid id)
    {
        var aula = await _repository.GetByIdAsync(id);
        if (aula == null)
        {
            return NotFound();
        }

        var response = MapToResponse(aula);
        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/aulas",
            id);

        return Ok(new { data = response, links });
    }

    /// <summary>
    /// Cria uma nova aula
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AulaResponse>> Create([FromBody] CriarAulaRequest request)
    {
        var aula = new Aula(
            request.CursoId,
            request.Titulo,
            request.Conteudo,
            request.Ordem);

        var aulaCriada = await _repository.CreateAsync(aula);
        var response = MapToResponse(aulaCriada);

        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/aulas",
            aulaCriada.Id);

        return CreatedAtAction(
            nameof(GetById),
            new { id = aulaCriada.Id },
            new { data = response, links });
    }

    /// <summary>
    /// Atualiza uma aula
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<AulaResponse>> Update(Guid id, [FromBody] CriarAulaRequest request)
    {
        var aula = await _repository.GetByIdAsync(id);
        if (aula == null)
        {
            return NotFound();
        }

        aula.SetTitulo(request.Titulo);
        aula.SetConteudo(request.Conteudo);
        aula.SetOrdem(request.Ordem);

        await _repository.UpdateAsync(aula);

        var response = MapToResponse(aula);
        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/aulas",
            id);

        return Ok(new { data = response, links });
    }

    /// <summary>
    /// Remove uma aula
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var aula = await _repository.GetByIdAsync(id);
        if (aula == null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(id);
        return NoContent();
    }

    private AulaResponse MapToResponse(Aula aula)
    {
        return new AulaResponse
        {
            Id = aula.Id,
            CursoId = aula.CursoId,
            Titulo = aula.Titulo,
            Conteudo = aula.Conteudo,
            Ordem = aula.Ordem
        };
    }
}

