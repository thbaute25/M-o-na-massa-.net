using MaoNaMassa.API.Extensions;
using MaoNaMassa.API.Middleware;
using MaoNaMassa.Infrastructure.Data;
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
