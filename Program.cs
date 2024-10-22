using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Agrega servicios a la contenedor.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "QR Code Generator API", Version = "v1" });
});

var app = builder.Build();

// Configura el middleware para Swagger
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "QR Code Generator API v1");
        c.RoutePrefix = string.Empty; // Para servir Swagger en la raíz
    });
}

app.UseAuthorization();
app.MapControllers();
app.Run();
