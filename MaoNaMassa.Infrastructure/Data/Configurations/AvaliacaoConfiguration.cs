using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MaoNaMassa.Domain.Entities;

namespace MaoNaMassa.Infrastructure.Data.Configurations;

public class AvaliacaoConfiguration : IEntityTypeConfiguration<Avaliacao>
{
    public void Configure(EntityTypeBuilder<Avaliacao> builder)
    {
        builder.ToTable("Avaliacoes");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .IsRequired();

        builder.Property(a => a.ServicoId)
            .IsRequired();

        builder.Property(a => a.UsuarioId)
            .IsRequired();

        builder.Property(a => a.Nota)
            .IsRequired();

        builder.Property(a => a.Comentario)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(a => a.Data)
            .IsRequired();

        // Índices
        builder.HasIndex(a => a.ServicoId)
            .HasDatabaseName("IX_Avaliacoes_ServicoId");

        builder.HasIndex(a => a.UsuarioId)
            .HasDatabaseName("IX_Avaliacoes_UsuarioId");

        builder.HasIndex(a => a.Data)
            .HasDatabaseName("IX_Avaliacoes_Data");

        // Índice único: um usuário só pode avaliar um serviço uma vez
        builder.HasIndex(a => new { a.UsuarioId, a.ServicoId })
            .IsUnique()
            .HasDatabaseName("IX_Avaliacoes_UsuarioId_ServicoId");

        // Relacionamentos
        builder.HasOne(a => a.Servico)
            .WithMany(s => s.Avaliacoes)
            .HasForeignKey(a => a.ServicoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Usuario)
            .WithMany(u => u.Avaliacoes)
            .HasForeignKey(a => a.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

