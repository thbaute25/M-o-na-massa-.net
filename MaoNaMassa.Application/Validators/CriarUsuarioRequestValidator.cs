using FluentValidation;
using MaoNaMassa.Application.DTOs.Input;

namespace MaoNaMassa.Application.Validators;

/// <summary>
/// Validador para CriarUsuarioRequest
/// </summary>
public class CriarUsuarioRequestValidator : AbstractValidator<CriarUsuarioRequest>
{
    public CriarUsuarioRequestValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MinimumLength(3).WithMessage("Nome deve ter no mínimo 3 caracteres.")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Email inválido.")
            .MaximumLength(255).WithMessage("Email deve ter no máximo 255 caracteres.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(6).WithMessage("Senha deve ter no mínimo 6 caracteres.");

        RuleFor(x => x.Cidade)
            .NotEmpty().WithMessage("Cidade é obrigatória.")
            .MaximumLength(100).WithMessage("Cidade deve ter no máximo 100 caracteres.");

        RuleFor(x => x.AreaInteresse)
            .NotEmpty().WithMessage("Área de interesse é obrigatória.")
            .MaximumLength(100).WithMessage("Área de interesse deve ter no máximo 100 caracteres.");

        RuleFor(x => x.TipoUsuario)
            .NotEmpty().WithMessage("Tipo de usuário é obrigatório.")
            .Must(tipo => new[] { "Aprendiz", "Cliente", "Empresa", "Profissional" }.Contains(tipo))
            .WithMessage("Tipo de usuário inválido. Valores válidos: Aprendiz, Cliente, Empresa, Profissional.");
    }
}

