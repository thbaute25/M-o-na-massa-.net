using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MaoNaMassa.Domain.Interfaces;
using MaoNaMassa.API.ViewModels;

namespace MaoNaMassa.API.Pages.Curso;

public class DetalhesModel : PageModel
{
    private readonly ICursoRepository _cursoRepository;

    public DetalhesModel(ICursoRepository cursoRepository)
    {
        _cursoRepository = cursoRepository;
    }

    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    public CursoViewModel? Curso { get; set; }
    public string? MensagemErro { get; set; }
    public bool CursoCriado { get; set; }

    public async Task OnGetAsync()
    {
        // Verificar se há parâmetro de sucesso na query string
        CursoCriado = Request.Query.ContainsKey("criado") && Request.Query["criado"] == "true";

        if (Id == Guid.Empty)
        {
            MensagemErro = "ID do curso não fornecido.";
            return;
        }

        var curso = await _cursoRepository.GetByIdAsync(Id);

        if (curso == null)
        {
            MensagemErro = "Curso não encontrado.";
            return;
        }

        Curso = new CursoViewModel
        {
            Id = curso.Id,
            Titulo = curso.Titulo,
            Descricao = curso.Descricao,
            Area = curso.Area,
            Nivel = curso.Nivel,
            DataCriacao = curso.DataCriacao,
            TotalAulas = curso.Aulas?.Count ?? 0,
            TotalQuizzes = curso.Quizzes?.Count ?? 0,
            TotalCertificadosEmitidos = curso.Certificados?.Count ?? 0
        };
    }
}

