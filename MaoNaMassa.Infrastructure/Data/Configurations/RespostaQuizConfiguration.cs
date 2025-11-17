using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MaoNaMassa.Domain.Entities;

namespace MaoNaMassa.Infrastructure.Data.Configurations;

public class RespostaQuizConfiguration : IEntityTypeConfiguration<RespostaQuiz>
{
    public void Configure(EntityTypeBuilder<RespostaQuiz> builder)
    {
        builder.ToTable("RespostasQuiz");

        builder.HasKey(rq => rq.Id);

        builder.Property(rq => rq.Id)
            .IsRequired();

        builder.Property(rq => rq.UsuarioId)
            .IsRequired();

        builder.Property(rq => rq.QuizId)
            .IsRequired();

        builder.Property(rq => rq.Resposta)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(rq => rq.Correta)
            .IsRequired();

        builder.Property(rq => rq.DataResposta)
            .IsRequired();

        // Índices
        builder.HasIndex(rq => rq.UsuarioId)
            .HasDatabaseName("IX_RespostasQuiz_UsuarioId");

        builder.HasIndex(rq => rq.QuizId)
            .HasDatabaseName("IX_RespostasQuiz_QuizId");

        // Índice único: um usuário só pode responder um quiz uma vez
        builder.HasIndex(rq => new { rq.UsuarioId, rq.QuizId })
            .IsUnique()
            .HasDatabaseName("IX_RespostasQuiz_UsuarioId_QuizId");

        // Relacionamentos
        builder.HasOne(rq => rq.Usuario)
            .WithMany(u => u.RespostasQuiz)
            .HasForeignKey(rq => rq.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rq => rq.Quiz)
            .WithMany(q => q.RespostasQuiz)
            .HasForeignKey(rq => rq.QuizId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

