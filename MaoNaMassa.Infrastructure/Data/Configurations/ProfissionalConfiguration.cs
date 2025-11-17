using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MaoNaMassa.Domain.Entities;

namespace MaoNaMassa.Infrastructure.Data.Configurations;

public class ProfissionalConfiguration : IEntityTypeConfiguration<Profissional>
{
    public void Configure(EntityTypeBuilder<Profissional> builder)
    {
        builder.ToTable("Profissionais");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .IsRequired();

        builder.Property(p => p.UsuarioId)
            .IsRequired();

        builder.Property(p => p.Descricao)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(p => p.AvaliacaoMedia)
            .HasColumnType("decimal(3,2)")
            .IsRequired(false);

        builder.Property(p => p.Disponivel)
            .IsRequired()
            .HasDefaultValue(true);

        // Índices
        builder.HasIndex(p => p.UsuarioId)
            .IsUnique()
            .HasDatabaseName("IX_Profissionais_UsuarioId");

        builder.HasIndex(p => p.Disponivel)
            .HasDatabaseName("IX_Profissionais_Disponivel");

        // Relacionamentos
        builder.HasOne(p => p.Usuario)
            .WithOne(u => u.Profissional)
            .HasForeignKey<Profissional>(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Servicos)
            .WithOne(s => s.Profissional)
            .HasForeignKey(s => s.ProfissionalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

