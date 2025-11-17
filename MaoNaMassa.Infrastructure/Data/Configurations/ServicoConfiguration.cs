using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MaoNaMassa.Domain.Entities;

namespace MaoNaMassa.Infrastructure.Data.Configurations;

public class ServicoConfiguration : IEntityTypeConfiguration<Servico>
{
    public void Configure(EntityTypeBuilder<Servico> builder)
    {
        builder.ToTable("Servicos");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .IsRequired();

        builder.Property(s => s.ProfissionalId)
            .IsRequired();

        builder.Property(s => s.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Descricao)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(s => s.Cidade)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Preco)
            .HasColumnType("decimal(10,2)")
            .IsRequired(false);

        builder.Property(s => s.DataPublicacao)
            .IsRequired();

        // Índices
        builder.HasIndex(s => s.ProfissionalId)
            .HasDatabaseName("IX_Servicos_ProfissionalId");

        builder.HasIndex(s => s.Cidade)
            .HasDatabaseName("IX_Servicos_Cidade");

        builder.HasIndex(s => s.DataPublicacao)
            .HasDatabaseName("IX_Servicos_DataPublicacao");

        // Relacionamentos
        builder.HasOne(s => s.Profissional)
            .WithMany(p => p.Servicos)
            .HasForeignKey(s => s.ProfissionalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Avaliacoes)
            .WithOne(a => a.Servico)
            .HasForeignKey(a => a.ServicoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

