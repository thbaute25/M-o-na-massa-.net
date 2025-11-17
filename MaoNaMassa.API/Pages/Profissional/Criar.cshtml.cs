using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MaoNaMassa.API.ViewModels;
using MaoNaMassa.Application.UseCases;
using MaoNaMassa.Application.DTOs.Input;

namespace MaoNaMassa.API.Pages.Profissional;

public class CriarModel : PageModel
{
    private readonly CriarPerfilProfissionalUseCase _criarPerfilUseCase;

    public CriarModel(CriarPerfilProfissionalUseCase criarPerfilUseCase)
    {
        _criarPerfilUseCase = criarPerfilUseCase;
    }

    [BindProperty]
    public CriarPerfilProfissionalViewModel ViewModel { get; set; } = new();

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
            var dto = new MaoNaMassa.Application.DTOs.CreateProfissionalDTO
            {
                UsuarioId = ViewModel.UsuarioId,
                Descricao = ViewModel.Descricao
            };

            var profissional = await _criarPerfilUseCase.ExecuteAsync(dto);

            MensagemSucesso = $"Perfil profissional criado com sucesso! ID: {profissional.Id}";
            ModelState.Clear();
            ViewModel = new CriarPerfilProfissionalViewModel();
            
            return Page();
        }
        catch (Exception ex)
        {
            MensagemErro = ex.Message;
            return Page();
        }
    }
}

