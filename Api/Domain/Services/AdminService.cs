using minimal_api.Domain.Entities;
using minimal_api.Domain.Interfaces;
using minimal_api.Domain.Dto;
using minimal_api.Infra.Database;

namespace minimal_api.Domain.Services;

public class AdminService(DbContexto contexto) : IAdminService
{
    private readonly DbContexto _contexto = contexto;

    public Admin Create(Admin admin)
    {
        _contexto.Administradores.Add(admin);
        _contexto.SaveChanges();

        return admin;
    }

    public List<Admin> GetAll(int? page)
    {
        var query = _contexto.Administradores.AsQueryable();

        // Paginação
        int itensPorPagina = 10;
        if (page != null)
        {
            int skipCount = ((int)page - 1) * itensPorPagina;
            query = query.Skip(skipCount).Take(itensPorPagina);
        }
        return query.ToList();
    }

    public Admin? GetById(string id)
    {
        return _contexto.Administradores.Where(v => v.Id == id).FirstOrDefault();
    }


    public void Update(Veiculo veiculo)
    {
        _contexto.Veiculos.Update(veiculo);
        _contexto.SaveChanges();
    }

    public Admin? Login(LoginDto loginDto)
    {
        // Verifica se os dados de entrada são válidos
        if (loginDto == null || string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
        {
            throw new ArgumentException("Dados de login inválidos.");
        }

        // Busca o administrador no banco de dados
        var admin = _contexto.Administradores
            .FirstOrDefault(a => a.Email == loginDto.Email && a.Password == loginDto.Password);

        return admin;
    }

}