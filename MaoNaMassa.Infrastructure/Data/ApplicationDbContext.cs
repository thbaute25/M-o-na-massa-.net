using Microsoft.EntityFrameworkCore;
using MaoNaMassa.Domain.Entities;
using MaoNaMassa.Infrastructure.Data.Configurations;

namespace MaoNaMassa.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Aula> Aulas { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<RespostaQuiz> RespostasQuiz { get; set; }
    public DbSet<Certificado> Certificados { get; set; }
    public DbSet<Profissional> Profissionais { get; set; }
    public DbSet<Servico> Servicos { get; set; }
    public DbSet<Avaliacao> Avaliacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar todas as configurações
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}

