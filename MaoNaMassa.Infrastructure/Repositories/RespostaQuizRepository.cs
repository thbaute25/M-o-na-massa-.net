using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Domain.Interfaces;
using MaoNaMassa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MaoNaMassa.Infrastructure.Repositories;

public class RespostaQuizRepository : IRespostaQuizRepository
{
    private readonly ApplicationDbContext _context;

    public RespostaQuizRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RespostaQuiz?> GetByIdAsync(Guid id)
    {
        return await _context.RespostasQuiz
            .Include(r => r.Usuario)
            .Include(r => r.Quiz)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<RespostaQuiz>> GetByUsuarioIdAsync(Guid usuarioId)
    {
        return await _context.RespostasQuiz
            .Where(r => r.UsuarioId == usuarioId)
            .Include(r => r.Quiz)
            .ToListAsync();
    }

    public async Task<IEnumerable<RespostaQuiz>> GetByQuizIdAsync(Guid quizId)
    {
        return await _context.RespostasQuiz
            .Where(r => r.QuizId == quizId)
            .Include(r => r.Usuario)
            .ToListAsync();
    }

    public async Task<RespostaQuiz> CreateAsync(RespostaQuiz respostaQuiz)
    {
        _context.RespostasQuiz.Add(respostaQuiz);
        await _context.SaveChangesAsync();
        return respostaQuiz;
    }

    public async Task<int> CountRespostasCorretasByUsuarioAndCursoAsync(Guid usuarioId, Guid cursoId)
    {
        return await _context.RespostasQuiz
            .Where(r => r.UsuarioId == usuarioId && 
                       r.Correta && 
                       r.Quiz != null && 
                       r.Quiz.CursoId == cursoId)
            .CountAsync();
    }
}

