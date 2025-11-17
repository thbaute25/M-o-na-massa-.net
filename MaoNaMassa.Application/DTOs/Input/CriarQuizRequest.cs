namespace MaoNaMassa.Application.DTOs.Input;

/// <summary>
/// DTO de entrada para criar um novo quiz
/// </summary>
public class CriarQuizRequest
{
    public Guid CursoId { get; set; }
    public string Pergunta { get; set; } = string.Empty;
    public string RespostaCorreta { get; set; } = string.Empty;
}

