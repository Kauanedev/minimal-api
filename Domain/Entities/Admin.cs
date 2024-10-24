using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApi.Entities
{

    public class Admin
    {
        [Key] // Define a chave primária
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Gera automaticamente o valor
        public int Id { get; set; } = default!;
        [Required] // Define que o campo é obrigatório
        [StringLength(255)] // Define o tamanho máximo do campo
        public string Email { get; set; } = default!;
        [StringLength(50)]
        public string Password { get; set; } = default!;
        [StringLength(10)]
        public string Perfil { get; set; } = default!;

    }
}