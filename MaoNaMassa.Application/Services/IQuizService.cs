using MaoNaMassa.Application.DTOs;

namespace MaoNaMassa.Application.Services;

public interface IQuizService
{
    Task<ResultadoQuizDTO> ResponderQuizAsync(Guid usuarioId, ResponderQuizDTO dto);
    Task<IEnumerable<QuizDTO>> GetByCursoIdAsync(Guid cursoId);
    Task<QuizDTO> CreateAsync(CreateQuizDTO dto);
}

