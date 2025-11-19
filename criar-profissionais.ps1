# Script para criar usuários e perfis profissionais
# Execute este script para popular a aba "Profissionais"

Write-Host "=== Criando Usuários e Perfis Profissionais ===" -ForegroundColor Cyan
Write-Host ""

$baseUrl = "http://localhost:5136/api"

# Lista de profissionais para criar
$profissionais = @(
    @{
        Nome = "Carlos Eletricista"
        Email = "carlos.eletricista@email.com"
        Senha = "senha123"
        Cidade = "São Paulo"
        AreaInteresse = "Elétrica"
        TipoUsuario = "Profissional"
        Descricao = "Eletricista com 10 anos de experiência em instalações residenciais e comerciais. Especializado em instalações elétricas completas, manutenção e reparos."
    },
    @{
        Nome = "Ana Pintora"
        Email = "ana.pintora@email.com"
        Senha = "senha123"
        Cidade = "Rio de Janeiro"
        AreaInteresse = "Pintura"
        TipoUsuario = "Profissional"
        Descricao = "Pintora profissional com experiência em pintura residencial e comercial. Trabalho com diferentes tipos de acabamento e técnicas profissionais."
    },
    @{
        Nome = "Roberto Encanador"
        Email = "roberto.encanador@email.com"
        Senha = "senha123"
        Cidade = "Brasília"
        AreaInteresse = "Encanamento"
        TipoUsuario = "Profissional"
        Descricao = "Encanador experiente em instalações hidráulicas residenciais e comerciais. Resolvo problemas de vazamento, instalação e manutenção."
    },
    @{
        Nome = "Fernanda Marceneira"
        Email = "fernanda.marceneira@email.com"
        Senha = "senha123"
        Cidade = "Curitiba"
        AreaInteresse = "Marcenaria"
        TipoUsuario = "Profissional"
        Descricao = "Marceneira especializada em móveis sob medida e reformas. Trabalho com diferentes tipos de madeira e técnicas de acabamento."
    },
    @{
        Nome = "João Pedreiro"
        Email = "joao.pedreiro@email.com"
        Senha = "senha123"
        Cidade = "São Paulo"
        AreaInteresse = "Construção"
        TipoUsuario = "Profissional"
        Descricao = "Pedreiro com experiência em alvenaria, reboco, contrapiso e acabamentos. Trabalho em obras residenciais e comerciais."
    }
)

$usuariosCriados = @()

# Criar usuários
Write-Host "Criando usuários..." -ForegroundColor Yellow
foreach ($prof in $profissionais) {
    try {
        $body = @{
            nome = $prof.Nome
            email = $prof.Email
            senha = $prof.Senha
            cidade = $prof.Cidade
            areaInteresse = $prof.AreaInteresse
            tipoUsuario = $prof.TipoUsuario
        } | ConvertTo-Json

        $response = Invoke-RestMethod -Uri "$baseUrl/usuarios" `
            -Method POST `
            -ContentType "application/json" `
            -Body $body

        $usuariosCriados += @{
            Id = $response.data.id
            Nome = $prof.Nome
            Descricao = $prof.Descricao
        }

        Write-Host "  ✓ Usuário criado: $($prof.Nome)" -ForegroundColor Green
    }
    catch {
        Write-Host "  ✗ Erro ao criar usuário $($prof.Nome): $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "Criando perfis profissionais..." -ForegroundColor Yellow

# Criar perfis profissionais
foreach ($usuario in $usuariosCriados) {
    try {
        $body = @{
            descricao = $usuario.Descricao
        } | ConvertTo-Json

        $response = Invoke-RestMethod -Uri "$baseUrl/profissionais?usuarioId=$($usuario.Id)" `
            -Method POST `
            -ContentType "application/json" `
            -Body $body

        Write-Host "  ✓ Perfil profissional criado para: $($usuario.Nome)" -ForegroundColor Green
    }
    catch {
        Write-Host "  ✗ Erro ao criar perfil para $($usuario.Nome): $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "=== Concluído! ===" -ForegroundColor Cyan
Write-Host "Acesse http://localhost:5136/Profissionais para ver os profissionais criados" -ForegroundColor Green
