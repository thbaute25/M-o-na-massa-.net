using MaoNaMassa.Application.DTOs;
using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Domain.Exceptions;
using MaoNaMassa.Domain.Interfaces;

namespace MaoNaMassa.Application.Services;

public class ProfissionalService : IProfissionalService
{
    private readonly IProfissionalRepository _profissionalRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public ProfissionalService(
        IProfissionalRepository profissionalRepository,
        IUsuarioRepository usuarioRepository)
    {
        _profissionalRepository = profissionalRepository;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<ProfissionalDTO> CriarPerfilProfissionalAsync(CreateProfissionalDTO dto)
    {
        // Regra de Negócio 1: Validar se usuário existe
        var usuario = await _usuarioRepository.GetByIdAsync(dto.UsuarioId);
        if (usuario == null)
            throw new EntityNotFoundException("Usuário", dto.UsuarioId);

        // Regra de Negócio 2: Verificar se usuário já possui perfil profissional
        var perfilExistente = await _profissionalRepository.GetByUsuarioIdAsync(dto.UsuarioId);
        if (perfilExistente != null)
            throw new DomainException("Usuário já possui perfil profissional.");

        // Regra de Negócio 3: Validar tipo de usuário (deve ser "Profissional" ou "Aprendiz")
        if (usuario.TipoUsuario != "Profissional" && usuario.TipoUsuario != "Aprendiz")
            throw new DomainException($"Usuário do tipo '{usuario.TipoUsuario}' não pode criar perfil profissional.");

        // Regra de Negócio 4: Criar perfil profissional
        var profissional = new Profissional(dto.UsuarioId, dto.Descricao);
        var profissionalCriado = await _profissionalRepository.CreateAsync(profissional);

        return new ProfissionalDTO
        {
            Id = profissionalCriado.Id,
            UsuarioId = profissionalCriado.UsuarioId,
            NomeUsuario = usuario.Nome,
            EmailUsuario = usuario.Email,
            CidadeUsuario = usuario.Cidade,
            Descricao = profissionalCriado.Descricao,
            AvaliacaoMedia = profissionalCriado.AvaliacaoMedia,
            Disponivel = profissionalCriado.Disponivel,
            TotalServicos = 0,
            TotalAvaliacoes = 0
        };
    }

    public async Task<ProfissionalDTO?> GetByUsuarioIdAsync(Guid usuarioId)
    {
        var profissional = await _profissionalRepository.GetByUsuarioIdAsync(usuarioId);
        if (profissional == null)
            return null;

        return await MapToDTOAsync(profissional);
    }

    public async Task<ProfissionalDTO?> GetByIdAsync(Guid id)
    {
        var profissional = await _profissionalRepository.GetByIdAsync(id);
        if (profissional == null)
            return null;

        return await MapToDTOAsync(profissional);
    }

    public async Task<IEnumerable<ProfissionalDTO>> GetDisponiveisAsync()
    {
        var profissionais = await _profissionalRepository.GetDisponiveisAsync();
        var dtos = new List<ProfissionalDTO>();

        foreach (var profissional in profissionais)
        {
            dtos.Add(await MapToDTOAsync(profissional));
        }

        return dtos;
    }

    public async Task AtualizarDisponibilidadeAsync(Guid id, bool disponivel)
    {
        var profissional = await _profissionalRepository.GetByIdAsync(id);
        if (profissional == null)
            throw new EntityNotFoundException("Profissional", id);

        profissional.AlterarDisponibilidade(disponivel);
        await _profissionalRepository.UpdateAsync(profissional);
    }

    private async Task<ProfissionalDTO> MapToDTOAsync(Profissional profissional)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(profissional.UsuarioId);
        var totalAvaliacoes = profissional.Servicos.SelectMany(s => s.Avaliacoes).Count();

        return new ProfissionalDTO
        {
            Id = profissional.Id,
            UsuarioId = profissional.UsuarioId,
            NomeUsuario = usuario?.Nome ?? string.Empty,
            EmailUsuario = usuario?.Email ?? string.Empty,
            CidadeUsuario = usuario?.Cidade ?? string.Empty,
            Descricao = profissional.Descricao,
            AvaliacaoMedia = profissional.AvaliacaoMedia,
            Disponivel = profissional.Disponivel,
            TotalServicos = profissional.Servicos.Count,
            TotalAvaliacoes = totalAvaliacoes
        };
    }
}

