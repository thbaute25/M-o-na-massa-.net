using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MaoNaMassa.API.ViewModels;
using MaoNaMassa.Application.UseCases;
using MaoNaMassa.Application.DTOs.Input;

namespace MaoNaMassa.API.Pages.Servico;

public class CriarModel : PageModel
{
    private readonly CriarServicoUseCase _criarServicoUseCase;

    public CriarModel(CriarServicoUseCase criarServicoUseCase)
    {
        _criarServicoUseCase = criarServicoUseCase;
    }

    [BindProperty]
    public CriarServicoViewModel ViewModel { get; set; } = new();

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
            var dto = new MaoNaMassa.Application.DTOs.CreateServicoDTO
            {
                ProfissionalId = ViewModel.ProfissionalId,
                Titulo = ViewModel.Titulo,
                Descricao = ViewModel.Descricao,
                Cidade = ViewModel.Cidade,
                Preco = ViewModel.Preco
            };

            var servico = await _criarServicoUseCase.ExecuteAsync(dto);

            MensagemSucesso = $"Serviço '{servico.Titulo}' criado com sucesso! ID: {servico.Id}";
            ModelState.Clear();
            ViewModel = new CriarServicoViewModel();
            
            return Page();
        }
        catch (Exception ex)
        {
            MensagemErro = ex.Message;
            return Page();
        }
    }
}

