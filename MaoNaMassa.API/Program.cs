using MaoNaMassa.API.Extensions;
using MaoNaMassa.API.Middleware;
using MaoNaMassa.Application.UseCases;
using MaoNaMassa.Domain.Interfaces;
using MaoNaMassa.Infrastructure.Data;
using MaoNaMassa.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Context - Usando In-Memory para testes (sem necessidade de banco real)
// Configurado para não falhar se o banco não existir
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseInMemoryDatabase("MaoNaMassaDb");
    options.EnableSensitiveDataLogging(); // Apenas para desenvolvimento
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
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
