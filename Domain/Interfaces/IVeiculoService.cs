using minimal_api.Domain.Entities;
using minimal_api.Domain.Dto;

namespace minimal_api.Domain.Interfaces
{
    public interface IVeiculoService
    {
        List<Veiculo> GetAll(int page = 1, string? nome = null, string? marca = null);
        Veiculo? GetById(int id);
        void Create(Veiculo veiculo);
        void Update(int id, Veiculo veiculo);
        void Delete(Veiculo veiculo);
    }
}