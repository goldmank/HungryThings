namespace Game.Scripts.Model
{
    public class LevelsModel : PlayerPrefObject
    {
        public LevelsModel() : base("levels")
        {
            LoadKeys();
        }

        public bool IsLevelCompleted(int index)
        {
            return 1 == GetInt("level_" + index + "_complete");
        }

        public void MarkLevelAsComplete(int index)
        {
            Set("level_" + index + "_complete", 1);
        }

        public int CurrLevel
        {
            get => GetInt("level", 0);
            set => Set("level", value);
        }
        
        public int MaxLevel
        {
            get => GetInt("max_level", 0);
            set => Set("max_level", value);
        }
    }
}