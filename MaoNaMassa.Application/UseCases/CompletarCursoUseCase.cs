using MaoNaMassa.Application.DTOs;
using MaoNaMassa.Domain.Exceptions;
using MaoNaMassa.Domain.Interfaces;

namespace MaoNaMassa.Application.UseCases;

/// <summary>
/// Caso de Uso: Completar Curso
/// Permite que um usuário complete um curso respondendo todos os quizzes e obtenha um certificado.
/// </summary>
public class CompletarCursoUseCase
{
    private readonly IQuizRepository _quizRepository;
    private readonly IRespostaQuizRepository _respostaQuizRepository;
    private readonly ICertificadoRepository _certificadoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ICursoRepository _cursoRepository;

    public CompletarCursoUseCase(
        IQuizRepository quizRepository,
        IRespostaQuizRepository respostaQuizRepository,
        ICertificadoRepository certificadoRepository,
        IUsuarioRepository usuarioRepository,
        ICursoRepository cursoRepository)
    {
        _quizRepository = quizRepository;
        _respostaQuizRepository = respostaQuizRepository;
        _certificadoRepository = certificadoRepository;
        _usuarioRepository = usuarioRepository;
        _cursoRepository = cursoRepository;
    }

    /// <summary>
    /// Executa o caso de uso: Completar curso e gerar certificado
    /// </summary>
    public async Task<CertificadoDTO> ExecuteAsync(Guid usuarioId, Guid cursoId)
    {
        // 1. Validar usuário
        var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
        if (usuario == null)
            throw new EntityNotFoundException("Usuário", usuarioId);

        // 2. Validar curso
        var curso = await _cursoRepository.GetByIdAsync(cursoId);
        if (curso == null)
            throw new EntityNotFoundException("Curso", cursoId);

        // 3. Verificar se já possui certificado
        var jaPossuiCertificado = await _certificadoRepository.UsuarioJaPossuiCertificadoAsync(usuarioId, cursoId);
        if (jaPossuiCertificado)
            throw new DomainException("Usuário já possui certificado para este curso.");

        // 4. Verificar se curso possui quizzes
        var quizzes = await _quizRepository.GetByCursoIdAsync(cursoId);
        if (!quizzes.Any())
            throw new DomainException("Curso não possui quizzes para avaliação.");

        // 5. Verificar se usuário respondeu todos os quizzes
        var respostas = await _respostaQuizRepository.GetByUsuarioIdAsync(usuarioId);
        var respostasDoCurso = respostas.Where(r => quizzes.Any(q => q.Id == r.QuizId)).ToList();
        
        if (respostasDoCurso.Count < quizzes.Count())
            throw new DomainException($"Usuário precisa responder todos os {quizzes.Count()} quizzes do curso antes de obter o certificado.");

        // 6. Calcular nota final
        var totalQuizzes = quizzes.Count();
        var respostasCorretas = respostasDoCurso.Count(r => r.Correta);
        var notaFinal = (decimal)(respostasCorretas * 100.0 / totalQuizzes);

        // 7. Validar nota mínima
        if (notaFinal < 70)
            throw new DomainException($"Nota final ({notaFinal:F2}) insuficiente para certificado. Mínimo necessário: 70. Respostas corretas: {respostasCorretas}/{totalQuizzes}");

        // 8. Gerar certificado
        var certificado = Domain.Entities.Certificado.Criar(usuario, curso, notaFinal);
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
}

