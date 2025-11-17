using FluentValidation;
using MaoNaMassa.Application.DTOs.Input;

namespace MaoNaMassa.Application.Validators;

/// <summary>
/// Validador para ResponderQuizRequest
/// </summary>
public class ResponderQuizRequestValidator : AbstractValidator<ResponderQuizRequest>
{
    public ResponderQuizRequestValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty().WithMessage("ID do quiz é obrigatório.");

        RuleFor(x => x.Resposta)
            .NotEmpty().WithMessage("Resposta é obrigatória.")
            .MaximumLength(200).WithMessage("Resposta deve ter no máximo 200 caracteres.");
    }
}

