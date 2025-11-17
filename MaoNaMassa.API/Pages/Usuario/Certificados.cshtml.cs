using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MaoNaMassa.API.Pages.Usuario;

public class CertificadosModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid UsuarioId { get; set; }

    public void OnGet()
    {
    }
}

