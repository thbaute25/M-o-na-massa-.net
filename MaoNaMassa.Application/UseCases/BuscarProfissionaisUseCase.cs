using MaoNaMassa.Application.DTOs;
using MaoNaMassa.Domain.Interfaces;

namespace MaoNaMassa.Application.UseCases;

/// <summary>
/// Caso de Uso: Buscar Profissionais
/// Permite buscar profissionais disponíveis com filtros específicos.
/// </summary>
public class BuscarProfissionaisUseCase
{
    private readonly IProfissionalRepository _profissionalRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IServicoRepository _servicoRepository;
    private readonly IAvaliacaoRepository _avaliacaoRepository;

    public BuscarProfissionaisUseCase(
        IProfissionalRepository profissionalRepository,
        IUsuarioRepository usuarioRepository,
        IServicoRepository servicoRepository,
        IAvaliacaoRepository avaliacaoRepository)
    {
        _profissionalRepository = profissionalRepository;
        _usuarioRepository = usuarioRepository;
        _servicoRepository = servicoRepository;
        _avaliacaoRepository = avaliacaoRepository;
    }

    /// <summary>
    /// Executa o caso de uso: Buscar profissionais com filtros
    /// </summary>
    public async Task<IEnumerable<ProfissionalDTO>> ExecuteAsync(
        string? cidade = null,
        string? areaInteresse = null,
        decimal? avaliacaoMinima = null,
        bool apenasDisponiveis = true)
    {
        // 1. Buscar profissionais baseado nos filtros
        IEnumerable<Domain.Entities.Profissional> profissionais;

        if (apenasDisponiveis)
        {
            profissionais = await _profissionalRepository.GetDisponiveisAsync();
        }
        else
        {
            profissionais = await _profissionalRepository.GetAllAsync();
        }

        // 2. Aplicar filtros adicionais
        var profissionaisFiltrados = new List<Domain.Entities.Profissional>();

        foreach (var profissional in profissionais)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(profissional.UsuarioId);
            if (usuario == null) continue;

            // Filtro por cidade
            if (!string.IsNullOrWhiteSpace(cidade) && 
                !usuario.Cidade.Equals(cidade, StringComparison.OrdinalIgnoreCase))
                continue;

            // Filtro por área de interesse
            if (!string.IsNullOrWhiteSpace(areaInteresse) && 
                !usuario.AreaInteresse.Equals(areaInteresse, StringComparison.OrdinalIgnoreCase))
                continue;

            // Filtro por avaliação mínima
            if (avaliacaoMinima.HasValue)
            {
                profissional.RecalcularAvaliacaoMedia();
                if (!profissional.AvaliacaoMedia.HasValue || 
                    profissional.AvaliacaoMedia.Value < avaliacaoMinima.Value)
                    continue;
            }

            profissionaisFiltrados.Add(profissional);
        }

        // 3. Mapear para DTOs
        var dtos = new List<ProfissionalDTO>();
        foreach (var profissional in profissionaisFiltrados)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(profissional.UsuarioId);
            var servicos = await _servicoRepository.GetByProfissionalIdAsync(profissional.Id);
            var totalAvaliacoes = servicos.SelectMany(s => s.Avaliacoes).Count();

            dtos.Add(new ProfissionalDTO
            {
                Id = profissional.Id,
                UsuarioId = profissional.UsuarioId,
                NomeUsuario = usuario?.Nome ?? string.Empty,
                EmailUsuario = usuario?.Email ?? string.Empty,
                CidadeUsuario = usuario?.Cidade ?? string.Empty,
                Descricao = profissional.Descricao,
                AvaliacaoMedia = profissional.AvaliacaoMedia,
                Disponivel = profissional.Disponivel,
                TotalServicos = servicos.Count(),
                TotalAvaliacoes = totalAvaliacoes
            });
        }

        // 4. Ordenar por avaliação média (maior primeiro) e depois por total de avaliações
        return dtos.OrderByDescending(p => p.AvaliacaoMedia ?? 0)
                   .ThenByDescending(p => p.TotalAvaliacoes);
    }
}

