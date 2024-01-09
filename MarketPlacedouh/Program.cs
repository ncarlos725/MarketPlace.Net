// Aquí vamos a configurar todas nuestras cosas básicas para decirle que

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.

builder.Services.AddControllers();

// Aprende más acerca de la configuración de Swagger/OpenAPI en https://aka.ms/aspnetcore/smashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Esta es para que valide lo que esta en appsetting.json (Jwt)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // emisor 
        ValidateAudience = true, // para los que consumen
        ValidateLifetime = true, // duracion
        ValidateIssuerSigningKey = true, 
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // "Issuer" = "http://localhost:7200/"
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Aquí guardamos nuestra clave en bytes
    };
        
});

var app = builder.Build();

// Configure the http request  pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();    
app.UseAuthorization();
app.MapControllers();
app.Run();

//aqui me quede ...continuar despues de buscar cuna
//https://youtu.be/PR5XM-SNcm8?t=1055