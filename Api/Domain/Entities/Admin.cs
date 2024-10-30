using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using minimal_api.Domain.Enums;

namespace minimal_api.Domain.Entities
{

    public class Admin
    {
        [Key] // Define a chave primária
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required] // Define que o campo é obrigatório
        [StringLength(255)] // Define o tamanho máximo do campo
        public string Email { get; set; } = default!;

        [StringLength(50)]
        [Required]
        public string Password { get; set; } = default!;

        [Range(0, 2)]
        public PerfilEnum Perfil { get; set; } = default!;

    }
}