using MaoNaMassa.API.Helpers;
using MaoNaMassa.Application.DTOs;
using MaoNaMassa.Application.DTOs.Input;
using MaoNaMassa.Application.DTOs.Output;
using MaoNaMassa.Application.UseCases;
using MaoNaMassa.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaoNaMassa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvaliacoesController : ControllerBase
{
    private readonly IAvaliacaoRepository _repository;
    private readonly AvaliarServicoUseCase _avaliarServicoUseCase;

    public AvaliacoesController(
        IAvaliacaoRepository repository,
        AvaliarServicoUseCase avaliarServicoUseCase)
    {
        _repository = repository;
        _avaliarServicoUseCase = avaliarServicoUseCase;
    }

    /// <summary>
    /// Busca todas as avaliações de um serviço
    /// </summary>
    [HttpGet("servico/{servicoId}")]
    public async Task<ActionResult<IEnumerable<AvaliacaoResponse>>> GetByServicoId(Guid servicoId)
    {
        var avaliacoes = await _repository.GetByServicoIdAsync(servicoId);
        var responses = avaliacoes.Select(a => MapToResponse(a)).ToList();

        return Ok(new { data = responses });
    }

    /// <summary>
    /// Busca uma avaliação por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<AvaliacaoResponse>> GetById(Guid id)
    {
        var avaliacao = await _repository.GetByIdAsync(id);
        if (avaliacao == null)
        {
            return NotFound();
        }

        var response = MapToResponse(avaliacao);
        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/avaliacoes",
            id,
            includeDelete: false);

        return Ok(new
        {
            data = response,
            links
        });
    }

    /// <summary>
    /// Cria uma nova avaliação de serviço
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AvaliacaoResponse>> Create([FromBody] AvaliarServicoRequest request, [FromQuery] Guid usuarioId)
    {
        var createDto = new CreateAvaliacaoDTO
        {
            UsuarioId = usuarioId,
            ServicoId = request.ServicoId,
            Nota = request.Nota,
            Comentario = request.Comentario
        };

        var avaliacaoDto = await _avaliarServicoUseCase.ExecuteAsync(createDto);
        var avaliacao = await _repository.GetByIdAsync(avaliacaoDto.Id);

        var response = MapToResponse(avaliacao!);
        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/avaliacoes",
            avaliacaoDto.Id,
            includeDelete: false);

        return CreatedAtAction(
            nameof(GetById),
            new { id = avaliacaoDto.Id },
            new { data = response, links });
    }

    private AvaliacaoResponse MapToResponse(Domain.Entities.Avaliacao avaliacao)
    {
        return new AvaliacaoResponse
        {
            Id = avaliacao.Id,
            ServicoId = avaliacao.ServicoId,
            UsuarioId = avaliacao.UsuarioId,
            NomeUsuario = avaliacao.Usuario?.Nome ?? string.Empty,
            Nota = avaliacao.Nota,
            Comentario = avaliacao.Comentario,
            Data = avaliacao.Data
        };
    }
}

