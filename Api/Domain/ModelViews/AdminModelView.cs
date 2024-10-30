using minimal_api.Domain.Enums;

namespace minimal_api.Domain.ModelViews
{
    public record AdminModelView
    {
        public string? Id { get; set; }
        public required string Email { get; set; }
        public required PerfilEnum Perfil { get; set; }
    }
}