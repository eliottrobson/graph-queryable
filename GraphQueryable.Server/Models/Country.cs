namespace GraphQueryable.Server.Models
{
    public record Country
    {
        public string Code { get; init; }

        public string Name { get; init; }
        
        public Continent Continent { get; init; }
    }
}