using minimal_api.Domain.Entities;
using minimal_api.Domain.Dto;

namespace minimal_api.Domain.Interfaces
{
    public interface IAdminService
    {
        Admin? Login(LoginDto loginDto);
        Admin Create(Admin admin);
        List<Admin> GetAll(int? page);
        Admin? GetById(string id);
    }
}