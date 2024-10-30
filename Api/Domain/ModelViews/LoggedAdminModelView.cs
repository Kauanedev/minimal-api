using minimal_api.Domain.Enums;

namespace minimal_api.Domain.ModelViews
{
    public record LoggedAdminModelView
    {
        public string? Id { get; set; }
        public required string Email { get; set; }
        public required PerfilEnum Perfil { get; set; }
        public string? Token { get; set; }
    }
}