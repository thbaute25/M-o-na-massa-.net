using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MaoNaMassa.API.ViewModels;
using MaoNaMassa.Application.DTOs.Input;
using MaoNaMassa.Domain.Interfaces;

namespace MaoNaMassa.API.Pages.Curso;

public class CriarModel : PageModel
{
    private readonly ICursoRepository _cursoRepository;

    public CriarModel(ICursoRepository cursoRepository)
    {
        _cursoRepository = cursoRepository;
    }

    [BindProperty]
    public CriarCursoViewModel ViewModel { get; set; } = new();

    public string? MensagemErro { get; set; }
    public string? MensagemSucesso { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var curso = new Domain.Entities.Curso(
                ViewModel.Titulo,
                ViewModel.Descricao,
                ViewModel.Area,
                ViewModel.Nivel
            );

            await _cursoRepository.CreateAsync(curso);

            MensagemSucesso = $"Curso '{curso.Titulo}' criado com sucesso! ID: {curso.Id}";
            ModelState.Clear();
            ViewModel = new CriarCursoViewModel();
            
            return Page();
        }
        catch (Exception ex)
        {
            MensagemErro = ex.Message;
            return Page();
        }
    }
}

