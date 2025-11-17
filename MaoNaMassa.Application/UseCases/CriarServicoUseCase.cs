using MaoNaMassa.Application.DTOs;
using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Domain.Exceptions;
using MaoNaMassa.Domain.Interfaces;

namespace MaoNaMassa.Application.UseCases;

/// <summary>
/// Caso de Uso: Criar Serviço
/// Permite que um profissional crie um novo serviço para oferecer.
/// </summary>
public class CriarServicoUseCase
{
    private readonly IServicoRepository _servicoRepository;
    private readonly IProfissionalRepository _profissionalRepository;
    private readonly IAvaliacaoRepository _avaliacaoRepository;

    public CriarServicoUseCase(
        IServicoRepository servicoRepository,
        IProfissionalRepository profissionalRepository,
        IAvaliacaoRepository avaliacaoRepository)
    {
        _servicoRepository = servicoRepository;
        _profissionalRepository = profissionalRepository;
        _avaliacaoRepository = avaliacaoRepository;
    }

    /// <summary>
    /// Executa o caso de uso: Criar serviço
    /// </summary>
    public async Task<ServicoDTO> ExecuteAsync(CreateServicoDTO dto)
    {
        // 1. Validar profissional
        var profissional = await _profissionalRepository.GetByIdAsync(dto.ProfissionalId);
        if (profissional == null)
            throw new EntityNotFoundException("Profissional", dto.ProfissionalId);

        // 2. Regra de negócio: Profissional deve estar disponível
        if (!profissional.Disponivel)
            throw new DomainException("Profissional não está disponível para oferecer novos serviços.");

        // 3. Criar serviço
        var servico = new Servico(
            dto.ProfissionalId,
            dto.Titulo,
            dto.Descricao,
            dto.Cidade,
            dto.Preco);

        var servicoCriado = await _servicoRepository.CreateAsync(servico);

        // 4. Mapear para DTO
        var avaliacaoMedia = await _avaliacaoRepository.GetAvaliacaoMediaByServicoIdAsync(servicoCriado.Id);

        return new ServicoDTO
        {
            Id = servicoCriado.Id,
            ProfissionalId = servicoCriado.ProfissionalId,
            NomeProfissional = profissional.Usuario?.Nome ?? string.Empty,
            Titulo = servicoCriado.Titulo,
            Descricao = servicoCriado.Descricao,
            Cidade = servicoCriado.Cidade,
            Preco = servicoCriado.Preco,
            DataPublicacao = servicoCriado.DataPublicacao,
            AvaliacaoMedia = avaliacaoMedia,
            TotalAvaliacoes = 0
        };
    }
}

