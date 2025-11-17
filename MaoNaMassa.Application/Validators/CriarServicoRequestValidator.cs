using FluentValidation;
using MaoNaMassa.Application.DTOs.Input;

namespace MaoNaMassa.Application.Validators;

/// <summary>
/// Validador para CriarServicoRequest
/// </summary>
public class CriarServicoRequestValidator : AbstractValidator<CriarServicoRequest>
{
    public CriarServicoRequestValidator()
    {
        RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("Título é obrigatório.")
            .MinimumLength(5).WithMessage("Título deve ter no mínimo 5 caracteres.")
            .MaximumLength(200).WithMessage("Título deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("Descrição é obrigatória.")
            .MinimumLength(20).WithMessage("Descrição deve ter no mínimo 20 caracteres.")
            .MaximumLength(1000).WithMessage("Descrição deve ter no máximo 1000 caracteres.");

        RuleFor(x => x.Cidade)
            .NotEmpty().WithMessage("Cidade é obrigatória.")
            .MaximumLength(100).WithMessage("Cidade deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Preco)
            .GreaterThanOrEqualTo(0).WithMessage("Preço não pode ser negativo.")
            .When(x => x.Preco.HasValue);
    }
}

