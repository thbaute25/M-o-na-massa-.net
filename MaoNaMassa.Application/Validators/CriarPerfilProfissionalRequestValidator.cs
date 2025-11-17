using FluentValidation;
using MaoNaMassa.Application.DTOs.Input;

namespace MaoNaMassa.Application.Validators;

/// <summary>
/// Validador para CriarPerfilProfissionalRequest
/// </summary>
public class CriarPerfilProfissionalRequestValidator : AbstractValidator<CriarPerfilProfissionalRequest>
{
    public CriarPerfilProfissionalRequestValidator()
    {
        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("Descrição é obrigatória.")
            .MinimumLength(20).WithMessage("Descrição deve ter no mínimo 20 caracteres.")
            .MaximumLength(1000).WithMessage("Descrição deve ter no máximo 1000 caracteres.");
    }
}

