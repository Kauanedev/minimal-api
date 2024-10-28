using minimal_api.Domain.Entities;
using minimal_api.Domain.Interfaces;
using minimal_api.Domain.Dto;
using minimal_api.Infra.Database;

namespace minimal_api.Domain.Services;

public class AdminService(DbContexto contexto) : IAdminService
{
    private readonly DbContexto _contexto = contexto;

    public Admin? Login(LoginDto loginDto)
    {
        var adm = _contexto.Administradores.Where
        (a => a.Email == loginDto.Email && a.Password == loginDto.Password)
        .FirstOrDefault();

        return adm;
    }

}