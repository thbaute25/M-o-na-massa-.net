using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MaoNaMassa.API.Pages.Cidade;

public class ServicosModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Cidade { get; set; } = string.Empty;

    public void OnGet()
    {
    }
}

