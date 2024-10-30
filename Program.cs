using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using minimal_api.Domain.Interfaces;
using minimal_api.Domain.Services;
using minimal_api.Infra.Database;
using minimal_api.Domain.Dto;
using minimal_api.Domain.ModelViews;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Serialization;
using System.Text.Json;

#region Builder
var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(key)) key = "secret_key";

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IVeiculoService, VeiculoService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        In = ParameterLocation.Header,
        Description = "Insira o token JWT",
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

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

string createJwtToken(Admin admin)
{
    if (string.IsNullOrEmpty(key)) throw new InvalidOperationException("A chave JWT está vazia.");

    var securitykey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key));
    var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
        new Claim(ClaimTypes.Email, admin.Email),
        new Claim("Perfil", admin.Perfil.ToString()),
        new Claim(ClaimTypes.Role, admin.Perfil.ToString())
    };

    var token = new JwtSecurityToken
    (
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
}

static ErrorMessages ErrorDtoAdmin(AdminDto adminDto)
{
    var validation = new ErrorMessages { Messages = [] };

    if (string.IsNullOrEmpty(adminDto.Email)) validation.Messages.Add("O campo Email deve ser preenchido");
    if (adminDto.Perfil.ToString() == null) validation.Messages.Add("O campo Perfil deve ser preenchido");
    if (string.IsNullOrEmpty(adminDto.Password)) validation.Messages.Add("O campo Password deve ser preenchido");

    return validation;
}

app.MapPost("/administradores/login",
([FromBody] LoginDto loginDto, IAdminService adminService) =>
{
    try
    {    // Chama o método de login
        var admin = adminService.Login(loginDto);

        if (admin != null)
        {
            var perfilEnum = (PerfilEnum)Enum.Parse(typeof(PerfilEnum), admin.Perfil.ToString());

            string token = createJwtToken(admin);
            var adminReturn = new LoggedAdminModelView
            {
                Id = admin.Id,
                Email = admin.Email,
                Perfil = perfilEnum,
                Token = token

            };

            return Results.Ok(adminReturn);
        }

        return Results.Unauthorized();

    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
}).WithTags("Administradores");



app.MapPost("/administradores",
([FromBody] AdminDto adminDto, IAdminService adminService) =>
{
    try
    {
        var perfilEnum = 0;
        if (adminDto.Perfil == null) adminDto.Perfil = PerfilEnum.Admin;
        if (adminDto.Perfil.HasValue && adminDto.Perfil.Value == PerfilEnum.Editor) perfilEnum = 1;


        ErrorMessages validation = ErrorDtoAdmin(adminDto);
        if (validation.Messages.Count != 0) return Results.BadRequest(validation);
        {
            var admin = new Admin
            {
                Email = adminDto.Email,
                Perfil = (PerfilEnum)perfilEnum,
                Password = adminDto.Password
            };
            adminService.Create(admin);

            var adminReturn = new AdminModelView
            {
                Id = admin.Id,
                Email = admin.Email,
                Perfil = admin.Perfil
            };

            return Results.Created($"/veiculo", adminReturn);
        }
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }

}).WithTags("Administradores");

app.MapGet("/administradores", ([FromQuery] int? page, IAdminService adminService) =>
{
    try
    {
        var adminList = new List<AdminModelView>();
        var adminReturn = adminService.GetAll(page);

        foreach (var admin in adminReturn)
        {
            var adminModelView = new AdminModelView
            {
                Id = admin.Id,
                Email = admin.Email,
                Perfil = admin.Perfil,
            };
            adminList.Add(adminModelView);
        }
        return Results.Ok(adminList);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
.WithTags("Administradores");

app.MapGet("/administradores/{id}", ([FromQuery] string id, IAdminService adminService) =>
{
    try
    {
        var admin = adminService.GetById(id);

        if (admin == null) return Results.NotFound();

        return Results.Ok(new AdminModelView
        {
            Id = admin.Id,
            Email = admin.Email,
            Perfil = admin.Perfil
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
.WithTags("Administradores");

#endregion

#region Veiculos
static ErrorMessages ErrorDtoVeiculos(VeiculoDto veiculoDto)
{
    var validation = new ErrorMessages { Messages = [] };

    if (string.IsNullOrEmpty(veiculoDto.Nome)) validation.Messages.Add("O campo Nome deve ser preenchido");
    if (string.IsNullOrEmpty(veiculoDto.Marca)) validation.Messages.Add("O campo Marca deve ser preenchido");
    if (veiculoDto.Ano <= 1950) validation.Messages.Add("O campo Ano é obrigatório e não pode ser um número negativo ou menor que 1950");

    return validation;
}

app.MapPost("/veiculos", ([FromBody] VeiculoDto veiculoDto, IVeiculoService veiculoService) =>
{
    try
    {
        ErrorMessages validation = ErrorDtoVeiculos(veiculoDto);
        if (validation.Messages.Count != 0) return Results.BadRequest(validation);

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
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Admin, Editor" })
.WithTags("Veiculos");


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
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Admin, Editor" })
.WithTags("Veiculos");

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
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Admin, Editor" })
.WithTags("Veiculos");

app.MapPut("/veiculos/{id}", ([FromQuery] int id, VeiculoDto veiculoDto, IVeiculoService veiculoService) =>
{
    try
    {

        var veiculo = veiculoService.GetById(id);
        if (veiculo == null) return Results.NotFound();

        ErrorMessages validation = ErrorDtoVeiculos(veiculoDto);
        if (validation.Messages.Count != 0) return Results.BadRequest(validation);

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
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
.WithTags("Veiculos");

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
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
.WithTags("Veiculos");


#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

//ordem é importante!
app.UseAuthentication();
app.UseAuthorization();

app.Run();
#endregion
