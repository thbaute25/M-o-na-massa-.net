using MaoNaMassa.Application.DTOs;
using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Domain.Exceptions;
using MaoNaMassa.Domain.Interfaces;

namespace MaoNaMassa.Application.Services;

public class ServicoService : IServicoService
{
    private readonly IServicoRepository _servicoRepository;
    private readonly IProfissionalRepository _profissionalRepository;
    private readonly IAvaliacaoRepository _avaliacaoRepository;

    public ServicoService(
        IServicoRepository servicoRepository,
        IProfissionalRepository profissionalRepository,
        IAvaliacaoRepository avaliacaoRepository)
    {
        _servicoRepository = servicoRepository;
        _profissionalRepository = profissionalRepository;
        _avaliacaoRepository = avaliacaoRepository;
    }

    public async Task<ServicoDTO> CriarServicoAsync(CreateServicoDTO dto)
    {
        // Regra de Negócio 1: Validar se profissional existe
        var profissional = await _profissionalRepository.GetByIdAsync(dto.ProfissionalId);
        if (profissional == null)
            throw new EntityNotFoundException("Profissional", dto.ProfissionalId);

        // Regra de Negócio 2: Profissional deve estar disponível
        if (!profissional.Disponivel)
            throw new DomainException("Profissional não está disponível para oferecer novos serviços.");

        // Regra de Negócio 3: Criar serviço
        var servico = new Servico(
            dto.ProfissionalId,
            dto.Titulo,
            dto.Descricao,
            dto.Cidade,
            dto.Preco);

        var servicoCriado = await _servicoRepository.CreateAsync(servico);

        return await MapToDTOAsync(servicoCriado);
    }

    public async Task<ServicoDTO?> GetByIdAsync(Guid id)
    {
        var servico = await _servicoRepository.GetByIdAsync(id);
        if (servico == null)
            return null;

        return await MapToDTOAsync(servico);
    }

    public async Task<IEnumerable<ServicoDTO>> GetByProfissionalIdAsync(Guid profissionalId)
    {
        var servicos = await _servicoRepository.GetByProfissionalIdAsync(profissionalId);
        var dtos = new List<ServicoDTO>();

        foreach (var servico in servicos)
        {
            dtos.Add(await MapToDTOAsync(servico));
        }

        return dtos;
    }

    public async Task<IEnumerable<ServicoDTO>> BuscarServicosAsync(BuscarServicoDTO filtros)
    {
        // Regra de Negócio: Buscar serviços com filtros
        var servicos = await _servicoRepository.SearchAsync(filtros.Cidade, filtros.Termo);
        var dtos = new List<ServicoDTO>();

        foreach (var servico in servicos)
        {
            var dto = await MapToDTOAsync(servico);

            // Aplicar filtros adicionais
            if (filtros.PrecoMaximo.HasValue && dto.Preco.HasValue && dto.Preco > filtros.PrecoMaximo)
                continue;

            if (filtros.AvaliacaoMinima.HasValue && (!dto.AvaliacaoMedia.HasValue || dto.AvaliacaoMedia < filtros.AvaliacaoMinima))
                continue;

            dtos.Add(dto);
        }

        return dtos.OrderByDescending(s => s.AvaliacaoMedia ?? 0)
                   .ThenByDescending(s => s.DataPublicacao);
    }

    private async Task<ServicoDTO> MapToDTOAsync(Servico servico)
    {
        var profissional = await _profissionalRepository.GetByIdAsync(servico.ProfissionalId);
        var avaliacaoMedia = await _avaliacaoRepository.GetAvaliacaoMediaByServicoIdAsync(servico.Id);
        var totalAvaliacoes = servico.Avaliacoes.Count;

        return new ServicoDTO
        {
            Id = servico.Id,
            ProfissionalId = servico.ProfissionalId,
            NomeProfissional = profissional?.Usuario?.Nome ?? string.Empty,
            Titulo = servico.Titulo,
            Descricao = servico.Descricao,
            Cidade = servico.Cidade,
            Preco = servico.Preco,
            DataPublicacao = servico.DataPublicacao,
            AvaliacaoMedia = avaliacaoMedia,
            TotalAvaliacoes = totalAvaliacoes
        };
    }
}

