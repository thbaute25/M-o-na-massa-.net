using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MaoNaMassa.API.Pages;

public class BuscarModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Termo { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Tipo { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Cidade { get; set; }

    public void OnGet()
    {
    }
}

