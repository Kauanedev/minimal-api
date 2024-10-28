using Microsoft.EntityFrameworkCore;
using minimal_api.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Minimal_api.Domain.Services;
using minimal_api.Infra.Database;
using minimal_api.Domain.Dto;
using minimal_api.Domain.MoedlViews;
using minimal_api.Domain.Services;
using minimal_api.Domain.Entities;

#region Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IVeiculoService, VeiculoService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
        );
});

var app = builder.Build();
#endregion


#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion


#region Administradores 
app.MapPost("/administradores/login",
([FromBody] LoginDto loginDto, IAdminService adminService) =>
{
    if (adminService.Login(loginDto) != null)
        return Results.Ok("Login realizado com sucesso!");

    else return Results.Unauthorized();

}).WithTags("Administradores");
#endregion

#region Veiculos
app.MapPost("/veiculos", ([FromBody] VeiculoDto veiculoDto, IVeiculoService veiculoService) =>
{
    try
    {
        var veiculo = new Veiculo
        {
            Nome = veiculoDto.Nome,
            Marca = veiculoDto.Marca,
            Ano = veiculoDto.Ano
        };

        veiculoService.Create(veiculo);

        return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
}).WithTags("Veiculos");

app.MapGet("/veiculos", ([FromQuery] int? page, IVeiculoService veiculoService) =>
{
    try
    {
        var veiculos = veiculoService.GetAll(page);
        return Results.Ok(veiculos);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
}).WithTags("Veiculos");

app.MapGet("/veiculos/{id}", ([FromQuery] int id, IVeiculoService veiculoService) =>
{
    try
    {
        var veiculo = veiculoService.GetById(id);

        if (veiculo == null) return Results.NotFound();

        return Results.Ok(veiculo);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
}).WithTags("Veiculos");

app.MapPut("/veiculos/{id}", ([FromQuery] int id, VeiculoDto veiculoDto, IVeiculoService veiculoService) =>
{
    try
    {
        var veiculo = veiculoService.GetById(id);

        if (veiculo == null) return Results.NotFound();

        veiculo.Nome = veiculoDto.Nome;
        veiculo.Marca = veiculoDto.Marca;
        veiculo.Ano = veiculoDto.Ano;

        veiculoService.Update(veiculo);

        return Results.Ok(veiculo);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
}).WithTags("Veiculos");

app.MapDelete("/veiculos/{id}", ([FromQuery] int id, IVeiculoService veiculoService) =>
{
    try
    {
        var veiculo = veiculoService.GetById(id);

        if (veiculo == null) return Results.NotFound();

        veiculoService.Delete(veiculo);

        return Results.Ok("Veiculo deletado com sucesso!");
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
}).WithTags("Veiculos");


#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion
