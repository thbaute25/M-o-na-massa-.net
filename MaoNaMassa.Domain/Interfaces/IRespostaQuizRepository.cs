using MaoNaMassa.Domain.Entities;

namespace MaoNaMassa.Domain.Interfaces;

public interface IRespostaQuizRepository
{
    Task<RespostaQuiz?> GetByIdAsync(Guid id);
    Task<IEnumerable<RespostaQuiz>> GetByUsuarioIdAsync(Guid usuarioId);
    Task<IEnumerable<RespostaQuiz>> GetByQuizIdAsync(Guid quizId);
    Task<RespostaQuiz> CreateAsync(RespostaQuiz respostaQuiz);
    Task<int> CountRespostasCorretasByUsuarioAndCursoAsync(Guid usuarioId, Guid cursoId);
}

