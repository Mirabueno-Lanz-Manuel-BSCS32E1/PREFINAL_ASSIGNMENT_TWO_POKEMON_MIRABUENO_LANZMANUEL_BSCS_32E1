namespace PokemonApp.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Moves { get; set; }
        public List<string> Abilities { get; set; }
    }
}
