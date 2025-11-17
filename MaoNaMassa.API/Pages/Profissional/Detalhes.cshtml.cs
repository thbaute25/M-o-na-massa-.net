using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MaoNaMassa.API.Pages.Profissional;

public class DetalhesModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid ProfissionalId { get; set; }

    public void OnGet()
    {
    }
}

