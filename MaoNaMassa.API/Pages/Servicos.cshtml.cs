using Microsoft.AspNetCore.Mvc.RazorPages;
using MaoNaMassa.Domain.Interfaces;
using MaoNaMassa.API.ViewModels;

namespace MaoNaMassa.API.Pages;

public class ServicosModel : PageModel
{
    private readonly IServicoRepository _servicoRepository;
    private readonly IProfissionalRepository _profissionalRepository;

    public ServicosModel(
        IServicoRepository servicoRepository,
        IProfissionalRepository profissionalRepository)
    {
        _servicoRepository = servicoRepository;
        _profissionalRepository = profissionalRepository;
    }

    public List<ServicoViewModel> Servicos { get; set; } = new();

    public async Task OnGetAsync()
    {
        var servicos = await _servicoRepository.GetAllAsync();
        
        Servicos = new List<ServicoViewModel>();
        
        foreach (var servico in servicos)
        {
            var profissional = await _profissionalRepository.GetByIdAsync(servico.ProfissionalId);
            
            // Calcular avaliação média manualmente se houver avaliações
            decimal? avaliacaoMedia = null;
            int totalAvaliacoes = 0;
            
            if (servico.Avaliacoes != null && servico.Avaliacoes.Any())
            {
                totalAvaliacoes = servico.Avaliacoes.Count;
                avaliacaoMedia = (decimal?)servico.Avaliacoes.Average(a => a.Nota);
            }

            Servicos.Add(new ServicoViewModel
            {
                Id = servico.Id,
                ProfissionalId = servico.ProfissionalId,
                NomeProfissional = profissional?.Usuario?.Nome ?? "Profissional não encontrado",
                Titulo = servico.Titulo,
                Descricao = servico.Descricao,
                Cidade = servico.Cidade,
                Preco = servico.Preco,
                DataPublicacao = servico.DataPublicacao,
                AvaliacaoMedia = avaliacaoMedia,
                TotalAvaliacoes = totalAvaliacoes
            });
        }

        // Ordenar por data de publicação (mais recentes primeiro)
        Servicos = Servicos.OrderByDescending(s => s.DataPublicacao).ToList();
    }
}

