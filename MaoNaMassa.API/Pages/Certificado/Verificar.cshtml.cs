using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MaoNaMassa.API.Pages.Certificado;

public class VerificarModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Codigo { get; set; } = string.Empty;

    public void OnGet()
    {
    }
}

