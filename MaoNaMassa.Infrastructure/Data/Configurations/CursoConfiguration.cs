using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MaoNaMassa.Domain.Entities;

namespace MaoNaMassa.Infrastructure.Data.Configurations;

public class CursoConfiguration : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> builder)
    {
        builder.ToTable("Cursos");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .IsRequired();

        builder.Property(c => c.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Descricao)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(c => c.Area)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Nivel)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.DataCriacao)
            .IsRequired();

        // Índices
        builder.HasIndex(c => c.Area)
            .HasDatabaseName("IX_Cursos_Area");

        builder.HasIndex(c => c.Nivel)
            .HasDatabaseName("IX_Cursos_Nivel");

        // Relacionamentos
        builder.HasMany(c => c.Aulas)
            .WithOne(a => a.Curso)
            .HasForeignKey(a => a.CursoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Quizzes)
            .WithOne(q => q.Curso)
            .HasForeignKey(q => q.CursoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Certificados)
            .WithOne(cert => cert.Curso)
            .HasForeignKey(cert => cert.CursoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

