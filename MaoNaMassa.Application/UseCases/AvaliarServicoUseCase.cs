using MaoNaMassa.Application.DTOs;
using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Domain.Exceptions;
using MaoNaMassa.Domain.Interfaces;

namespace MaoNaMassa.Application.UseCases;

/// <summary>
/// Caso de Uso: Avaliar Serviço
/// Permite que um usuário avalie um serviço prestado por um profissional.
/// </summary>
public class AvaliarServicoUseCase
{
    private readonly IAvaliacaoRepository _avaliacaoRepository;
    private readonly IServicoRepository _servicoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IProfissionalRepository _profissionalRepository;

    public AvaliarServicoUseCase(
        IAvaliacaoRepository avaliacaoRepository,
        IServicoRepository servicoRepository,
        IUsuarioRepository usuarioRepository,
        IProfissionalRepository profissionalRepository)
    {
        _avaliacaoRepository = avaliacaoRepository;
        _servicoRepository = servicoRepository;
        _usuarioRepository = usuarioRepository;
        _profissionalRepository = profissionalRepository;
    }

    /// <summary>
    /// Executa o caso de uso: Avaliar serviço
    /// </summary>
    public async Task<AvaliacaoDTO> ExecuteAsync(CreateAvaliacaoDTO dto)
    {
        // 1. Validar usuário
        var usuario = await _usuarioRepository.GetByIdAsync(dto.UsuarioId);
        if (usuario == null)
            throw new EntityNotFoundException("Usuário", dto.UsuarioId);

        // 2. Validar serviço
        var servico = await _servicoRepository.GetByIdAsync(dto.ServicoId);
        if (servico == null)
            throw new EntityNotFoundException("Serviço", dto.ServicoId);

        // 3. Validar profissional do serviço
        var profissional = await _profissionalRepository.GetByIdAsync(servico.ProfissionalId);
        if (profissional == null)
            throw new DomainException("Profissional do serviço não encontrado.");

        // 4. Regra de negócio: Usuário não pode avaliar seu próprio serviço
        if (profissional.UsuarioId == dto.UsuarioId)
            throw new DomainException("Usuário não pode avaliar seu próprio serviço.");

        // 5. Verificar se usuário já avaliou este serviço
        var jaAvaliou = await _avaliacaoRepository.UsuarioJaAvaliouServicoAsync(dto.UsuarioId, dto.ServicoId);
        if (jaAvaliou)
            throw new DomainException("Usuário já avaliou este serviço.");

        // 6. Criar avaliação
        var avaliacao = Avaliacao.Criar(usuario, servico, dto.Nota, dto.Comentario);
        var avaliacaoCriada = await _avaliacaoRepository.CreateAsync(avaliacao);

        // 7. Atualizar avaliação média do serviço
        var avaliacaoMedia = await _avaliacaoRepository.GetAvaliacaoMediaByServicoIdAsync(dto.ServicoId);
        servico.AdicionarAvaliacao(avaliacaoCriada);
        await _servicoRepository.UpdateAsync(servico);

        // 8. Recalcular avaliação média do profissional
        profissional.RecalcularAvaliacaoMedia();
        await _profissionalRepository.UpdateAvaliacaoMediaAsync(profissional.Id, profissional.AvaliacaoMedia);

        return new AvaliacaoDTO
        {
            Id = avaliacaoCriada.Id,
            ServicoId = avaliacaoCriada.ServicoId,
            UsuarioId = avaliacaoCriada.UsuarioId,
            NomeUsuario = usuario.Nome,
            Nota = avaliacaoCriada.Nota,
            Comentario = avaliacaoCriada.Comentario,
            Data = avaliacaoCriada.Data
        };
    }
}

