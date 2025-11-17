using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MaoNaMassa.API.ViewModels;
using MaoNaMassa.Application.UseCases;
using MaoNaMassa.Application.DTOs.Input;
using MaoNaMassa.Domain.Interfaces;

namespace MaoNaMassa.API.Pages.Avaliacao;

public class CriarModel : PageModel
{
    private readonly AvaliarServicoUseCase _avaliarServicoUseCase;
    private readonly IServicoRepository _servicoRepository;

    public CriarModel(
        AvaliarServicoUseCase avaliarServicoUseCase,
        IServicoRepository servicoRepository)
    {
        _avaliarServicoUseCase = avaliarServicoUseCase;
        _servicoRepository = servicoRepository;
    }

    [BindProperty]
    public AvaliarServicoViewModel ViewModel { get; set; } = new();

    public string? MensagemErro { get; set; }
    public string? MensagemSucesso { get; set; }

    public async Task OnGetAsync(Guid? servicoId = null, Guid? usuarioId = null)
    {
        if (servicoId.HasValue)
        {
            ViewModel.ServicoId = servicoId.Value;
            var servico = await _servicoRepository.GetByIdAsync(servicoId.Value);
            if (servico != null)
            {
                ViewModel.ServicoTitulo = servico.Titulo;
                ViewModel.ProfissionalNome = "Profissional";
            }
        }

        if (usuarioId.HasValue)
        {
            ViewModel.UsuarioId = usuarioId.Value;
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            // Recarregar dados do serviço se necessário
            if (ViewModel.ServicoId != Guid.Empty)
            {
                var servico = await _servicoRepository.GetByIdAsync(ViewModel.ServicoId);
                if (servico != null)
                {
                    ViewModel.ServicoTitulo = servico.Titulo;
                }
            }
            return Page();
        }

        try
        {
            var dto = new MaoNaMassa.Application.DTOs.CreateAvaliacaoDTO
            {
                UsuarioId = ViewModel.UsuarioId,
                ServicoId = ViewModel.ServicoId,
                Nota = ViewModel.Nota,
                Comentario = ViewModel.Comentario
            };

            var avaliacao = await _avaliarServicoUseCase.ExecuteAsync(dto);

            MensagemSucesso = $"Avaliação enviada com sucesso! Nota: {avaliacao.Nota}/5";
            
            // Limpar formulário
            ViewModel.Comentario = string.Empty;
            ViewModel.Nota = 0;
            
            return Page();
        }
        catch (Exception ex)
        {
            MensagemErro = ex.Message;
            return Page();
        }
    }
}

