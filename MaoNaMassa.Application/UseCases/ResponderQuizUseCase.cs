using MaoNaMassa.Application.DTOs;
using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Domain.Exceptions;
using MaoNaMassa.Domain.Interfaces;

namespace MaoNaMassa.Application.UseCases;

/// <summary>
/// Caso de Uso: Responder Quiz
/// Permite que um usuário responda um quiz de um curso.
/// </summary>
public class ResponderQuizUseCase
{
    private readonly IQuizRepository _quizRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IRespostaQuizRepository _respostaQuizRepository;
    private readonly ICursoRepository _cursoRepository;

    public ResponderQuizUseCase(
        IQuizRepository quizRepository,
        IUsuarioRepository usuarioRepository,
        IRespostaQuizRepository respostaQuizRepository,
        ICursoRepository cursoRepository)
    {
        _quizRepository = quizRepository;
        _usuarioRepository = usuarioRepository;
        _respostaQuizRepository = respostaQuizRepository;
        _cursoRepository = cursoRepository;
    }

    /// <summary>
    /// Executa o caso de uso: Responder quiz
    /// </summary>
    public async Task<ResultadoQuizDTO> ExecuteAsync(Guid usuarioId, ResponderQuizDTO dto)
    {
        // 1. Validar usuário
        var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
        if (usuario == null)
            throw new EntityNotFoundException("Usuário", usuarioId);

        // 2. Validar quiz
        var quiz = await _quizRepository.GetByIdAsync(dto.QuizId);
        if (quiz == null)
            throw new EntityNotFoundException("Quiz", dto.QuizId);

        // 3. Validar curso do quiz
        var curso = await _cursoRepository.GetByIdAsync(quiz.CursoId);
        if (curso == null)
            throw new DomainException("Curso do quiz não encontrado.");

        // 4. Verificar se usuário já respondeu este quiz
        var respostasExistentes = await _respostaQuizRepository.GetByUsuarioIdAsync(usuarioId);
        var jaRespondeu = respostasExistentes.Any(r => r.QuizId == dto.QuizId);
        
        if (jaRespondeu)
            throw new DomainException("Usuário já respondeu este quiz.");

        // 5. Criar resposta do quiz
        var respostaQuiz = RespostaQuiz.Criar(usuario, quiz, dto.Resposta);
        var respostaCriada = await _respostaQuizRepository.CreateAsync(respostaQuiz);

        // 6. Retornar resultado
        return new ResultadoQuizDTO
        {
            RespostaQuizId = respostaCriada.Id,
            Correta = respostaCriada.Correta,
            RespostaEnviada = respostaCriada.Resposta,
            RespostaCorreta = quiz.RespostaCorreta
        };
    }
}

