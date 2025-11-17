using MaoNaMassa.Application.DTOs;
using MaoNaMassa.Domain.Interfaces;

namespace MaoNaMassa.Application.UseCases;

/// <summary>
/// Caso de Uso: Buscar Serviços
/// Permite buscar serviços oferecidos pelos profissionais com diversos filtros.
/// </summary>
public class BuscarServicosUseCase
{
    private readonly IServicoRepository _servicoRepository;
    private readonly IProfissionalRepository _profissionalRepository;
    private readonly IAvaliacaoRepository _avaliacaoRepository;

    public BuscarServicosUseCase(
        IServicoRepository servicoRepository,
        IProfissionalRepository profissionalRepository,
        IAvaliacaoRepository avaliacaoRepository)
    {
        _servicoRepository = servicoRepository;
        _profissionalRepository = profissionalRepository;
        _avaliacaoRepository = avaliacaoRepository;
    }

    /// <summary>
    /// Executa o caso de uso: Buscar serviços com filtros
    /// </summary>
    public async Task<IEnumerable<ServicoDTO>> ExecuteAsync(BuscarServicoDTO filtros)
    {
        // 1. Buscar serviços baseado nos filtros básicos
        var servicos = await _servicoRepository.SearchAsync(filtros.Cidade, filtros.Termo);
        var dtos = new List<ServicoDTO>();

        // 2. Mapear para DTOs e aplicar filtros adicionais
        foreach (var servico in servicos)
        {
            var profissional = await _profissionalRepository.GetByIdAsync(servico.ProfissionalId);
            if (profissional == null) continue;

            // Verificar se profissional está disponível
            if (!profissional.Disponivel) continue;

            var avaliacaoMedia = await _avaliacaoRepository.GetAvaliacaoMediaByServicoIdAsync(servico.Id);
            var totalAvaliacoes = servico.Avaliacoes.Count;

            // Aplicar filtros adicionais
            if (filtros.PrecoMaximo.HasValue && servico.Preco.HasValue && 
                servico.Preco > filtros.PrecoMaximo)
                continue;

            if (filtros.AvaliacaoMinima.HasValue && 
                (!avaliacaoMedia.HasValue || avaliacaoMedia < filtros.AvaliacaoMinima))
                continue;

            dtos.Add(new ServicoDTO
            {
                Id = servico.Id,
                ProfissionalId = servico.ProfissionalId,
                NomeProfissional = profissional.Usuario?.Nome ?? string.Empty,
                Titulo = servico.Titulo,
                Descricao = servico.Descricao,
                Cidade = servico.Cidade,
                Preco = servico.Preco,
                DataPublicacao = servico.DataPublicacao,
                AvaliacaoMedia = avaliacaoMedia,
                TotalAvaliacoes = totalAvaliacoes
            });
        }

        // 3. Ordenar resultados
        return dtos.OrderByDescending(s => s.AvaliacaoMedia ?? 0)
                   .ThenByDescending(s => s.TotalAvaliacoes)
                   .ThenByDescending(s => s.DataPublicacao);
    }
}

