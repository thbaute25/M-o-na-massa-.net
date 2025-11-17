namespace MaoNaMassa.API.ViewModels;

/// <summary>
/// ViewModel para a página inicial com estatísticas e informações gerais
/// </summary>
public class HomeViewModel
{
    public int TotalCursos { get; set; }
    public int TotalUsuarios { get; set; }
    public int TotalProfissionais { get; set; }
    public int TotalServicos { get; set; }
    public int TotalCertificadosEmitidos { get; set; }

    public List<CursoViewModel> CursosRecentes { get; set; } = new();
    public List<ServicoViewModel> ServicosRecentes { get; set; } = new();
    public List<ProfissionalViewModel> ProfissionaisDestaque { get; set; } = new();
}

