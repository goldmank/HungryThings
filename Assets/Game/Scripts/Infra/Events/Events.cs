namespace IO.Infra.Scripts.Events
{
    public class Events
    {
        public static class Game
        {
            public static string EaterHolderSpawn = "EaterHolderSpawn";
            public static string EaterHolderDiscard = "EaterHolderDiscard";
            public static string EaterHolderReleased = "EaterHolderReleased";
            public static string LevelCompleted = "LevelCompleted";

            public static string ParamEaterType = "EaterType";
        }
        
        public static class Debug
        {
            public static string TimeSpeedChanged = "TimeSpeedChanged";
            public static string MoveToNextEvent = "MoveToNextEvent";
            public static string ParamValue = "ParamValue";
        }

        public static class Guide
        {
            public static string AimOn = "AimOn";
            public static string AimOff = "AimOff";
            public static string ShowNotAimed = "ShowNotAimed";
        }
        
        public static class Ui
        {
            
        }
    }
}