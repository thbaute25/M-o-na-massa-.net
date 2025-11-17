using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MaoNaMassa.API.Pages.Servico;

public class DetalhesModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid ServicoId { get; set; }

    public void OnGet()
    {
    }
}

