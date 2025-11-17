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

            // Redirecionar para a página de detalhes do curso criado com mensagem de sucesso
            return RedirectToPage("/Curso/Detalhes", new { id = curso.Id, criado = true });
        }
        catch (Exception ex)
        {
            // Capturar exceção interna para diagnóstico
            var mensagemErro = ex.Message;
            if (ex.InnerException != null)
            {
                mensagemErro += $" | Detalhes: {ex.InnerException.Message}";
            }
            
            MensagemErro = mensagemErro;
            
            // Log do erro completo para diagnóstico
            Console.WriteLine($"Erro ao criar curso: {ex}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Exceção interna: {ex.InnerException}");
            }
            
            return Page();
        }
    }
}

