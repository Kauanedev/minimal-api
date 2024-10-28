using minimal_api.Domain.Enums;

namespace minimal_api.Domain.Dto
{
    public class AdminDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required PerfilEnum Perfil { get; set; }
    }
}