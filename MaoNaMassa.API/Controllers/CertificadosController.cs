using MaoNaMassa.API.Helpers;
using MaoNaMassa.Application.DTOs.Input;
using MaoNaMassa.Application.DTOs.Output;
using MaoNaMassa.Application.UseCases;
using MaoNaMassa.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaoNaMassa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CertificadosController : ControllerBase
{
    private readonly ICertificadoRepository _repository;
    private readonly CompletarCursoUseCase _completarCursoUseCase;

    public CertificadosController(
        ICertificadoRepository repository,
        CompletarCursoUseCase completarCursoUseCase)
    {
        _repository = repository;
        _completarCursoUseCase = completarCursoUseCase;
    }

    /// <summary>
    /// Busca todos os certificados de um usuário
    /// </summary>
    [HttpGet("usuario/{usuarioId}")]
    public async Task<ActionResult<IEnumerable<CertificadoResponse>>> GetByUsuarioId(Guid usuarioId)
    {
        var certificados = await _repository.GetByUsuarioIdAsync(usuarioId);
        var responses = certificados.Select(c => MapToResponse(c)).ToList();

        return Ok(new { data = responses });
    }

    /// <summary>
    /// Busca um certificado por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CertificadoResponse>> GetById(Guid id)
    {
        var certificado = await _repository.GetByIdAsync(id);
        if (certificado == null)
        {
            return NotFound();
        }

        var response = MapToResponse(certificado);
        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/certificados",
            id,
            includeDelete: false);

        return Ok(new
        {
            data = response,
            links
        });
    }

    /// <summary>
    /// Busca um certificado por código
    /// </summary>
    [HttpGet("codigo/{codigo}")]
    public async Task<ActionResult<CertificadoResponse>> GetByCodigo(string codigo)
    {
        var certificado = await _repository.GetByCodigoAsync(codigo);
        if (certificado == null)
        {
            return NotFound();
        }

        var response = MapToResponse(certificado);
        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/certificados",
            certificado.Id,
            includeDelete: false);

        return Ok(new
        {
            data = response,
            links
        });
    }

    /// <summary>
    /// Completa um curso e gera certificado
    /// </summary>
    [HttpPost("completar-curso")]
    public async Task<ActionResult<CertificadoResponse>> CompletarCurso([FromBody] CompletarCursoRequest request, [FromQuery] Guid usuarioId)
    {
        var certificadoDto = await _completarCursoUseCase.ExecuteAsync(usuarioId, request.CursoId);
        var certificado = await _repository.GetByIdAsync(certificadoDto.Id);

        var response = MapToResponse(certificado!);
        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/certificados",
            certificadoDto.Id,
            includeDelete: false);

        return CreatedAtAction(
            nameof(GetById),
            new { id = certificadoDto.Id },
            new { data = response, links });
    }

    private CertificadoResponse MapToResponse(Domain.Entities.Certificado certificado)
    {
        return new CertificadoResponse
        {
            Id = certificado.Id,
            UsuarioId = certificado.UsuarioId,
            CursoId = certificado.CursoId,
            NomeUsuario = certificado.Usuario?.Nome ?? string.Empty,
            TituloCurso = certificado.Curso?.Titulo ?? string.Empty,
            NotaFinal = certificado.NotaFinal,
            DataConclusao = certificado.DataConclusao,
            CodigoCertificado = certificado.CodigoCertificado
        };
    }
}

