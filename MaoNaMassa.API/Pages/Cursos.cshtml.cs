using Microsoft.AspNetCore.Mvc.RazorPages;
using MaoNaMassa.Domain.Interfaces;
using MaoNaMassa.API.ViewModels;

namespace MaoNaMassa.API.Pages;

public class CursosModel : PageModel
{
    private readonly ICursoRepository _cursoRepository;

    public CursosModel(ICursoRepository cursoRepository)
    {
        _cursoRepository = cursoRepository;
    }

    public List<CursoViewModel> Cursos { get; set; } = new();

    public async Task OnGetAsync()
    {
        var cursos = await _cursoRepository.GetAllAsync();
        
        Cursos = cursos.Select(c => new CursoViewModel
        {
            Id = c.Id,
            Titulo = c.Titulo,
            Descricao = c.Descricao,
            Area = c.Area,
            Nivel = c.Nivel,
            DataCriacao = c.DataCriacao,
            TotalAulas = 0, // Será carregado quando necessário
            TotalQuizzes = 0, // Será carregado quando necessário
            TotalCertificadosEmitidos = 0 // Será carregado quando necessário
        }).ToList();
    }
}

