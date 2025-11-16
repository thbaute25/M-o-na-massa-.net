using MaoNaMassa.Application.DTOs;
using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Domain.Exceptions;
using MaoNaMassa.Domain.Interfaces;

namespace MaoNaMassa.Application.Services;

public class CertificadoService : ICertificadoService
{
    private readonly ICertificadoRepository _certificadoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ICursoRepository _cursoRepository;
    private readonly IRespostaQuizRepository _respostaQuizRepository;
    private readonly IQuizRepository _quizRepository;

    public CertificadoService(
        ICertificadoRepository certificadoRepository,
        IUsuarioRepository usuarioRepository,
        ICursoRepository cursoRepository,
        IRespostaQuizRepository respostaQuizRepository,
        IQuizRepository quizRepository)
    {
        _certificadoRepository = certificadoRepository;
        _usuarioRepository = usuarioRepository;
        _cursoRepository = cursoRepository;
        _respostaQuizRepository = respostaQuizRepository;
        _quizRepository = quizRepository;
    }

    public async Task<CertificadoDTO?> GerarCertificadoAsync(Guid usuarioId, Guid cursoId)
    {
        // Regra de Negócio 1: Validar se usuário existe
        var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
        if (usuario == null)
            throw new EntityNotFoundException("Usuário", usuarioId);

        // Regra de Negócio 2: Validar se curso existe
        var curso = await _cursoRepository.GetByIdAsync(cursoId);
        if (curso == null)
            throw new EntityNotFoundException("Curso", cursoId);

        // Regra de Negócio 3: Verificar se usuário já possui certificado deste curso
        var jaPossui = await _certificadoRepository.UsuarioJaPossuiCertificadoAsync(usuarioId, cursoId);
        if (jaPossui)
            throw new DomainException("Usuário já possui certificado para este curso.");

        // Regra de Negócio 4: Validar se curso possui quizzes
        if (!curso.Quizzes.Any())
            throw new DomainException("Curso não possui quizzes para avaliação.");

        // Regra de Negócio 5: Calcular nota final baseada nas respostas dos quizzes
        var totalQuizzes = curso.Quizzes.Count;
        var respostasCorretas = await _respostaQuizRepository.CountRespostasCorretasByUsuarioAndCursoAsync(usuarioId, cursoId);

        if (respostasCorretas == 0)
            throw new DomainException("Usuário não respondeu nenhum quiz do curso.");

        // Calcular nota: (respostas corretas / total de quizzes) * 100
        var notaFinal = (decimal)(respostasCorretas * 100.0 / totalQuizzes);

        // Regra de Negócio 6: Criar certificado apenas se nota >= 70
        if (notaFinal < 70)
            throw new DomainException($"Nota final ({notaFinal:F2}) insuficiente para certificado. Mínimo necessário: 70.");

        // Regra de Negócio 7: Criar certificado
        var certificado = Certificado.Criar(usuario, curso, notaFinal);
        var certificadoCriado = await _certificadoRepository.CreateAsync(certificado);

        return new CertificadoDTO
        {
            Id = certificadoCriado.Id,
            UsuarioId = certificadoCriado.UsuarioId,
            CursoId = certificadoCriado.CursoId,
            NomeUsuario = usuario.Nome,
            TituloCurso = curso.Titulo,
            NotaFinal = certificadoCriado.NotaFinal,
            DataConclusao = certificadoCriado.DataConclusao,
            CodigoCertificado = certificadoCriado.CodigoCertificado,
            Aprovado = certificadoCriado.EstaAprovado()
        };
    }

    public async Task<CertificadoDTO?> GetByIdAsync(Guid id)
    {
        var certificado = await _certificadoRepository.GetByIdAsync(id);
        if (certificado == null)
            return null;

        return await MapToDTOAsync(certificado);
    }

    public async Task<CertificadoDTO?> GetByCodigoAsync(string codigo)
    {
        var certificado = await _certificadoRepository.GetByCodigoAsync(codigo);
        if (certificado == null)
            return null;

        return await MapToDTOAsync(certificado);
    }

    public async Task<IEnumerable<CertificadoDTO>> GetByUsuarioIdAsync(Guid usuarioId)
    {
        var certificados = await _certificadoRepository.GetByUsuarioIdAsync(usuarioId);
        var dtos = new List<CertificadoDTO>();

        foreach (var certificado in certificados)
        {
            dtos.Add(await MapToDTOAsync(certificado));
        }

        return dtos;
    }

    public async Task<bool> UsuarioPossuiCertificadoAsync(Guid usuarioId, Guid cursoId)
    {
        return await _certificadoRepository.UsuarioJaPossuiCertificadoAsync(usuarioId, cursoId);
    }

    private async Task<CertificadoDTO> MapToDTOAsync(Certificado certificado)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(certificado.UsuarioId);
        var curso = await _cursoRepository.GetByIdAsync(certificado.CursoId);

        return new CertificadoDTO
        {
            Id = certificado.Id,
            UsuarioId = certificado.UsuarioId,
            CursoId = certificado.CursoId,
            NomeUsuario = usuario?.Nome ?? string.Empty,
            TituloCurso = curso?.Titulo ?? string.Empty,
            NotaFinal = certificado.NotaFinal,
            DataConclusao = certificado.DataConclusao,
            CodigoCertificado = certificado.CodigoCertificado,
            Aprovado = certificado.EstaAprovado()
        };
    }
}

