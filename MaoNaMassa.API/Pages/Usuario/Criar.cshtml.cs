using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MaoNaMassa.API.ViewModels;
using MaoNaMassa.Application.DTOs.Input;
using MaoNaMassa.Domain.Interfaces;

namespace MaoNaMassa.API.Pages.Usuario;

public class CriarModel : PageModel
{
    private readonly IUsuarioRepository _usuarioRepository;

    public CriarModel(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    [BindProperty]
    public CriarUsuarioViewModel ViewModel { get; set; } = new();

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
            var request = new CriarUsuarioRequest
            {
                Nome = ViewModel.Nome,
                Email = ViewModel.Email,
                Senha = ViewModel.Senha,
                Cidade = ViewModel.Cidade,
                AreaInteresse = ViewModel.AreaInteresse,
                TipoUsuario = ViewModel.TipoUsuario
            };

            var usuario = new Domain.Entities.Usuario(
                request.Nome,
                request.Email,
                request.Senha,
                request.Cidade,
                request.AreaInteresse,
                request.TipoUsuario
            );

            await _usuarioRepository.CreateAsync(usuario);

            MensagemSucesso = $"Usuário '{usuario.Nome}' criado com sucesso! ID: {usuario.Id}";
            ModelState.Clear();
            ViewModel = new CriarUsuarioViewModel();
            
            return Page();
        }
        catch (Exception ex)
        {
            MensagemErro = ex.Message;
            return Page();
        }
    }
}

