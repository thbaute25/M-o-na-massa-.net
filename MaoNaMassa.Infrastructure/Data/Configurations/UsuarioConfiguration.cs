using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MaoNaMassa.Domain.Entities;

namespace MaoNaMassa.Infrastructure.Data.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .IsRequired();

        builder.Property(u => u.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Senha)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Cidade)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.AreaInteresse)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.TipoUsuario)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.DataCriacao)
            .IsRequired();

        // Índices
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Usuarios_Email");

        builder.HasIndex(u => u.TipoUsuario)
            .HasDatabaseName("IX_Usuarios_TipoUsuario");

        builder.HasIndex(u => u.Cidade)
            .HasDatabaseName("IX_Usuarios_Cidade");

        // Relacionamentos
        builder.HasOne(u => u.Profissional)
            .WithOne(p => p.Usuario)
            .HasForeignKey<Profissional>(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.RespostasQuiz)
            .WithOne(rq => rq.Usuario)
            .HasForeignKey(rq => rq.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Certificados)
            .WithOne(c => c.Usuario)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Avaliacoes)
            .WithOne(a => a.Usuario)
            .HasForeignKey(a => a.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

