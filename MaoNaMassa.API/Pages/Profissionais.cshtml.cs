using Microsoft.AspNetCore.Mvc.RazorPages;
using MaoNaMassa.Domain.Interfaces;
using MaoNaMassa.API.ViewModels;

namespace MaoNaMassa.API.Pages;

public class ProfissionaisModel : PageModel
{
    private readonly IProfissionalRepository _profissionalRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public ProfissionaisModel(
        IProfissionalRepository profissionalRepository,
        IUsuarioRepository usuarioRepository)
    {
        _profissionalRepository = profissionalRepository;
        _usuarioRepository = usuarioRepository;
    }

    public List<ProfissionalViewModel> Profissionais { get; set; } = new();

    public async Task OnGetAsync()
    {
        var profissionais = await _profissionalRepository.GetAllAsync();
        
        Profissionais = new List<ProfissionalViewModel>();
        
        foreach (var profissional in profissionais)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(profissional.UsuarioId);
            if (usuario != null)
            {
                Profissionais.Add(new ProfissionalViewModel
                {
                    Id = profissional.Id,
                    UsuarioId = profissional.UsuarioId,
                    NomeUsuario = usuario.Nome,
                    EmailUsuario = usuario.Email,
                    CidadeUsuario = usuario.Cidade,
                    AreaInteresse = usuario.AreaInteresse,
                    Descricao = profissional.Descricao,
                    AvaliacaoMedia = profissional.AvaliacaoMedia,
                    Disponivel = profissional.Disponivel,
                    TotalServicos = profissional.Servicos?.Count ?? 0,
                    TotalAvaliacoes = profissional.Servicos?.SelectMany(s => s.Avaliacoes).Count() ?? 0
                });
            }
        }
    }
}

