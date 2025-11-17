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
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioRepository _repository;
    private readonly IProfissionalRepository _profissionalRepository;
    private readonly ICertificadoRepository _certificadoRepository;

    public UsuariosController(
        IUsuarioRepository repository,
        IProfissionalRepository profissionalRepository,
        ICertificadoRepository certificadoRepository)
    {
        _repository = repository;
        _profissionalRepository = profissionalRepository;
        _certificadoRepository = certificadoRepository;
    }

    /// <summary>
    /// Busca todos os usuários com paginação
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PaginacaoResponse<UsuarioResponse>>> GetAll(
        [FromQuery] PaginacaoRequest paginacao,
        [FromQuery] string? nome = null,
        [FromQuery] string? cidade = null,
        [FromQuery] string? tipoUsuario = null)
    {
        var usuarios = await _repository.GetAllAsync();
        var query = usuarios.AsQueryable();

        // Filtros
        if (!string.IsNullOrWhiteSpace(nome))
        {
            query = query.Where(u => u.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(cidade))
        {
            query = query.Where(u => u.Cidade.Contains(cidade, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(tipoUsuario))
        {
            query = query.Where(u => u.TipoUsuario == tipoUsuario);
        }

        // Ordenação
        if (!string.IsNullOrWhiteSpace(paginacao.OrdenarPor))
        {
            query = paginacao.OrdenarPor.ToLower() switch
            {
                "nome" => paginacao.OrdenarDescendente 
                    ? query.OrderByDescending(u => u.Nome)
                    : query.OrderBy(u => u.Nome),
                "datacriacao" => paginacao.OrdenarDescendente
                    ? query.OrderByDescending(u => u.DataCriacao)
                    : query.OrderBy(u => u.DataCriacao),
                _ => query.OrderBy(u => u.Nome)
            };
        }
        else
        {
            query = query.OrderBy(u => u.Nome);
        }

        var totalItens = query.Count();
        var usuariosPaginados = query
            .Skip(paginacao.Skip)
            .Take(paginacao.Take)
            .ToList();

        var responses = usuariosPaginados.Select(u => MapToResponse(u)).ToList();
        var paginacaoResponse = new PaginacaoResponse<UsuarioResponse>(
            responses,
            paginacao.Pagina,
            paginacao.TamanhoPagina,
            totalItens);

        // HATEOAS
        var queryParams = new Dictionary<string, string>
        {
            { "nome", nome ?? string.Empty },
            { "cidade", cidade ?? string.Empty },
            { "tipoUsuario", tipoUsuario ?? string.Empty },
            { "ordenarPor", paginacao.OrdenarPor ?? string.Empty },
            { "ordenarDescendente", paginacao.OrdenarDescendente.ToString() }
        };

        var links = HateoasHelper.CreatePaginationLinks(
            $"{Request.Scheme}://{Request.Host}/api/usuarios",
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
    /// Busca um usuário por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioResponse>> GetById(Guid id)
    {
        var usuario = await _repository.GetByIdAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }

        var response = MapToResponse(usuario);
        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/usuarios",
            id);

        return Ok(new
        {
            data = response,
            links
        });
    }

    /// <summary>
    /// Cria um novo usuário
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<UsuarioResponse>> Create([FromBody] CriarUsuarioRequest request)
    {
        var usuario = new Usuario(
            request.Nome,
            request.Email,
            request.Senha,
            request.Cidade,
            request.AreaInteresse,
            request.TipoUsuario);

        var usuarioCriado = await _repository.CreateAsync(usuario);
        var response = MapToResponse(usuarioCriado);

        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/usuarios",
            usuarioCriado.Id);

        return CreatedAtAction(
            nameof(GetById),
            new { id = usuarioCriado.Id },
            new { data = response, links });
    }

    /// <summary>
    /// Atualiza um usuário
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<UsuarioResponse>> Update(Guid id, [FromBody] AtualizarUsuarioRequest request)
    {
        var usuario = await _repository.GetByIdAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }

        usuario.SetNome(request.Nome);
        usuario.SetCidade(request.Cidade);
        usuario.SetAreaInteresse(request.AreaInteresse);

        await _repository.UpdateAsync(usuario);

        var response = MapToResponse(usuario);
        var links = HateoasHelper.CreateResourceLinks(
            $"{Request.Scheme}://{Request.Host}/api/usuarios",
            id);

        return Ok(new { data = response, links });
    }

    /// <summary>
    /// Remove um usuário
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var usuario = await _repository.GetByIdAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Busca usuários com filtros e paginação
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<PaginacaoResponse<UsuarioResponse>>> Search(
        [FromQuery] PaginacaoRequest paginacao,
        [FromQuery] string? nome = null,
        [FromQuery] string? cidade = null,
        [FromQuery] string? tipoUsuario = null,
        [FromQuery] string? areaInteresse = null)
    {
        var usuarios = await _repository.GetAllAsync();
        var query = usuarios.AsQueryable();

        // Filtros
        if (!string.IsNullOrWhiteSpace(nome))
        {
            query = query.Where(u => u.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(cidade))
        {
            query = query.Where(u => u.Cidade.Contains(cidade, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(tipoUsuario))
        {
            query = query.Where(u => u.TipoUsuario == tipoUsuario);
        }

        if (!string.IsNullOrWhiteSpace(areaInteresse))
        {
            query = query.Where(u => u.AreaInteresse.Contains(areaInteresse, StringComparison.OrdinalIgnoreCase));
        }

        // Ordenação
        if (!string.IsNullOrWhiteSpace(paginacao.OrdenarPor))
        {
            query = paginacao.OrdenarPor.ToLower() switch
            {
                "nome" => paginacao.OrdenarDescendente
                    ? query.OrderByDescending(u => u.Nome)
                    : query.OrderBy(u => u.Nome),
                "datacriacao" => paginacao.OrdenarDescendente
                    ? query.OrderByDescending(u => u.DataCriacao)
                    : query.OrderBy(u => u.DataCriacao),
                "cidade" => paginacao.OrdenarDescendente
                    ? query.OrderByDescending(u => u.Cidade)
                    : query.OrderBy(u => u.Cidade),
                _ => query.OrderBy(u => u.Nome)
            };
        }
        else
        {
            query = query.OrderBy(u => u.Nome);
        }

        var totalItens = query.Count();
        var usuariosPaginados = query
            .Skip(paginacao.Skip)
            .Take(paginacao.Take)
            .ToList();

        var responses = usuariosPaginados.Select(u => MapToResponse(u)).ToList();
        var paginacaoResponse = new PaginacaoResponse<UsuarioResponse>(
            responses,
            paginacao.Pagina,
            paginacao.TamanhoPagina,
            totalItens);

        // HATEOAS
        var queryParams = new Dictionary<string, string>
        {
            { "nome", nome ?? string.Empty },
            { "cidade", cidade ?? string.Empty },
            { "tipoUsuario", tipoUsuario ?? string.Empty },
            { "areaInteresse", areaInteresse ?? string.Empty },
            { "ordenarPor", paginacao.OrdenarPor ?? string.Empty },
            { "ordenarDescendente", paginacao.OrdenarDescendente.ToString() }
        };

        var links = HateoasHelper.CreatePaginationLinks(
            $"{Request.Scheme}://{Request.Host}/api/usuarios/search",
            paginacao.Pagina,
            paginacaoResponse.TotalPaginas,
            queryParams);

        return Ok(new
        {
            data = paginacaoResponse,
            links
        });
    }

    private UsuarioResponse MapToResponse(Usuario usuario)
    {
        var temPerfilProfissional = usuario.Profissional != null;
        var totalCertificados = usuario.Certificados?.Count ?? 0;

        return new UsuarioResponse
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Cidade = usuario.Cidade,
            AreaInteresse = usuario.AreaInteresse,
            TipoUsuario = usuario.TipoUsuario,
            DataCriacao = usuario.DataCriacao,
            TemPerfilProfissional = temPerfilProfissional,
            TotalCertificados = totalCertificados
        };
    }
}

