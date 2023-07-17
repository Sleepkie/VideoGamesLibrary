namespace VideoGamesLibrary.DTO
{
    public class GameDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Developer { get; set; }

        public List<string> Genres { get; set; }
    }
}
