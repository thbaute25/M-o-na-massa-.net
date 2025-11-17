using System.ComponentModel.DataAnnotations;

namespace MaoNaMassa.API.ViewModels;

/// <summary>
/// ViewModel para exibição de dados do curso
/// </summary>
public class CursoViewModel
{
    public Guid Id { get; set; }

    [Display(Name = "Título")]
    public string Titulo { get; set; } = string.Empty;

    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = string.Empty;

    [Display(Name = "Área")]
    public string Area { get; set; } = string.Empty;

    [Display(Name = "Nível")]
    public string Nivel { get; set; } = string.Empty;

    [Display(Name = "Data de Criação")]
    [DataType(DataType.Date)]
    public DateTime DataCriacao { get; set; }

    [Display(Name = "Total de Aulas")]
    public int TotalAulas { get; set; }

    [Display(Name = "Total de Quizzes")]
    public int TotalQuizzes { get; set; }

    [Display(Name = "Certificados Emitidos")]
    public int TotalCertificadosEmitidos { get; set; }
}

/// <summary>
/// ViewModel para exibição completa do curso (com aulas e quizzes)
/// </summary>
public class CursoCompletoViewModel : CursoViewModel
{
    public List<AulaViewModel> Aulas { get; set; } = new();
    public List<QuizViewModel> Quizzes { get; set; } = new();
}

/// <summary>
/// ViewModel para busca/filtro de cursos
/// </summary>
public class BuscarCursoViewModel
{
    [Display(Name = "Título")]
    public string? Titulo { get; set; }

    [Display(Name = "Área")]
    public string? Area { get; set; }

    [Display(Name = "Nível")]
    public string? Nivel { get; set; }

    [Display(Name = "Página")]
    [Range(1, int.MaxValue, ErrorMessage = "A página deve ser maior que zero")]
    public int Pagina { get; set; } = 1;

    [Display(Name = "Itens por Página")]
    [Range(1, 100, ErrorMessage = "Os itens por página devem estar entre 1 e 100")]
    public int ItensPorPagina { get; set; } = 10;

    [Display(Name = "Ordenar por")]
    public string? OrdenarPor { get; set; } = "DataCriacao";

    [Display(Name = "Ordem")]
    public string? Ordem { get; set; } = "desc";
}

/// <summary>
/// ViewModel para exibição de aula
/// </summary>
public class AulaViewModel
{
    public Guid Id { get; set; }
    public Guid CursoId { get; set; }

    [Display(Name = "Título")]
    public string Titulo { get; set; } = string.Empty;

    [Display(Name = "Conteúdo")]
    public string Conteudo { get; set; } = string.Empty;

    [Display(Name = "Ordem")]
    public int Ordem { get; set; }
}

/// <summary>
/// ViewModel para exibição de quiz
/// </summary>
public class QuizViewModel
{
    public Guid Id { get; set; }
    public Guid CursoId { get; set; }

    [Display(Name = "Pergunta")]
    public string Pergunta { get; set; } = string.Empty;

    [Display(Name = "Resposta Correta")]
    public string RespostaCorreta { get; set; } = string.Empty;
}

