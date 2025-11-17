using MaoNaMassa.API.Helpers;
using MaoNaMassa.Application.DTOs;
using MaoNaMassa.Application.DTOs.Input;
using MaoNaMassa.Application.DTOs.Output;
using MaoNaMassa.Application.DTOs.Paginacao;
using MaoNaMassa.Application.UseCases;
using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MaoNaMassa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicosController : ControllerBase
{
    private readonly IServicoRepository _repository;
    private readonly IProfissionalRepository _profissionalRepository;
    private readonly IAvaliacaoRepository _avaliacaoRepository;
    private readonly BuscarServicosUseCase _buscarServicosUseCase;
    private readonly CriarServicoUseCase _criarServicoUseCase;

    public ServicosController(
        IServicoRepository repository,
        IProfissionalRepository profissionalRepository,
        IAvaliacaoRepository avaliacaoRepository,
        BuscarServicosUseCase buscarServicosUseCase,
        CriarServicoUseCase criarServicoUseCase)
    {
        _repository = repository;
        _profissionalRepository = profissionalRepository;
        _avaliacaoRepository = avaliacaoRepository;
        _buscarServicosUseCase = buscarServicosUseCase;
        _criarServicoUseCase = criarServicoUseCase;
    }

    /// <summary>
    /// Busca todos os serviços com paginação
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PaginacaoResponse<ServicoResponse>>> GetAll(
        [FromQuery] PaginacaoRequest paginacao)
    {
        var servicos = await _repository.GetAllAsync();
        var query = servicos.AsQueryable();

        // Ordenação
        if (!string.IsNullOrWhiteSpace(paginacao.OrdenarPor))
        {
            query = paginacao.OrdenarPor.ToLower() switch
            {
                "titulo" => paginacao.OrdenarDescendente
                    ? query.OrderByDescending(s => s.Titulo)
                    : query.OrderBy(s => s.Titulo),
                "datapublicacao" => paginacao.OrdenarDescendente
                    ? query.OrderByDescending(s => s.DataPublicacao)
                    : query.OrderBy(s => s.DataPublicacao),
                "preco" => paginacao.OrdenarDescendente
                    ? query.OrderByDescending(s => s.Preco ?? 0)
                    : query.OrderBy(s => s.Preco ?? 0),
                _ => query.OrderByDescending(s => s.DataPublicacao)
            };
        }
        else
        {
            query = query.OrderByDescending(s => s.DataPublicacao);
        }

        var totalItens = query.Count();
        var servicosPaginados = query
            .Skip(paginacao.Skip)
            .Take(paginacao.Take)
            .ToList();

        var responses = new List<ServicoResponse>();
        foreach (var servico in servicosPaginados)
        {
            var profissional = await _profissionalRepository.GetByIdAsync(servico.ProfissionalId);
            var avaliacaoMedia = await _avaliacaoRepository.GetAvaliacaoMediaByServicoIdAsync(servico.Id);
            var totalAvaliacoes = servico.Avaliacoes?.Count ?? 0;

            responses.Add(MapToResponse(servico, profissional, avaliacaoMedia, totalAvaliacoes));
        }

        var paginacaoResponse = new PaginacaoResponse<ServicoResponse>(
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
            $"{Request.Scheme}://{Request.Host}/api/servicos",
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
    /// Busca um serviço por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ServicoResponse>> GetById(Guid id)
    {
        var servico = await _repository.GetByIdAsync(id);
        if (servico == null)
        {
            return NotFound();
        }

        var profissional = await _profissionalRepository.GetByIdAsync(servico.ProfissionalId);
        var avaliacaoMedia = await _avaliacaoRepository.GetAvaliacaoMediaByServicoIdAsync(servico.Id);
        var totalAvaliacoes = servico.Avaliacoes?.Count ?? 0;

        var response = MapToResponse(servico, profissional, avaliacaoMedia, totalAvaliacoes);
        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/servicos",
            id);

        return Ok(new
        {
            data = response,
            links
        });
    }

    /// <summary>
    /// Cria um novo serviço
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ServicoResponse>> Create([FromBody] CriarServicoRequest request, [FromQuery] Guid profissionalId)
    {
        var createDto = new CreateServicoDTO
        {
            ProfissionalId = profissionalId,
            Titulo = request.Titulo,
            Descricao = request.Descricao,
            Cidade = request.Cidade,
            Preco = request.Preco
        };

        var servicoDto = await _criarServicoUseCase.ExecuteAsync(createDto);
        var servico = await _repository.GetByIdAsync(servicoDto.Id);

        var response = MapToResponse(servico!, null, servicoDto.AvaliacaoMedia, servicoDto.TotalAvaliacoes);
        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/servicos",
            servicoDto.Id);

        return CreatedAtAction(
            nameof(GetById),
            new { id = servicoDto.Id },
            new { data = response, links });
    }

    /// <summary>
    /// Atualiza um serviço
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ServicoResponse>> Update(Guid id, [FromBody] CriarServicoRequest request)
    {
        var servico = await _repository.GetByIdAsync(id);
        if (servico == null)
        {
            return NotFound();
        }

        servico.SetTitulo(request.Titulo);
        servico.SetDescricao(request.Descricao);
        servico.SetCidade(request.Cidade);
        if (request.Preco.HasValue)
        {
            servico.SetPreco(request.Preco.Value);
        }

        await _repository.UpdateAsync(servico);

        var profissional = await _profissionalRepository.GetByIdAsync(servico.ProfissionalId);
        var avaliacaoMedia = await _avaliacaoRepository.GetAvaliacaoMediaByServicoIdAsync(servico.Id);
        var totalAvaliacoes = servico.Avaliacoes?.Count ?? 0;

        var response = MapToResponse(servico, profissional, avaliacaoMedia, totalAvaliacoes);
        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/servicos",
            id);

        return Ok(new { data = response, links });
    }

    /// <summary>
    /// Remove um serviço
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var servico = await _repository.GetByIdAsync(id);
        if (servico == null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Busca serviços com filtros, paginação e ordenação
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<PaginacaoResponse<ServicoResponse>>> Search(
        [FromQuery] PaginacaoRequest paginacao,
        [FromQuery] string? cidade = null,
        [FromQuery] string? termo = null,
        [FromQuery] decimal? precoMaximo = null,
        [FromQuery] decimal? avaliacaoMinima = null)
    {
        var filtros = new BuscarServicoDTO
        {
            Cidade = cidade,
            Termo = termo,
            PrecoMaximo = precoMaximo,
            AvaliacaoMinima = avaliacaoMinima
        };

        var servicosDto = await _buscarServicosUseCase.ExecuteAsync(filtros);
        var servicosList = servicosDto.ToList();

        // Aplicar ordenação adicional se necessário
        if (!string.IsNullOrWhiteSpace(paginacao.OrdenarPor))
        {
            servicosList = paginacao.OrdenarPor.ToLower() switch
            {
                "titulo" => paginacao.OrdenarDescendente
                    ? servicosList.OrderByDescending(s => s.Titulo).ToList()
                    : servicosList.OrderBy(s => s.Titulo).ToList(),
                "datapublicacao" => paginacao.OrdenarDescendente
                    ? servicosList.OrderByDescending(s => s.DataPublicacao).ToList()
                    : servicosList.OrderBy(s => s.DataPublicacao).ToList(),
                "preco" => paginacao.OrdenarDescendente
                    ? servicosList.OrderByDescending(s => s.Preco ?? 0).ToList()
                    : servicosList.OrderBy(s => s.Preco ?? 0).ToList(),
                "avaliacao" => paginacao.OrdenarDescendente
                    ? servicosList.OrderByDescending(s => s.AvaliacaoMedia ?? 0).ToList()
                    : servicosList.OrderBy(s => s.AvaliacaoMedia ?? 0).ToList(),
                _ => servicosList
            };
        }

        var totalItens = servicosList.Count;
        var servicosPaginados = servicosList
            .Skip(paginacao.Skip)
            .Take(paginacao.Take)
            .ToList();

        var responses = servicosPaginados.Select(s => MapToResponse(s)).ToList();
        var paginacaoResponse = new PaginacaoResponse<ServicoResponse>(
            responses,
            paginacao.Pagina,
            paginacao.TamanhoPagina,
            totalItens);

        // HATEOAS
        var queryParams = new Dictionary<string, string>
        {
            { "cidade", cidade ?? string.Empty },
            { "termo", termo ?? string.Empty },
            { "precoMaximo", precoMaximo?.ToString() ?? string.Empty },
            { "avaliacaoMinima", avaliacaoMinima?.ToString() ?? string.Empty },
            { "ordenarPor", paginacao.OrdenarPor ?? string.Empty },
            { "ordenarDescendente", paginacao.OrdenarDescendente.ToString() }
        };

        var links = HateoasHelper.CreatePaginationLinks(
            $"{Request.Scheme}://{Request.Host}/api/servicos/search",
            paginacao.Pagina,
            paginacaoResponse.TotalPaginas,
            queryParams);

        return Ok(new
        {
            data = paginacaoResponse,
            links
        });
    }

    private ServicoResponse MapToResponse(Servico servico, Profissional? profissional, decimal? avaliacaoMedia, int totalAvaliacoes)
    {
        return new ServicoResponse
        {
            Id = servico.Id,
            ProfissionalId = servico.ProfissionalId,
            NomeProfissional = profissional?.Usuario?.Nome ?? string.Empty,
            CidadeProfissional = profissional?.Usuario?.Cidade ?? string.Empty,
            Titulo = servico.Titulo,
            Descricao = servico.Descricao,
            Cidade = servico.Cidade,
            Preco = servico.Preco,
            DataPublicacao = servico.DataPublicacao,
            AvaliacaoMedia = avaliacaoMedia,
            TotalAvaliacoes = totalAvaliacoes
        };
    }

    private ServicoResponse MapToResponse(ServicoDTO servicoDto)
    {
        return new ServicoResponse
        {
            Id = servicoDto.Id,
            ProfissionalId = servicoDto.ProfissionalId,
            NomeProfissional = servicoDto.NomeProfissional,
            CidadeProfissional = string.Empty,
            Titulo = servicoDto.Titulo,
            Descricao = servicoDto.Descricao,
            Cidade = servicoDto.Cidade,
            Preco = servicoDto.Preco,
            DataPublicacao = servicoDto.DataPublicacao,
            AvaliacaoMedia = servicoDto.AvaliacaoMedia,
            TotalAvaliacoes = servicoDto.TotalAvaliacoes
        };
    }
}

