namespace MaoNaMassa.Application.DTOs.Output;

/// <summary>
/// DTO de saída para resultado da resposta do quiz
/// </summary>
public class ResultadoQuizResponse
{
    public Guid RespostaQuizId { get; set; }
    public bool Correta { get; set; }
    public string RespostaEnviada { get; set; } = string.Empty;
    public string RespostaCorreta { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
}

