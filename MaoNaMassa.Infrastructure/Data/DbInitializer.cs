using MaoNaMassa.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaoNaMassa.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        Console.WriteLine("=== DbInitializer: Iniciando ===");
        
        // Verificar se já existem cursos
        int countCursos = 0;
        try
        {
            countCursos = await context.Cursos.CountAsync();
            Console.WriteLine($"DbInitializer: Encontrados {countCursos} cursos no banco.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DbInitializer: ERRO ao verificar cursos - {ex.Message}");
            Console.WriteLine($"DbInitializer: StackTrace: {ex.StackTrace}");
            // Tentar criar mesmo assim se a tabela não existir ainda
            if (!ex.Message.Contains("no such table"))
            {
                return;
            }
        }
        
        if (countCursos == 0)
        {
            Console.WriteLine("DbInitializer: Nenhum curso encontrado. Criando cursos de exemplo...");

        // Criar cursos de exemplo
        var cursos = new List<Curso>
        {
            new Curso(
                "Instalação Elétrica Residencial",
                "Aprenda a fazer instalações elétricas residenciais com segurança. Este curso aborda desde os conceitos básicos de eletricidade até a instalação completa de um sistema elétrico residencial, incluindo leitura de projetos, escolha de materiais e técnicas de instalação.",
                "Elétrica",
                "Iniciante"
            ),
            new Curso(
                "Pintura e Acabamento",
                "Domine as técnicas de pintura residencial e comercial. Aprenda preparação de superfícies, escolha de tintas, aplicação de diferentes tipos de acabamento e técnicas profissionais para obter resultados impecáveis.",
                "Pintura",
                "Iniciante"
            ),
            new Curso(
                "Encanamento Básico",
                "Curso completo sobre instalações hidráulicas residenciais. Você aprenderá a identificar e resolver problemas comuns, fazer instalações de canos, conexões e reparos em sistemas de água e esgoto.",
                "Encanamento",
                "Iniciante"
            ),
            new Curso(
                "Instalação de Pisos e Revestimentos",
                "Aprenda a instalar diferentes tipos de pisos e revestimentos: cerâmica, porcelanato, laminado e vinílico. Técnicas de nivelamento, assentamento e acabamento profissional.",
                "Acabamento",
                "Intermediário"
            ),
            new Curso(
                "Instalação de Ar Condicionado Split",
                "Curso avançado sobre instalação e manutenção de sistemas de ar condicionado split. Inclui dimensionamento, instalação de unidades internas e externas, drenagem e carga de gás refrigerante.",
                "Climatização",
                "Avançado"
            ),
            new Curso(
                "Marcenaria Básica",
                "Introdução à marcenaria com foco em móveis residenciais. Aprenda a trabalhar com madeira, usar ferramentas básicas, fazer cortes precisos e montar móveis simples como prateleiras e armários.",
                "Marcenaria",
                "Iniciante"
            ),
            new Curso(
                "Instalação de Drywall",
                "Técnicas profissionais para construção de paredes e forros em drywall. Aprenda estruturação, fixação de chapas, acabamento com massa corrida e instalação de portas e janelas.",
                "Construção",
                "Intermediário"
            ),
            new Curso(
                "Soldagem Elétrica Básica",
                "Curso prático de soldagem elétrica para iniciantes. Aprenda os fundamentos da soldagem, tipos de eletrodos, técnicas de solda e segurança no trabalho com equipamentos de solda.",
                "Soldagem",
                "Intermediário"
            )
        };

            try
            {
                await context.Cursos.AddRangeAsync(cursos);
                var saved = await context.SaveChangesAsync();
                Console.WriteLine($"DbInitializer: {saved} cursos criados com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DbInitializer: Erro ao salvar cursos - {ex.Message}");
                throw;
            }
        }
        else
        {
            Console.WriteLine("DbInitializer: Cursos já existem. Pulando seed de cursos.");
        }

        // Criar usuários de exemplo (independente dos cursos)
        await SeedUsuariosAsync(context);
    }

    private static async Task SeedUsuariosAsync(ApplicationDbContext context)
    {
        try
        {
            var count = await context.Usuarios.CountAsync();
            Console.WriteLine($"DbInitializer: Encontrados {count} usuários no banco.");
            
            if (count > 0)
            {
                Console.WriteLine("DbInitializer: Usuários já existem. Pulando seed de usuários.");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DbInitializer: ERRO ao verificar usuários - {ex.Message}");
            return;
        }
        
        Console.WriteLine("DbInitializer: Criando usuários de exemplo...");

        var usuarios = new List<Usuario>
        {
            // Aprendizes
            new Usuario(
                "João Silva",
                "joao.silva@email.com",
                "senha123",
                "São Paulo",
                "Elétrica",
                "Aprendiz"
            ),
            new Usuario(
                "Maria Santos",
                "maria.santos@email.com",
                "senha123",
                "Rio de Janeiro",
                "Pintura",
                "Aprendiz"
            ),
            new Usuario(
                "Pedro Oliveira",
                "pedro.oliveira@email.com",
                "senha123",
                "Belo Horizonte",
                "Encanamento",
                "Aprendiz"
            ),
            
            // Profissionais
            new Usuario(
                "Carlos Eletricista",
                "carlos.eletricista@email.com",
                "senha123",
                "São Paulo",
                "Elétrica",
                "Profissional"
            ),
            new Usuario(
                "Ana Pintora",
                "ana.pintora@email.com",
                "senha123",
                "Rio de Janeiro",
                "Pintura",
                "Profissional"
            ),
            new Usuario(
                "Roberto Encanador",
                "roberto.encanador@email.com",
                "senha123",
                "Brasília",
                "Encanamento",
                "Profissional"
            ),
            new Usuario(
                "Fernanda Marceneira",
                "fernanda.marceneira@email.com",
                "senha123",
                "Curitiba",
                "Marcenaria",
                "Profissional"
            ),
            
            // Clientes
            new Usuario(
                "Patricia Cliente",
                "patricia.cliente@email.com",
                "senha123",
                "São Paulo",
                "Reformas",
                "Cliente"
            ),
            new Usuario(
                "Lucas Cliente",
                "lucas.cliente@email.com",
                "senha123",
                "Porto Alegre",
                "Manutenção",
                "Cliente"
            ),
            
            // Empresa
            new Usuario(
                "Construtora ABC",
                "contato@construtoraabc.com",
                "senha123",
                "São Paulo",
                "Construção",
                "Empresa"
            )
        };

        try
        {
            await context.Usuarios.AddRangeAsync(usuarios);
            var saved = await context.SaveChangesAsync();
            Console.WriteLine($"DbInitializer: {saved} usuários criados com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DbInitializer: Erro ao salvar usuários - {ex.Message}");
            throw;
        }
    }
}

