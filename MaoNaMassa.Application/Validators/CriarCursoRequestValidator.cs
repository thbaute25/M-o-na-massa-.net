using FluentValidation;
using MaoNaMassa.Application.DTOs.Input;

namespace MaoNaMassa.Application.Validators;

/// <summary>
/// Validador para CriarCursoRequest
/// </summary>
public class CriarCursoRequestValidator : AbstractValidator<CriarCursoRequest>
{
    public CriarCursoRequestValidator()
    {
        RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("Título é obrigatório.")
            .MinimumLength(5).WithMessage("Título deve ter no mínimo 5 caracteres.")
            .MaximumLength(200).WithMessage("Título deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("Descrição é obrigatória.")
            .MinimumLength(10).WithMessage("Descrição deve ter no mínimo 10 caracteres.")
            .MaximumLength(1000).WithMessage("Descrição deve ter no máximo 1000 caracteres.");

        RuleFor(x => x.Area)
            .NotEmpty().WithMessage("Área é obrigatória.")
            .MaximumLength(100).WithMessage("Área deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Nivel)
            .NotEmpty().WithMessage("Nível é obrigatório.")
            .Must(nivel => new[] { "Iniciante", "Intermediário", "Avançado" }.Contains(nivel))
            .WithMessage("Nível inválido. Valores válidos: Iniciante, Intermediário, Avançado.");
    }
}

