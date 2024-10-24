using Microsoft.EntityFrameworkCore;
using MinimalApi.Entities;

namespace MinimalApi.Infraestrutura.Db
{
    public class DbContexto(IConfiguration configuracaoAppSettings) : DbContext
    {
        private readonly IConfiguration _configuracaoAppSettings = configuracaoAppSettings;

        public DbSet<Admin> Administradores { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var stringConexao = _configuracaoAppSettings.GetConnectionString("DefaultConnection")?.ToString();
                if (!string.IsNullOrEmpty(stringConexao))
                    optionsBuilder.UseMySql(
                        stringConexao,
                        ServerVersion.AutoDetect(stringConexao)
                        );
            }
        }
    }
}