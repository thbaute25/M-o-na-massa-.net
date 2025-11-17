using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using MaoNaMassa.Application.Validators;

namespace MaoNaMassa.API.Extensions;

/// <summary>
/// Extensões para configuração de serviços
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configura validações e tratamento de erros
    /// </summary>
    public static IServiceCollection AddValidationAndErrorHandling(this IServiceCollection services)
    {
        // FluentValidation
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<CriarUsuarioRequestValidator>();

        // Configurar ProblemDetails
        services.AddProblemDetails();

        // Configurar comportamento de validação
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "Erro de Validação",
                    Status = 400,
                    Detail = "Um ou mais erros de validação ocorreram.",
                    Instance = context.HttpContext.Request.Path
                };

                if (errors.Any())
                {
                    problemDetails.Extensions.Add("errors", errors);
                }

                return new BadRequestObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json" }
                };
            };
        });

        return services;
    }
}

