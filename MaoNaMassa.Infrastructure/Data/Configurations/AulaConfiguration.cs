using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MaoNaMassa.Domain.Entities;

namespace MaoNaMassa.Infrastructure.Data.Configurations;

public class AulaConfiguration : IEntityTypeConfiguration<Aula>
{
    public void Configure(EntityTypeBuilder<Aula> builder)
    {
        builder.ToTable("Aulas");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .IsRequired();

        builder.Property(a => a.CursoId)
            .IsRequired();

        builder.Property(a => a.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Conteudo)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(a => a.Ordem)
            .IsRequired();

        // Índices
        builder.HasIndex(a => a.CursoId)
            .HasDatabaseName("IX_Aulas_CursoId");

        builder.HasIndex(a => new { a.CursoId, a.Ordem })
            .IsUnique()
            .HasDatabaseName("IX_Aulas_CursoId_Ordem");

        // Relacionamentos
        builder.HasOne(a => a.Curso)
            .WithMany(c => c.Aulas)
            .HasForeignKey(a => a.CursoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

