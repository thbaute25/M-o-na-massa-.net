using MaoNaMassa.API.Helpers;
using MaoNaMassa.Application.DTOs;
using MaoNaMassa.Application.DTOs.Input;
using MaoNaMassa.Application.DTOs.Output;
using MaoNaMassa.Application.DTOs.Paginacao;
using MaoNaMassa.Application.UseCases;
using MaoNaMassa.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaoNaMassa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfissionaisController : ControllerBase
{
    private readonly IProfissionalRepository _repository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly BuscarProfissionaisUseCase _buscarProfissionaisUseCase;
    private readonly CriarPerfilProfissionalUseCase _criarPerfilProfissionalUseCase;

    public ProfissionaisController(
        IProfissionalRepository repository,
        IUsuarioRepository usuarioRepository,
        BuscarProfissionaisUseCase buscarProfissionaisUseCase,
        CriarPerfilProfissionalUseCase criarPerfilProfissionalUseCase)
    {
        _repository = repository;
        _usuarioRepository = usuarioRepository;
        _buscarProfissionaisUseCase = buscarProfissionaisUseCase;
        _criarPerfilProfissionalUseCase = criarPerfilProfissionalUseCase;
    }

    /// <summary>
    /// Busca todos os profissionais com paginação
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PaginacaoResponse<ProfissionalResponse>>> GetAll(
        [FromQuery] PaginacaoRequest paginacao)
    {
        var profissionais = await _repository.GetAllAsync();
        var query = profissionais.AsQueryable();

        // Ordenação
        if (!string.IsNullOrWhiteSpace(paginacao.OrdenarPor))
        {
            query = paginacao.OrdenarPor.ToLower() switch
            {
                "avaliacao" => paginacao.OrdenarDescendente
                    ? query.OrderByDescending(p => p.AvaliacaoMedia ?? 0)
                    : query.OrderBy(p => p.AvaliacaoMedia ?? 0),
                _ => query.OrderByDescending(p => p.AvaliacaoMedia ?? 0)
            };
        }
        else
        {
            query = query.OrderByDescending(p => p.AvaliacaoMedia ?? 0);
        }

        var totalItens = query.Count();
        var profissionaisPaginados = query
            .Skip(paginacao.Skip)
            .Take(paginacao.Take)
            .ToList();

        var responses = new List<ProfissionalResponse>();
        foreach (var profissional in profissionaisPaginados)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(profissional.UsuarioId);
            responses.Add(MapToResponse(profissional, usuario));
        }

        var paginacaoResponse = new PaginacaoResponse<ProfissionalResponse>(
            responses,
            paginacao.Pagina,
            paginacao.TamanhoPagina,
            totalItens);

        // HATEOAS
        var queryParams = new Dictionary<string, string>
        {
            { "ordenarPor", paginacao.OrdenarPor ?? string.Empty },
            { "ordenarDescendente", paginacao.OrdenarDescendente.ToString() }
        };

        var links = HateoasHelper.CreatePaginationLinks(
            $"{Request.Scheme}://{Request.Host}/api/profissionais",
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
    /// Busca um profissional por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProfissionalResponse>> GetById(Guid id)
    {
        var profissional = await _repository.GetByIdAsync(id);
        if (profissional == null)
        {
            return NotFound();
        }

        var usuario = await _usuarioRepository.GetByIdAsync(profissional.UsuarioId);
        var response = MapToResponse(profissional, usuario);
        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/profissionais",
            id);

        return Ok(new
        {
            data = response,
            links
        });
    }

    /// <summary>
    /// Cria um novo perfil profissional
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ProfissionalResponse>> Create([FromBody] CriarPerfilProfissionalRequest request, [FromQuery] Guid usuarioId)
    {
        var createDto = new CreateProfissionalDTO
        {
            UsuarioId = usuarioId,
            Descricao = request.Descricao
        };

        var profissionalDto = await _criarPerfilProfissionalUseCase.ExecuteAsync(createDto);
        var profissional = await _repository.GetByIdAsync(profissionalDto.Id);
        var usuario = await _usuarioRepository.GetByIdAsync(profissionalDto.UsuarioId);

        var response = MapToResponse(profissional!, usuario);
        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/profissionais",
            profissionalDto.Id);

        return CreatedAtAction(
            nameof(GetById),
            new { id = profissionalDto.Id },
            new { data = response, links });
    }

    /// <summary>
    /// Busca profissionais com filtros e paginação
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<PaginacaoResponse<ProfissionalResponse>>> Search(
        [FromQuery] PaginacaoRequest paginacao,
        [FromQuery] string? cidade = null,
        [FromQuery] string? areaInteresse = null,
        [FromQuery] decimal? avaliacaoMinima = null,
        [FromQuery] bool apenasDisponiveis = true)
    {
        var profissionaisDto = await _buscarProfissionaisUseCase.ExecuteAsync(
            cidade,
            areaInteresse,
            avaliacaoMinima,
            apenasDisponiveis);

        var profissionaisList = profissionaisDto.ToList();

        // Aplicar ordenação adicional se necessário
        if (!string.IsNullOrWhiteSpace(paginacao.OrdenarPor))
        {
            profissionaisList = paginacao.OrdenarPor.ToLower() switch
            {
                "avaliacao" => paginacao.OrdenarDescendente
                    ? profissionaisList.OrderByDescending(p => p.AvaliacaoMedia ?? 0).ToList()
                    : profissionaisList.OrderBy(p => p.AvaliacaoMedia ?? 0).ToList(),
                "totalavaliacoes" => paginacao.OrdenarDescendente
                    ? profissionaisList.OrderByDescending(p => p.TotalAvaliacoes).ToList()
                    : profissionaisList.OrderBy(p => p.TotalAvaliacoes).ToList(),
                _ => profissionaisList
            };
        }

        var totalItens = profissionaisList.Count;
        var profissionaisPaginados = profissionaisList
            .Skip(paginacao.Skip)
            .Take(paginacao.Take)
            .ToList();

        var responses = profissionaisPaginados.Select(p => MapToResponse(p)).ToList();
        var paginacaoResponse = new PaginacaoResponse<ProfissionalResponse>(
            responses,
            paginacao.Pagina,
            paginacao.TamanhoPagina,
            totalItens);

        // HATEOAS
        var queryParams = new Dictionary<string, string>
        {
            { "cidade", cidade ?? string.Empty },
            { "areaInteresse", areaInteresse ?? string.Empty },
            { "avaliacaoMinima", avaliacaoMinima?.ToString() ?? string.Empty },
            { "apenasDisponiveis", apenasDisponiveis.ToString() },
            { "ordenarPor", paginacao.OrdenarPor ?? string.Empty },
            { "ordenarDescendente", paginacao.OrdenarDescendente.ToString() }
        };

        var links = HateoasHelper.CreatePaginationLinks(
            $"{Request.Scheme}://{Request.Host}/api/profissionais/search",
            paginacao.Pagina,
            paginacaoResponse.TotalPaginas,
            queryParams);

        return Ok(new
        {
            data = paginacaoResponse,
            links
        });
    }

    private ProfissionalResponse MapToResponse(Domain.Entities.Profissional profissional, Domain.Entities.Usuario? usuario)
    {
        return new ProfissionalResponse
        {
            Id = profissional.Id,
            UsuarioId = profissional.UsuarioId,
            NomeUsuario = usuario?.Nome ?? string.Empty,
            EmailUsuario = usuario?.Email ?? string.Empty,
            CidadeUsuario = usuario?.Cidade ?? string.Empty,
            AreaInteresse = usuario?.AreaInteresse ?? string.Empty,
            Descricao = profissional.Descricao,
            AvaliacaoMedia = profissional.AvaliacaoMedia,
            Disponivel = profissional.Disponivel,
            TotalServicos = profissional.Servicos?.Count ?? 0,
            TotalAvaliacoes = profissional.Servicos?.SelectMany(s => s.Avaliacoes).Count() ?? 0
        };
    }

    private ProfissionalResponse MapToResponse(ProfissionalDTO profissionalDto)
    {
        return new ProfissionalResponse
        {
            Id = profissionalDto.Id,
            UsuarioId = profissionalDto.UsuarioId,
            NomeUsuario = profissionalDto.NomeUsuario,
            EmailUsuario = profissionalDto.EmailUsuario,
            CidadeUsuario = profissionalDto.CidadeUsuario,
            AreaInteresse = string.Empty,
            Descricao = profissionalDto.Descricao,
            AvaliacaoMedia = profissionalDto.AvaliacaoMedia,
            Disponivel = profissionalDto.Disponivel,
            TotalServicos = profissionalDto.TotalServicos,
            TotalAvaliacoes = profissionalDto.TotalAvaliacoes
        };
    }
}

