using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApi; // Adicione este using

var builder = WebApplication.CreateBuilder(args);

// ADICIONE ESTA LINHA PARA CONFIGURAR A INJEÇÃO DE DEPENDÊNCIA
builder.Services.AddHttpClient<LocationService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();