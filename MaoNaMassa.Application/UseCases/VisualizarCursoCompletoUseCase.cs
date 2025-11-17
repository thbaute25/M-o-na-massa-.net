using MaoNaMassa.Application.DTOs;
using MaoNaMassa.Domain.Exceptions;
using MaoNaMassa.Domain.Interfaces;

namespace MaoNaMassa.Application.UseCases;

/// <summary>
/// Caso de Uso: Visualizar Curso Completo
/// Permite visualizar um curso com todas as suas aulas e quizzes.
/// </summary>
public class VisualizarCursoCompletoUseCase
{
    private readonly ICursoRepository _cursoRepository;
    private readonly IAulaRepository _aulaRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly IRespostaQuizRepository _respostaQuizRepository;
    private readonly ICertificadoRepository _certificadoRepository;

    public VisualizarCursoCompletoUseCase(
        ICursoRepository cursoRepository,
        IAulaRepository aulaRepository,
        IQuizRepository quizRepository,
        IRespostaQuizRepository respostaQuizRepository,
        ICertificadoRepository certificadoRepository)
    {
        _cursoRepository = cursoRepository;
        _aulaRepository = aulaRepository;
        _quizRepository = quizRepository;
        _respostaQuizRepository = respostaQuizRepository;
        _certificadoRepository = certificadoRepository;
    }

    /// <summary>
    /// Executa o caso de uso: Visualizar curso completo
    /// </summary>
    public async Task<CursoCompletoDTO> ExecuteAsync(Guid cursoId, Guid? usuarioId = null)
    {
        // 1. Validar curso
        var curso = await _cursoRepository.GetByIdAsync(cursoId);
        if (curso == null)
            throw new EntityNotFoundException("Curso", cursoId);

        // 2. Buscar aulas do curso (ordenadas)
        var aulas = await _aulaRepository.GetByCursoIdAsync(cursoId);
        var aulasOrdenadas = aulas.OrderBy(a => a.Ordem).ToList();

        // 3. Buscar quizzes do curso
        var quizzes = await _quizRepository.GetByCursoIdAsync(cursoId);

        // 4. Mapear para DTO
        var cursoCompleto = new CursoCompletoDTO
        {
            Id = curso.Id,
            Titulo = curso.Titulo,
            Descricao = curso.Descricao,
            Area = curso.Area,
            Nivel = curso.Nivel,
            DataCriacao = curso.DataCriacao,
            TotalAulas = aulasOrdenadas.Count,
            TotalQuizzes = quizzes.Count(),
            TotalCertificadosEmitidos = curso.Certificados.Count,
            Aulas = aulasOrdenadas.Select(a => new AulaDTO
            {
                Id = a.Id,
                CursoId = a.CursoId,
                Titulo = a.Titulo,
                Conteudo = a.Conteudo,
                Ordem = a.Ordem
            }).ToList(),
            Quizzes = quizzes.Select(q => new QuizDTO
            {
                Id = q.Id,
                CursoId = q.CursoId,
                Pergunta = q.Pergunta,
                // Ocultar resposta correta se não for o próprio usuário ou se já respondeu
                RespostaCorreta = string.Empty // Em produção, verificar permissões
            }).ToList()
        };

        // 5. Se usuário fornecido, verificar progresso (pode ser usado para exibir progresso na UI)
        if (usuarioId.HasValue)
        {
            var respostas = await _respostaQuizRepository.GetByUsuarioIdAsync(usuarioId.Value);
            var respostasDoCurso = respostas.Where(r => quizzes.Any(q => q.Id == r.QuizId)).ToList();
            var possuiCertificado = await _certificadoRepository.UsuarioJaPossuiCertificadoAsync(usuarioId.Value, cursoId);
            
            // Informações de progresso podem ser adicionadas ao DTO se necessário
            // Por enquanto, apenas retornamos o curso completo
        }

        return cursoCompleto;
    }
}

