namespace LifeQuest.Models
{
    public class Level
    {
        public int Id { get; set; }
        public int LevelsCount { get; set; }
        public string LevelName { get; set; }
        //points to gained in new Level
        public int Point { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
