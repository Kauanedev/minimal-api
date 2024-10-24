using MinimalApi.Infraestrutura.Db;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Dto;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
        );
});

var app = builder.Build();

app.MapPost("/login", (LoginDto loginDto) =>
{
    if (loginDto.Email == "admin@email.com" && loginDto.Password == "123456")
    {
        return Results.Ok("Login realizado com sucesso!");
    }
    else
    {
        return Results.Unauthorized();
    }
});


app.Run();
