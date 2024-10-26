using Microsoft.EntityFrameworkCore;
using minimal_api.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Minimal_api.Domain.Services;
using minimal_api.Infra.Database;
using minimal_api.Domain.Dto;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
        );
});

var app = builder.Build();

app.MapPost("/login",
([FromBody] LoginDto loginDto, IAdminService adminService) =>
{
    if (adminService.Login(loginDto) != null)
    {
        return Results.Ok("Login realizado com sucesso!");
    }
    else
    {
        return Results.Unauthorized();
    }
});


app.Run();
