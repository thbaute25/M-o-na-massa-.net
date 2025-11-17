using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MaoNaMassa.Domain.Entities;

namespace MaoNaMassa.Infrastructure.Data.Configurations;

public class CertificadoConfiguration : IEntityTypeConfiguration<Certificado>
{
    public void Configure(EntityTypeBuilder<Certificado> builder)
    {
        builder.ToTable("Certificados");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .IsRequired();

        builder.Property(c => c.UsuarioId)
            .IsRequired();

        builder.Property(c => c.CursoId)
            .IsRequired();

        builder.Property(c => c.NotaFinal)
            .IsRequired()
            .HasColumnType("decimal(5,2)");

        builder.Property(c => c.DataConclusao)
            .IsRequired();

        builder.Property(c => c.CodigoCertificado)
            .IsRequired()
            .HasMaxLength(50);

        // Índices
        builder.HasIndex(c => c.UsuarioId)
            .HasDatabaseName("IX_Certificados_UsuarioId");

        builder.HasIndex(c => c.CursoId)
            .HasDatabaseName("IX_Certificados_CursoId");

        builder.HasIndex(c => c.CodigoCertificado)
            .IsUnique()
            .HasDatabaseName("IX_Certificados_CodigoCertificado");

        // Índice único: um usuário só pode ter um certificado por curso
        builder.HasIndex(c => new { c.UsuarioId, c.CursoId })
            .IsUnique()
            .HasDatabaseName("IX_Certificados_UsuarioId_CursoId");

        // Relacionamentos
        builder.HasOne(c => c.Usuario)
            .WithMany(u => u.Certificados)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Curso)
            .WithMany(curso => curso.Certificados)
            .HasForeignKey(c => c.CursoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

