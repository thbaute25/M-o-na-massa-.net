using FluentValidation;
using MaoNaMassa.Application.DTOs.Input;

namespace MaoNaMassa.Application.Validators;

/// <summary>
/// Validador para AvaliarServicoRequest
/// </summary>
public class AvaliarServicoRequestValidator : AbstractValidator<AvaliarServicoRequest>
{
    public AvaliarServicoRequestValidator()
    {
        RuleFor(x => x.ServicoId)
            .NotEmpty().WithMessage("ID do serviço é obrigatório.");

        RuleFor(x => x.Nota)
            .NotEmpty().WithMessage("Nota é obrigatória.")
            .InclusiveBetween(1, 5).WithMessage("Nota deve estar entre 1 e 5.");

        RuleFor(x => x.Comentario)
            .MaximumLength(500).WithMessage("Comentário deve ter no máximo 500 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Comentario));
    }
}

