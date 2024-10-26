using Microsoft.EntityFrameworkCore;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Interfaces;
using minimal_api.Infra.Database;

namespace minimal_api.Domain.Services
{
    public class VeiculoService : IVeiculoService
    {
        private readonly DbContexto _contexto;
        public void Create(Veiculo veiculo)
        {
            _contexto.Veiculos.Add(veiculo);
            _contexto.SaveChanges();
        }
        public void Delete(Veiculo veiculo)
        {
            _contexto.Veiculos.Remove(veiculo);
            _contexto.SaveChanges();
        }

        public List<Veiculo> GetAll(int page = 1, string? nome = null, string? marca = null)
        {
            var query = _contexto.Veiculos.AsQueryable();
            if (!string.IsNullOrEmpty(nome))
                query = query.Where(v => EF.Functions.Like(v.Nome.ToLower(), $"%{nome}%"));

            // Paginação
            int itensPorPagina = 10;
            int skipCount = (page - 1) * itensPorPagina;
            query = query.Skip(skipCount).Take(itensPorPagina);


            return query.ToList();
        }

        public Veiculo? GetById(int id)
        {
            return _contexto.Veiculos.Where(v => v.Id == id).FirstOrDefault();
        }

        public void Update(int id, Veiculo veiculo)
        {
            _contexto.Veiculos.Update(veiculo);
            _contexto.SaveChanges();
        }

    }
}