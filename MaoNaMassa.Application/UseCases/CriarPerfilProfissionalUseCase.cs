using MaoNaMassa.Application.DTOs;
using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Domain.Exceptions;
using MaoNaMassa.Domain.Interfaces;

namespace MaoNaMassa.Application.UseCases;

/// <summary>
/// Caso de Uso: Criar Perfil Profissional
/// Permite que um usuário crie seu perfil profissional para oferecer serviços.
/// </summary>
public class CriarPerfilProfissionalUseCase
{
    private readonly IProfissionalRepository _profissionalRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ICertificadoRepository _certificadoRepository;

    public CriarPerfilProfissionalUseCase(
        IProfissionalRepository profissionalRepository,
        IUsuarioRepository usuarioRepository,
        ICertificadoRepository certificadoRepository)
    {
        _profissionalRepository = profissionalRepository;
        _usuarioRepository = usuarioRepository;
        _certificadoRepository = certificadoRepository;
    }

    /// <summary>
    /// Executa o caso de uso: Criar perfil profissional
    /// </summary>
    public async Task<ProfissionalDTO> ExecuteAsync(CreateProfissionalDTO dto)
    {
        // 1. Validar usuário
        var usuario = await _usuarioRepository.GetByIdAsync(dto.UsuarioId);
        if (usuario == null)
            throw new EntityNotFoundException("Usuário", dto.UsuarioId);

        // 2. Verificar se já possui perfil
        var perfilExistente = await _profissionalRepository.GetByUsuarioIdAsync(dto.UsuarioId);
        if (perfilExistente != null)
            throw new DomainException("Usuário já possui perfil profissional.");

        // 3. Validar tipo de usuário
        if (usuario.TipoUsuario != "Profissional" && usuario.TipoUsuario != "Aprendiz")
            throw new DomainException($"Usuário do tipo '{usuario.TipoUsuario}' não pode criar perfil profissional.");

        // 4. Opcional: Verificar se possui certificados (recomendação)
        var certificados = await _certificadoRepository.GetByUsuarioIdAsync(dto.UsuarioId);
        if (!certificados.Any())
        {
            // Não bloqueia, mas é uma recomendação
            // Em produção, pode ser apenas um aviso
        }

        // 5. Criar perfil profissional
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
}

