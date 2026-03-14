using System;

namespace LifeQuest.BLL.DTOs
{
    public class LevelDto
    {
        public int Id { get; set; }
        public int LevelsCount { get; set; }
        public string LevelName { get; set; } = string.Empty;
        public int Point { get; set; }
    }
}
