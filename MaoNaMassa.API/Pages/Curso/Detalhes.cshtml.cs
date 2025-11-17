using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MaoNaMassa.API.Pages.Curso;

public class DetalhesModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid CursoId { get; set; }

    public void OnGet()
    {
    }
}

