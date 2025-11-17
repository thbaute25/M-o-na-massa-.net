using MaoNaMassa.API.Helpers;
using MaoNaMassa.Application.DTOs.Input;
using MaoNaMassa.Application.DTOs.Output;
using MaoNaMassa.Application.DTOs.Paginacao;
using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaoNaMassa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CursosController : ControllerBase
{
    private readonly ICursoRepository _repository;

    public CursosController(ICursoRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Busca todos os cursos com paginação
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PaginacaoResponse<CursoResponse>>> GetAll(
        [FromQuery] PaginacaoRequest paginacao,
        [FromQuery] string? area = null,
        [FromQuery] string? nivel = null)
    {
        var cursos = await _repository.GetAllAsync();
        var query = cursos.AsQueryable();

        // Filtros
        if (!string.IsNullOrWhiteSpace(area))
        {
            query = query.Where(c => c.Area.Contains(area, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(nivel))
        {
            query = query.Where(c => c.Nivel == nivel);
        }

        // Ordenação
        if (!string.IsNullOrWhiteSpace(paginacao.OrdenarPor))
        {
            query = paginacao.OrdenarPor.ToLower() switch
            {
                "titulo" => paginacao.OrdenarDescendente
                    ? query.OrderByDescending(c => c.Titulo)
                    : query.OrderBy(c => c.Titulo),
                "datacriacao" => paginacao.OrdenarDescendente
                    ? query.OrderByDescending(c => c.DataCriacao)
                    : query.OrderBy(c => c.DataCriacao),
                "area" => paginacao.OrdenarDescendente
                    ? query.OrderByDescending(c => c.Area)
                    : query.OrderBy(c => c.Area),
                _ => query.OrderBy(c => c.Titulo)
            };
        }
        else
        {
            query = query.OrderBy(c => c.Titulo);
        }

        var totalItens = query.Count();
        var cursosPaginados = query
            .Skip(paginacao.Skip)
            .Take(paginacao.Take)
            .ToList();

        var responses = cursosPaginados.Select(c => MapToResponse(c)).ToList();
        var paginacaoResponse = new PaginacaoResponse<CursoResponse>(
            responses,
            paginacao.Pagina,
            paginacao.TamanhoPagina,
            totalItens);

        // HATEOAS
        var queryParams = new Dictionary<string, string>
        {
            { "area", area ?? string.Empty },
            { "nivel", nivel ?? string.Empty },
            { "ordenarPor", paginacao.OrdenarPor ?? string.Empty },
            { "ordenarDescendente", paginacao.OrdenarDescendente.ToString() }
        };

        var links = HateoasHelper.CreatePaginationLinks(
            $"{Request.Scheme}://{Request.Host}/api/cursos",
            paginacao.Pagina,
            paginacaoResponse.TotalPaginas,
            queryParams);

        return Ok(new
        {
            data = paginacaoResponse,
            links
        });
    }

    /// <summary>
    /// Busca um curso por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CursoResponse>> GetById(Guid id)
    {
        var curso = await _repository.GetByIdAsync(id);
        if (curso == null)
        {
            return NotFound();
        }

        var response = MapToResponse(curso);
        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/cursos",
            id);

        return Ok(new
        {
            data = response,
            links
        });
    }

    /// <summary>
    /// Cria um novo curso
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CursoResponse>> Create([FromBody] CriarCursoRequest request)
    {
        var curso = new Curso(
            request.Titulo,
            request.Descricao,
            request.Area,
            request.Nivel);

        var cursoCriado = await _repository.CreateAsync(curso);
        var response = MapToResponse(cursoCriado);

        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/cursos",
            cursoCriado.Id);

        return CreatedAtAction(
            nameof(GetById),
            new { id = cursoCriado.Id },
            new { data = response, links });
    }

    /// <summary>
    /// Atualiza um curso
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<CursoResponse>> Update(Guid id, [FromBody] CriarCursoRequest request)
    {
        var curso = await _repository.GetByIdAsync(id);
        if (curso == null)
        {
            return NotFound();
        }

        curso.SetTitulo(request.Titulo);
        curso.SetDescricao(request.Descricao);
        curso.SetArea(request.Area);
        curso.SetNivel(request.Nivel);

        await _repository.UpdateAsync(curso);

        var response = MapToResponse(curso);
        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/cursos",
            id);

        return Ok(new { data = response, links });
    }

    /// <summary>
    /// Remove um curso
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var curso = await _repository.GetByIdAsync(id);
        if (curso == null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Busca cursos com filtros e paginação
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<PaginacaoResponse<CursoResponse>>> Search(
        [FromQuery] PaginacaoRequest paginacao,
        [FromQuery] string? titulo = null,
        [FromQuery] string? area = null,
        [FromQuery] string? nivel = null)
    {
        var cursos = await _repository.GetAllAsync();
        var query = cursos.AsQueryable();

        // Filtros
        if (!string.IsNullOrWhiteSpace(titulo))
        {
            query = query.Where(c => c.Titulo.Contains(titulo, StringComparison.OrdinalIgnoreCase) ||
                                     c.Descricao.Contains(titulo, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(area))
        {
            query = query.Where(c => c.Area.Contains(area, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(nivel))
        {
            query = query.Where(c => c.Nivel == nivel);
        }

        // Ordenação
        if (!string.IsNullOrWhiteSpace(paginacao.OrdenarPor))
        {
            query = paginacao.OrdenarPor.ToLower() switch
            {
                "titulo" => paginacao.OrdenarDescendente
                    ? query.OrderByDescending(c => c.Titulo)
                    : query.OrderBy(c => c.Titulo),
                "datacriacao" => paginacao.OrdenarDescendente
                    ? query.OrderByDescending(c => c.DataCriacao)
                    : query.OrderBy(c => c.DataCriacao),
                "area" => paginacao.OrdenarDescendente
                    ? query.OrderByDescending(c => c.Area)
                    : query.OrderBy(c => c.Area),
                "nivel" => paginacao.OrdenarDescendente
                    ? query.OrderByDescending(c => c.Nivel)
                    : query.OrderBy(c => c.Nivel),
                _ => query.OrderBy(c => c.Titulo)
            };
        }
        else
        {
            query = query.OrderBy(c => c.Titulo);
        }

        var totalItens = query.Count();
        var cursosPaginados = query
            .Skip(paginacao.Skip)
            .Take(paginacao.Take)
            .ToList();

        var responses = cursosPaginados.Select(c => MapToResponse(c)).ToList();
        var paginacaoResponse = new PaginacaoResponse<CursoResponse>(
            responses,
            paginacao.Pagina,
            paginacao.TamanhoPagina,
            totalItens);

        // HATEOAS
        var queryParams = new Dictionary<string, string>
        {
            { "titulo", titulo ?? string.Empty },
            { "area", area ?? string.Empty },
            { "nivel", nivel ?? string.Empty },
            { "ordenarPor", paginacao.OrdenarPor ?? string.Empty },
            { "ordenarDescendente", paginacao.OrdenarDescendente.ToString() }
        };

        var links = HateoasHelper.CreatePaginationLinks(
            $"{Request.Scheme}://{Request.Host}/api/cursos/search",
            paginacao.Pagina,
            paginacaoResponse.TotalPaginas,
            queryParams);

        return Ok(new
        {
            data = paginacaoResponse,
            links
        });
    }

    private CursoResponse MapToResponse(Curso curso)
    {
        return new CursoResponse
        {
            Id = curso.Id,
            Titulo = curso.Titulo,
            Descricao = curso.Descricao,
            Area = curso.Area,
            Nivel = curso.Nivel,
            DataCriacao = curso.DataCriacao,
            TotalAulas = curso.Aulas?.Count ?? 0,
            TotalQuizzes = curso.Quizzes?.Count ?? 0,
            TotalCertificadosEmitidos = curso.Certificados?.Count ?? 0
        };
    }
}

