using MaoNaMassa.Application.DTOs;
using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Domain.Exceptions;
using MaoNaMassa.Domain.Interfaces;

namespace MaoNaMassa.Application.Services;

public class AvaliacaoService : IAvaliacaoService
{
    private readonly IAvaliacaoRepository _avaliacaoRepository;
    private readonly IServicoRepository _servicoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IProfissionalRepository _profissionalRepository;

    public AvaliacaoService(
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

    public async Task<AvaliacaoDTO> CriarAvaliacaoAsync(CreateAvaliacaoDTO dto)
    {
        // Regra de Negócio 1: Validar se usuário existe
        var usuario = await _usuarioRepository.GetByIdAsync(dto.UsuarioId);
        if (usuario == null)
            throw new EntityNotFoundException("Usuário", dto.UsuarioId);

        // Regra de Negócio 2: Validar se serviço existe
        var servico = await _servicoRepository.GetByIdAsync(dto.ServicoId);
        if (servico == null)
            throw new EntityNotFoundException("Serviço", dto.ServicoId);

        // Regra de Negócio 3: Usuário não pode avaliar seu próprio serviço
        var profissional = await _profissionalRepository.GetByIdAsync(servico.ProfissionalId);
        if (profissional == null)
            throw new DomainException("Profissional do serviço não encontrado.");

        if (profissional.UsuarioId == dto.UsuarioId)
            throw new DomainException("Usuário não pode avaliar seu próprio serviço.");

        // Regra de Negócio 4: Verificar se usuário já avaliou este serviço
        var jaAvaliou = await _avaliacaoRepository.UsuarioJaAvaliouServicoAsync(dto.UsuarioId, dto.ServicoId);
        if (jaAvaliou)
            throw new DomainException("Usuário já avaliou este serviço.");

        // Regra de Negócio 5: Criar avaliação
        var avaliacao = Avaliacao.Criar(usuario, servico, dto.Nota, dto.Comentario);
        var avaliacaoCriada = await _avaliacaoRepository.CreateAsync(avaliacao);

        // Regra de Negócio 6: Atualizar avaliação média do serviço
        var avaliacaoMedia = await _avaliacaoRepository.GetAvaliacaoMediaByServicoIdAsync(dto.ServicoId);
        servico.AdicionarAvaliacao(avaliacaoCriada);
        await _servicoRepository.UpdateAsync(servico);

        // Regra de Negócio 7: Recalcular avaliação média do profissional
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

    public async Task<IEnumerable<AvaliacaoDTO>> GetByServicoIdAsync(Guid servicoId)
    {
        var avaliacoes = await _avaliacaoRepository.GetByServicoIdAsync(servicoId);
        var dtos = new List<AvaliacaoDTO>();

        foreach (var avaliacao in avaliacoes)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(avaliacao.UsuarioId);
            dtos.Add(new AvaliacaoDTO
            {
                Id = avaliacao.Id,
                ServicoId = avaliacao.ServicoId,
                UsuarioId = avaliacao.UsuarioId,
                NomeUsuario = usuario?.Nome ?? string.Empty,
                Nota = avaliacao.Nota,
                Comentario = avaliacao.Comentario,
                Data = avaliacao.Data
            });
        }

        return dtos;
    }

    public async Task<decimal?> GetAvaliacaoMediaByServicoIdAsync(Guid servicoId)
    {
        return await _avaliacaoRepository.GetAvaliacaoMediaByServicoIdAsync(servicoId);
    }
}

