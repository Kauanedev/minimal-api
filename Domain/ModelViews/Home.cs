namespace minimal_api.Domain.ModelViews
{
    public struct Home
    {
        public readonly string Message { get => "Welcometo the minimal API"; }
        public readonly string Documentation { get => "http://localhost:5152/swagger"; }
    }
}