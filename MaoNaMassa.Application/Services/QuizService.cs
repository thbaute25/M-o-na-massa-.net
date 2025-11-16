using MaoNaMassa.Application.DTOs;
using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Domain.Exceptions;
using MaoNaMassa.Domain.Interfaces;

namespace MaoNaMassa.Application.Services;

public class QuizService : IQuizService
{
    private readonly IQuizRepository _quizRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IRespostaQuizRepository _respostaQuizRepository;
    private readonly ICursoRepository _cursoRepository;

    public QuizService(
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

    public async Task<ResultadoQuizDTO> ResponderQuizAsync(Guid usuarioId, ResponderQuizDTO dto)
    {
        // Regra de Negócio 1: Validar se usuário existe
        var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
        if (usuario == null)
            throw new EntityNotFoundException("Usuário", usuarioId);

        // Regra de Negócio 2: Validar se quiz existe
        var quiz = await _quizRepository.GetByIdAsync(dto.QuizId);
        if (quiz == null)
            throw new EntityNotFoundException("Quiz", dto.QuizId);

        // Regra de Negócio 3: Validar se quiz pertence a um curso válido
        var curso = await _cursoRepository.GetByIdAsync(quiz.CursoId);
        if (curso == null)
            throw new DomainException("Curso do quiz não encontrado.");

        // Regra de Negócio 4: Verificar se usuário já respondeu este quiz
        var respostasExistentes = await _respostaQuizRepository.GetByUsuarioIdAsync(usuarioId);
        var jaRespondeu = respostasExistentes.Any(r => r.QuizId == dto.QuizId);
        
        if (jaRespondeu)
            throw new DomainException("Usuário já respondeu este quiz.");

        // Regra de Negócio 5: Criar resposta do quiz
        var respostaQuiz = RespostaQuiz.Criar(usuario, quiz, dto.Resposta);
        var respostaCriada = await _respostaQuizRepository.CreateAsync(respostaQuiz);

        return new ResultadoQuizDTO
        {
            RespostaQuizId = respostaCriada.Id,
            Correta = respostaCriada.Correta,
            RespostaEnviada = respostaCriada.Resposta,
            RespostaCorreta = quiz.RespostaCorreta
        };
    }

    public async Task<IEnumerable<QuizDTO>> GetByCursoIdAsync(Guid cursoId)
    {
        var quizzes = await _quizRepository.GetByCursoIdAsync(cursoId);
        return quizzes.Select(q => new QuizDTO
        {
            Id = q.Id,
            CursoId = q.CursoId,
            Pergunta = q.Pergunta,
            RespostaCorreta = q.RespostaCorreta // Em produção, não expor a resposta correta antes de responder
        });
    }

    public async Task<QuizDTO> CreateAsync(CreateQuizDTO dto)
    {
        // Regra de Negócio: Validar se curso existe
        var curso = await _cursoRepository.GetByIdAsync(dto.CursoId);
        if (curso == null)
            throw new EntityNotFoundException("Curso", dto.CursoId);

        var quiz = new Quiz(dto.CursoId, dto.Pergunta, dto.RespostaCorreta);
        var quizCriado = await _quizRepository.CreateAsync(quiz);

        return new QuizDTO
        {
            Id = quizCriado.Id,
            CursoId = quizCriado.CursoId,
            Pergunta = quizCriado.Pergunta,
            RespostaCorreta = quizCriado.RespostaCorreta
        };
    }
}

