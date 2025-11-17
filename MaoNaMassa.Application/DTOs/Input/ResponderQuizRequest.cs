namespace MaoNaMassa.Application.DTOs.Input;

/// <summary>
/// DTO de entrada para responder um quiz
/// </summary>
public class ResponderQuizRequest
{
    public Guid QuizId { get; set; }
    public string Resposta { get; set; } = string.Empty;
}

