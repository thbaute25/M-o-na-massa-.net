# Script simples para criar serviços
$baseUrl = "http://localhost:5136/api"

# Aguardar API estar pronta
Write-Host "Aguardando API estar pronta..." -ForegroundColor Yellow
$tentativas = 0
do {
    Start-Sleep -Seconds 2
    $tentativas++
    try {
        $health = Invoke-RestMethod -Uri "$baseUrl/health" -UseBasicParsing -TimeoutSec 2
        Write-Host "API pronta!" -ForegroundColor Green
        break
    } catch {
        if ($tentativas -ge 10) {
            Write-Host "Timeout aguardando API" -ForegroundColor Red
            exit
        }
    }
} while ($true)

# Buscar profissionais
Write-Host "`nBuscando profissionais..." -ForegroundColor Cyan
$profs = Invoke-RestMethod -Uri "$baseUrl/profissionais?pagina=1&tamanhoPagina=20"

# Mapear profissionais
$map = @{}
foreach ($p in $profs.data.itens) {
    $map[$p.nomeUsuario] = $p.id
}

# Serviços para criar
$servicos = @(
    @{ Profissional = "Carlos Eletricista"; Titulo = "Instalacao de Chuveiro Eletrico"; Descricao = "Instalacao completa de chuveiro eletrico com seguranca e garantia. Inclui fiacao, disjuntor e teste de funcionamento."; Cidade = "Sao Paulo"; Preco = 250.00 },
    @{ Profissional = "Carlos Eletricista"; Titulo = "Instalacao de Tomadas e Interruptores"; Descricao = "Instalacao de tomadas e interruptores em toda a residencia. Trabalho limpo e organizado."; Cidade = "Sao Paulo"; Preco = 150.00 },
    @{ Profissional = "Ana Pintora"; Titulo = "Pintura de Parede Interna"; Descricao = "Pintura profissional de paredes internas com preparacao de superficie, massa corrida e acabamento impecavel."; Cidade = "Rio de Janeiro"; Preco = 35.00 },
    @{ Profissional = "Ana Pintora"; Titulo = "Pintura Externa com Impermeabilizacao"; Descricao = "Pintura externa com tratamento anti-umidade e impermeabilizacao. Garantia contra infiltracoes."; Cidade = "Rio de Janeiro"; Preco = 45.00 },
    @{ Profissional = "Roberto Encanador"; Titulo = "Desentupimento de Esgoto"; Descricao = "Desentupimento rapido e eficiente de esgotos e ralos. Equipamentos modernos e garantia de servico."; Cidade = "Brasilia"; Preco = 120.00 },
    @{ Profissional = "Roberto Encanador"; Titulo = "Instalacao de Torneira e Registro"; Descricao = "Instalacao de torneiras e registros com vedacao adequada. Sem vazamentos garantidos."; Cidade = "Brasilia"; Preco = 80.00 },
    @{ Profissional = "Fernanda Marceneira"; Titulo = "Moveis Sob Medida"; Descricao = "Fabricacao de moveis sob medida para sua casa. Design personalizado e acabamento profissional."; Cidade = "Curitiba"; Preco = 500.00 },
    @{ Profissional = "Fernanda Marceneira"; Titulo = "Instalacao de Prateleiras e Estantes"; Descricao = "Instalacao de prateleiras e estantes com fixacao segura. Ideal para organizacao de espacos."; Cidade = "Curitiba"; Preco = 100.00 },
    @{ Profissional = "Joao Silva"; Titulo = "Reparo de Instalacao Eletrica"; Descricao = "Reparo e manutencao de instalacoes eletricas residenciais. Aprendiz em busca de experiencia."; Cidade = "Sao Paulo"; Preco = 80.00 },
    @{ Profissional = "Maria Santos"; Titulo = "Pintura de Portas e Janelas"; Descricao = "Pintura e acabamento de portas e janelas. Trabalho cuidadoso e detalhado."; Cidade = "Rio de Janeiro"; Preco = 60.00 }
)

Write-Host "`nCriando servicos..." -ForegroundColor Cyan
$criados = 0
$erros = 0

foreach ($s in $servicos) {
    if (-not $map.ContainsKey($s.Profissional)) {
        Write-Host "  Profissional nao encontrado: $($s.Profissional)" -ForegroundColor Red
        $erros++
        continue
    }
    
    $profId = $map[$s.Profissional]
    $body = @{
        titulo = $s.Titulo
        descricao = $s.Descricao
        cidade = $s.Cidade
        preco = $s.Preco
    } | ConvertTo-Json
    
    try {
        $r = Invoke-RestMethod -Uri "$baseUrl/servicos?profissionalId=$profId" `
            -Method POST `
            -ContentType "application/json" `
            -Body $body
        
        $criados++
        Write-Host "  Servico criado: $($s.Titulo)" -ForegroundColor Green
    } catch {
        $erros++
        Write-Host "  Erro ao criar: $($s.Titulo) - $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host "`n=== Resumo ===" -ForegroundColor Cyan
Write-Host "Criados: $criados" -ForegroundColor Green
Write-Host "Erros: $erros" -ForegroundColor $(if ($erros -gt 0) { "Red" } else { "Green" })

if ($criados -gt 0) {
    Write-Host "`nAcesse: http://localhost:5136/Servicos" -ForegroundColor Yellow
}

