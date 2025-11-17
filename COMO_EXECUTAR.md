# 🚀 Como Executar e Testar a API

## Passo 1: Executar a API

Abra um terminal PowerShell na pasta do projeto e execute:

```powershell
cd MaoNaMassa.API
dotnet run
```

Você deve ver algo como:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5136
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

## Passo 2: Testar no Navegador

Com a API rodando, abra seu navegador e acesse:

1. **Swagger UI**: `http://localhost:5136/swagger`
   - Aqui você verá a documentação interativa da API

2. **Health Endpoint**: `http://localhost:5136/api/health`
   - Deve retornar: `{"status":"OK","message":"API Mão na Massa está funcionando!","timestamp":"..."}`

## Passo 3: Testar via PowerShell (em outro terminal)

```powershell
# Testar Health endpoint
Invoke-RestMethod -Uri http://localhost:5136/api/health

# Ou usando curl (se disponível)
curl http://localhost:5136/api/health
```

## ⚠️ Problemas Comuns

### "This localhost page can't be found"

**Solução 1**: Verifique se a API está realmente rodando
- Você deve ver as mensagens de log no terminal
- Se não estiver, execute `dotnet run` novamente

**Solução 2**: Verifique a porta
- A API está configurada para rodar na porta `5136`
- Se essa porta estiver em uso, o .NET pode escolher outra porta
- Verifique a mensagem "Now listening on:" no terminal

**Solução 3**: Tente acessar via IP local
- `http://127.0.0.1:5136/swagger`
- `http://127.0.0.1:5136/api/health`

**Solução 4**: Verifique o firewall
- O Windows Firewall pode estar bloqueando
- Tente desabilitar temporariamente para testar

### Erro ao iniciar

Se a API não iniciar, verifique:
1. Build está OK: `dotnet build`
2. Não há outros processos usando a porta
3. Todos os pacotes NuGet estão instalados

## 📝 Endpoints Disponíveis

Atualmente, apenas o Health endpoint está implementado:
- `GET /api/health` - Verifica se a API está funcionando

Os outros endpoints serão criados quando implementarmos os Controllers completos.

