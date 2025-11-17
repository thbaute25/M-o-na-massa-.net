using MaoNaMassa.API.Extensions;
using MaoNaMassa.API.Middleware;
using MaoNaMassa.Application.UseCases;
using MaoNaMassa.Domain.Interfaces;
using MaoNaMassa.Infrastructure.Data;
using MaoNaMassa.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddRazorPages(); // Adicionar suporte a Razor Pages
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Context - Usando SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(connectionString, sqlOptions => 
        sqlOptions.MigrationsAssembly("MaoNaMassa.Infrastructure"));
    
    // Apenas para desenvolvimento
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
});

// Repositórios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ICursoRepository, CursoRepository>();
builder.Services.AddScoped<IAulaRepository, AulaRepository>();
builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IRespostaQuizRepository, RespostaQuizRepository>();
builder.Services.AddScoped<ICertificadoRepository, CertificadoRepository>();
builder.Services.AddScoped<IProfissionalRepository, ProfissionalRepository>();
builder.Services.AddScoped<IServicoRepository, ServicoRepository>();
builder.Services.AddScoped<IAvaliacaoRepository, AvaliacaoRepository>();

// Use Cases
builder.Services.AddScoped<BuscarServicosUseCase>();
builder.Services.AddScoped<CriarServicoUseCase>();
builder.Services.AddScoped<BuscarProfissionaisUseCase>();
builder.Services.AddScoped<CriarPerfilProfissionalUseCase>();
builder.Services.AddScoped<AvaliarServicoUseCase>();
builder.Services.AddScoped<ResponderQuizUseCase>();
builder.Services.AddScoped<CompletarCursoUseCase>();
builder.Services.AddScoped<VisualizarCursoCompletoUseCase>();

// Validações e tratamento de erros
builder.Services.AddValidationAndErrorHandling();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Popular banco de dados com dados iniciais
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await DbInitializer.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao popular o banco de dados.");
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware de tratamento de erros global (deve ser um dos primeiros)
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Desabilitar HTTPS redirection temporariamente para facilitar testes
// app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseStaticFiles(); // Habilitar arquivos estáticos (CSS, JS, imagens)
app.UseRouting();
app.UseAuthorization();

// Configurar rotas padrão e personalizadas
app.MapRazorPages(); // Mapear Razor Pages (rotas: /, /Home, /Cursos, /Servicos, /Profissionais)
app.MapControllers(); // Mapear API Controllers (rotas: /api/*)

app.Run();
