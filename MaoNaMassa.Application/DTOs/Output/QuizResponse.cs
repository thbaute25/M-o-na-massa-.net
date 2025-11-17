namespace MaoNaMassa.Application.DTOs.Output;

/// <summary>
/// DTO de saída para informações do quiz
/// </summary>
public class QuizResponse
{
    public Guid Id { get; set; }
    public Guid CursoId { get; set; }
    public string Pergunta { get; set; } = string.Empty;
    public string? RespostaCorreta { get; set; } // Null se usuário ainda não respondeu
    public bool? JaRespondeu { get; set; }
    public bool? RespostaFoiCorreta { get; set; }
}

