using MaoNaMassa.Domain.Exceptions;

namespace MaoNaMassa.Domain.Entities;

public class Usuario
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string Senha { get; private set; }
    public string Cidade { get; private set; }
    public string AreaInteresse { get; private set; }
    public string TipoUsuario { get; private set; }
    public DateTime DataCriacao { get; private set; }
    
    // Navegação
    public virtual ICollection<RespostaQuiz> RespostasQuiz { get; private set; } = new List<RespostaQuiz>();
    public virtual ICollection<Certificado> Certificados { get; private set; } = new List<Certificado>();
    public virtual Profissional? Profissional { get; private set; }
    public virtual ICollection<Avaliacao> Avaliacoes { get; private set; } = new List<Avaliacao>();

    // Construtor privado para EF Core
    private Usuario() { }

    public Usuario(
        string nome,
        string email,
        string senha,
        string cidade,
        string areaInteresse,
        string tipoUsuario)
    {
        Id = Guid.NewGuid();
        DataCriacao = DateTime.UtcNow;
        
        SetNome(nome);
        SetEmail(email);
        SetSenha(senha);
        SetCidade(cidade);
        SetAreaInteresse(areaInteresse);
        SetTipoUsuario(tipoUsuario);
    }

    // Métodos para garantir invariantes
    public void SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome do usuário é obrigatório.");
        
        if (nome.Length < 3)
            throw new DomainException("Nome deve ter no mínimo 3 caracteres.");
        
        if (nome.Length > 100)
            throw new DomainException("Nome deve ter no máximo 100 caracteres.");

        Nome = nome.Trim();
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email é obrigatório.");
        
        if (!email.Contains("@") || !email.Contains("."))
            throw new DomainException("Email inválido.");
        
        if (email.Length > 255)
            throw new DomainException("Email deve ter no máximo 255 caracteres.");

        Email = email.Trim().ToLowerInvariant();
    }

    public void SetSenha(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            throw new DomainException("Senha é obrigatória.");
        
        if (senha.Length < 6)
            throw new DomainException("Senha deve ter no mínimo 6 caracteres.");

        Senha = senha; // Em produção, aplicar hash aqui
    }

    public void SetCidade(string cidade)
    {
        if (string.IsNullOrWhiteSpace(cidade))
            throw new DomainException("Cidade é obrigatória.");
        
        if (cidade.Length > 100)
            throw new DomainException("Cidade deve ter no máximo 100 caracteres.");

        Cidade = cidade.Trim();
    }

    public void SetAreaInteresse(string areaInteresse)
    {
        if (string.IsNullOrWhiteSpace(areaInteresse))
            throw new DomainException("Área de interesse é obrigatória.");
        
        if (areaInteresse.Length > 100)
            throw new DomainException("Área de interesse deve ter no máximo 100 caracteres.");

        AreaInteresse = areaInteresse.Trim();
    }

    public void SetTipoUsuario(string tipoUsuario)
    {
        if (string.IsNullOrWhiteSpace(tipoUsuario))
            throw new DomainException("Tipo de usuário é obrigatório.");
        
        var tiposValidos = new[] { "Aprendiz", "Cliente", "Empresa", "Profissional" };
        if (!tiposValidos.Contains(tipoUsuario))
            throw new DomainException($"Tipo de usuário inválido. Tipos válidos: {string.Join(", ", tiposValidos)}.");

        TipoUsuario = tipoUsuario;
    }

    public void AtualizarPerfil(string nome, string cidade, string areaInteresse)
    {
        SetNome(nome);
        SetCidade(cidade);
        SetAreaInteresse(areaInteresse);
    }
}

