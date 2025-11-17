using MaoNaMassa.Application.DTOs.Output;
using MaoNaMassa.Application.DTOs.Paginacao;

namespace MaoNaMassa.API.ViewModels.Mappers;

/// <summary>
/// Classe auxiliar para mapear DTOs da camada Application para ViewModels da camada de apresentação
/// </summary>
public static class ViewModelMapper
{
    // Mapeamento de Usuario
    public static UsuarioViewModel ToViewModel(this UsuarioResponse dto)
    {
        return new UsuarioViewModel
        {
            Id = dto.Id,
            Nome = dto.Nome,
            Email = dto.Email,
            Cidade = dto.Cidade,
            AreaInteresse = dto.AreaInteresse,
            TipoUsuario = dto.TipoUsuario,
            DataCriacao = dto.DataCriacao,
            TemPerfilProfissional = dto.TemPerfilProfissional,
            TotalCertificados = dto.TotalCertificados
        };
    }

    public static List<UsuarioViewModel> ToViewModelList(this IEnumerable<UsuarioResponse> dtos)
    {
        return dtos.Select(ToViewModel).ToList();
    }

    // Mapeamento de Curso
    public static CursoViewModel ToViewModel(this CursoResponse dto)
    {
        return new CursoViewModel
        {
            Id = dto.Id,
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            Area = dto.Area,
            Nivel = dto.Nivel,
            DataCriacao = dto.DataCriacao,
            TotalAulas = dto.TotalAulas,
            TotalQuizzes = dto.TotalQuizzes,
            TotalCertificadosEmitidos = dto.TotalCertificadosEmitidos
        };
    }

    public static List<CursoViewModel> ToViewModelList(this IEnumerable<CursoResponse> dtos)
    {
        return dtos.Select(ToViewModel).ToList();
    }

    public static CursoCompletoViewModel ToCompletoViewModel(this CursoCompletoResponse dto)
    {
        return new CursoCompletoViewModel
        {
            Id = dto.Id,
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            Area = dto.Area,
            Nivel = dto.Nivel,
            DataCriacao = dto.DataCriacao,
            TotalAulas = dto.TotalAulas,
            TotalQuizzes = dto.TotalQuizzes,
            TotalCertificadosEmitidos = dto.TotalCertificadosEmitidos,
            Aulas = dto.Aulas.Select(a => new AulaViewModel
            {
                Id = a.Id,
                CursoId = a.CursoId,
                Titulo = a.Titulo,
                Conteudo = a.Conteudo,
                Ordem = a.Ordem
            }).ToList(),
            Quizzes = dto.Quizzes.Select(q => new QuizViewModel
            {
                Id = q.Id,
                CursoId = q.CursoId,
                Pergunta = q.Pergunta,
                RespostaCorreta = q.RespostaCorreta ?? string.Empty
            }).ToList()
        };
    }

    // Mapeamento de Servico
    public static ServicoViewModel ToViewModel(this ServicoResponse dto)
    {
        return new ServicoViewModel
        {
            Id = dto.Id,
            ProfissionalId = dto.ProfissionalId,
            NomeProfissional = dto.NomeProfissional,
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            Cidade = dto.Cidade,
            Preco = dto.Preco,
            DataPublicacao = dto.DataPublicacao,
            AvaliacaoMedia = dto.AvaliacaoMedia,
            TotalAvaliacoes = dto.TotalAvaliacoes
        };
    }

    public static List<ServicoViewModel> ToViewModelList(this IEnumerable<ServicoResponse> dtos)
    {
        return dtos.Select(ToViewModel).ToList();
    }

    // Mapeamento de Profissional
    public static ProfissionalViewModel ToViewModel(this ProfissionalResponse dto)
    {
        return new ProfissionalViewModel
        {
            Id = dto.Id,
            UsuarioId = dto.UsuarioId,
            NomeUsuario = dto.NomeUsuario,
            EmailUsuario = dto.EmailUsuario,
            CidadeUsuario = dto.CidadeUsuario,
            Descricao = dto.Descricao,
            AvaliacaoMedia = dto.AvaliacaoMedia,
            Disponivel = dto.Disponivel,
            TotalServicos = dto.TotalServicos,
            TotalAvaliacoes = dto.TotalAvaliacoes
        };
    }

    public static List<ProfissionalViewModel> ToViewModelList(this IEnumerable<ProfissionalResponse> dtos)
    {
        return dtos.Select(ToViewModel).ToList();
    }

    // Mapeamento de Certificado
    public static CertificadoViewModel ToViewModel(this CertificadoResponse dto)
    {
        return new CertificadoViewModel
        {
            Id = dto.Id,
            UsuarioId = dto.UsuarioId,
            CursoId = dto.CursoId,
            NomeUsuario = dto.NomeUsuario,
            TituloCurso = dto.TituloCurso,
            NotaFinal = dto.NotaFinal,
            DataConclusao = dto.DataConclusao,
            CodigoCertificado = dto.CodigoCertificado,
            Aprovado = dto.Aprovado
        };
    }

    public static List<CertificadoViewModel> ToViewModelList(this IEnumerable<CertificadoResponse> dtos)
    {
        return dtos.Select(ToViewModel).ToList();
    }

    // Mapeamento de Avaliacao
    public static AvaliacaoViewModel ToViewModel(this AvaliacaoResponse dto)
    {
        return new AvaliacaoViewModel
        {
            Id = dto.Id,
            ServicoId = dto.ServicoId,
            UsuarioId = dto.UsuarioId,
            NomeUsuario = dto.NomeUsuario,
            TituloServico = string.Empty,
            Nota = dto.Nota,
            Comentario = dto.Comentario,
            Data = dto.Data
        };
    }

    public static AvaliacaoViewModel ToViewModel(this AvaliacaoBasicaResponse dto, Guid servicoId, Guid usuarioId)
    {
        return new AvaliacaoViewModel
        {
            Id = dto.Id,
            ServicoId = servicoId,
            UsuarioId = usuarioId,
            NomeUsuario = dto.NomeUsuario,
            TituloServico = string.Empty,
            Nota = dto.Nota,
            Comentario = dto.Comentario,
            Data = dto.Data
        };
    }

    public static List<AvaliacaoViewModel> ToViewModelList(this IEnumerable<AvaliacaoResponse> dtos)
    {
        return dtos.Select(ToViewModel).ToList();
    }

    // Mapeamento de Paginação
    public static PaginacaoViewModel ToViewModel<T>(this PaginacaoResponse<T> dto)
    {
        return new PaginacaoViewModel
        {
            PaginaAtual = dto.PaginaAtual,
            ItensPorPagina = dto.TamanhoPagina,
            TotalItens = dto.TotalItens,
            TotalPaginas = dto.TotalPaginas,
            TemPaginaAnterior = dto.TemPaginaAnterior,
            TemProximaPagina = dto.TemProximaPagina
        };
    }

    public static PaginacaoResponseViewModel<TViewModel> ToViewModel<TDto, TViewModel>(
        this PaginacaoResponse<TDto> dto,
        Func<TDto, TViewModel> mapper)
    {
        return new PaginacaoResponseViewModel<TViewModel>
        {
            Itens = dto.Itens.Select(mapper).ToList(),
            Paginacao = dto.ToViewModel()
        };
    }
}

