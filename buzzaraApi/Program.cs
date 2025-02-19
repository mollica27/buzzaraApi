using buzzaraApi.Data;
using buzzaraApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurando o banco de dados
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

// 2. Configura��o do JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(
    jwtSettings["Secret"]
    ?? throw new InvalidOperationException("JWT Secret Key is missing."));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };
    });

builder.Services.AddAuthorization();

// 3. Registrando os servi�os da aplica��o
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PerfilAcompanhanteService>();
builder.Services.AddScoped<MidiaService>();

// 4. Adicionando controllers e configurando Swagger
//    => Adicionamos tamb�m o ReferenceHandler para ignorar loops
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Ignora ciclos de refer�ncia automaticamente
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 5. Constru��o do app
var app = builder.Build();

// 6. Habilitando Swagger no ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 7. Configurar HTTPS e arquivos est�ticos
app.UseHttpsRedirection();
app.UseStaticFiles();

// 8. Ativar autentica��o e autoriza��o
app.UseAuthentication();
app.UseAuthorization();

// 9. Mapear os controllers
app.MapControllers();

// 10. Rodar a aplica��o
app.Run();
