using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MaoNaMassa.API.Pages.Area;

public class CursosModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Area { get; set; } = string.Empty;

    public void OnGet()
    {
    }
}

