using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MaoNaMassa.Domain.Entities;

namespace MaoNaMassa.Infrastructure.Data.Configurations;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.ToTable("Quizzes");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Id)
            .IsRequired();

        builder.Property(q => q.CursoId)
            .IsRequired();

        builder.Property(q => q.Pergunta)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(q => q.RespostaCorreta)
            .IsRequired()
            .HasMaxLength(200);

        // Índices
        builder.HasIndex(q => q.CursoId)
            .HasDatabaseName("IX_Quizzes_CursoId");

        // Relacionamentos
        builder.HasOne(q => q.Curso)
            .WithMany(c => c.Quizzes)
            .HasForeignKey(q => q.CursoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(q => q.RespostasQuiz)
            .WithOne(rq => rq.Quiz)
            .HasForeignKey(rq => rq.QuizId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

