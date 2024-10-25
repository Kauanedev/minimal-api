using minimal_api.Domain.Interfaces;
using MinimalApi.Dto;
using MinimalApi.Entities;
using MinimalApi.Infraestrutura.Db;

namespace MinimalApi.Domain.Interfaces;

public class AdminService : IAdminService
{
    private readonly DbContexto _contexto;
    public AdminService(DbContexto contexto)
    {
        _contexto = contexto;
    }
    public Admin? Login(LoginDto loginDto)
    {
        var adm = _contexto.Administradores.Where
        (a => a.Email == loginDto.Email && a.Password == loginDto.Password)
        .FirstOrDefault();

        return adm;
    }
}