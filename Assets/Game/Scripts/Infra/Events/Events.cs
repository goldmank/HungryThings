namespace IO.Infra.Scripts.Events
{
    public class Events
    {
        public static class Game
        {
            public static string LocalPlayerSpawned = "LocalPlayerSpawned";
            public static string LocalPlayerKilled = "LocalPlayerKilled";
            public static string AiPlayerSpawned = "AiPlayerSpawned";
            public static string RemotePlayerSpawned = "RemotePlayerSpawned";
            public static string PlayerKilled = "PlayerKilled";
            public static string PlayerHit = "PlayerHit";
            public static string GameTimeFinished = "GameTimeFinished";
            public static string GameTimeChanged = "GameTimeChanged";
            public static string GameOver = "GameOver";
            public static string RoundOverDueTimerout = "RoundOverDueTimerout";
            public static string GameStart = "GameStart";
            public static string PinataFinish = "PinataFinish";
            public static string RaceOver = "RaceOver";
            public static string FinishMatchMaking = "FinishMatchMaking";
            public static string FirstHitInMatch = "FirstHitInMatch";
            public static string PlayerBangingWall = "PlayerBangingWall";
            public static string BossSpawn = "BossSpawn";
            public static string BossKill = "BossKill";
            public static string RematchAgreed = "RematchAgreed";
            public static string RematchDeclined = "RematchDeclined";
            public static string SoccerInGoal = "SoccerInGoal";
            public static string DestorySoccerBall = "DestorySoccerBall";
            public static string DestoryAstroids = "DestoryAstroids";
            public static string StartAstroids = "StartAstroids";
            public static string DestoryPinata = "DestoryPinata";
            public static string DestoryBoss = "DestoryBoss";
            public static string DestoryRotator = "DestoryRotator";
            public static string StartRotator = "StartRotator";
            public static string FlagPosition = "FlagPosition";
            public static string FlagEnterBase = "FlagEnterBase";
            public static string FlagLeaveBase = "FlagLeaveBase";
            public static string FlagCaptured = "FlagCaptured";
            public static string FlagCapturedEffect = "FlagCapturedEffect";
            public static string InitFlagBase = "InitFlagBase";
            public static string PlayerShootStart = "PlayerShootStart";
            public static string PlayerShootEnd = "PlayerShootEnd";
            public static string PlayerDash = "PlayerDash";
            public static string PlayerBomb = "PlayerBomb";
            
            public static string ParamGameWorld = "ParamGameWorld";
            public static string ParamRematch = "ParamRematch";
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
            public static string GameStartClicked = "GameStartClicked";
            public static string StoreClicked = "StoreClicked";
            public static string FriendsClicked = "FriendsClicked";
            public static string WeaponsClicked = "WeaponsClicked";
            public static string BrawlersClicked = "BrawlersClicked";
            public static string HatsClicked = "HatsClicked";
            public static string ChallangesClicked = "ChallangesClicked";
            public static string MatchTypeSelected = "MatchTypeSelected";
            public static string PlayerBehindHud = "PlayerBehindHud";
            public static string BackFromInnerToMain = "BackFromInnerToMain";
            public static string CharacterChanged = "CharacterChanged";
            public static string CharacterSkinChanged = "CharacterSkinChanged";
            public static string WeaponSkinChanged = "WeaponSkinChanged";
            public static string WeaponSkinPreview = "WeaponSkinPreview";
            public static string WeaponChanged = "WeaponChanged";
            public static string BombChanged = "BombChanged";
            public static string HatChanged = "HatChanged";
            public static string SpanwerChanged = "SpanwerChanged";
            public static string ExplosionChanged = "ExplosionChanged";
            public static string JetChanged = "JetChanged";
            public static string DanceChanged = "DanceChanged";
            public static string PlayerScoreUpdated = "PlayerScoreUpdated";
            public static string BundlePurchased = "BundlePurchased";
            public static string StoreSkinPurchased = "StoreSkinPurchased";
            public static string RewardsScreenClicked = "RewardsScreenClicked";
            public static string LeaderboardsClick = "LeaderboardsClick";
            public static string NewsClick = "NewsClick";
            public static string HelpBoxShown = "HelpBoxShown";
            public static string SplashHidden = "SplashHidden";
            public static string GameOverScreenPrepare = "GameOverScreenPrepare";
            public static string EmojiReceived = "EmojiReceived";
            public static string NicknameChanged = "NicknameChanged";
            public static string StringsUpdated = "StringsUpdated";
            public static string ImagesUpdated = "ImagesUpdated";
            public static string BrawlerUnlocked = "BrawlerUnlocked";
            public static string PiggyBankClicked = "PiggyBankClicked";
            public static string SkinBoxClicked = "SkinBoxClicked";
            public static string LanguageChanged = "LanguageChanged";
            public static string ChangingLanguage = "ChangingLanguage";
            public static string HideHints = "HideHints";
            public static string FriendInvite = "FriendInvite";
            public static string FriendsUpdate = "FriendsUpdate";
            public static string OnlineFriendCountUpdate = "OnlineFriendCountUpdate";
            public static string OnlineInviteCountUpdate = "OnlineInviteCountUpdate";
        }

        public static class Param
        {
            public static string ParamPlayer = "ParamPlayer";
            public static string ParamKiller = "ParamKiller";
            public static string ParamTime = "ParamTime";
            public static string ParamPlayerType = "ParamPlayerType";
            public static string ParamPlayerSkin = "ParamPlayerSkin";
            public static string ParamItem = "ParamItem";
            public static string ParamItemName = "ParamItemName";
            public static string ParamMatchType = "ParamMatchType";
            public static string ParamActor = "ParamActor";
            public static string ParamTeam = "ParamTeam";
            public static string ParamScore = "ParamScore";
            public static string ParamGameOver = "ParamGameOver";
            public static string ParamRoundWinner = "ParamRoundWinner";
            public static string ParamMatchCode = "ParamMatchCode";
            public static string ParamObject = "ParamObject";
            public static string ParamAmount = "ParamAmount";
            public static string ParamSender = "ParamSender";
            public static string ParamRoomCode = "ParamRoomCode";
            public static string ParamCount = "ParamCount";
            public static string ParamPrivateMatch = "ParamPrivateMatch";
            public static string ParamPosition = "ParamPosition";
        }

        public static class Online
        {
            public static string Connected = "Connected";
            public static string Disconnected = "Disconnected";
            public static string ConnectStart = "ConnectStart";
            public static string JoinedRoom = "JoinedRoom";
            public static string JoinedRoomFailed = "JoinedRoomFailed";
            public static string MatchingFailed = "MatchingFailed";
            public static string PlayerEnter = "PlayerEnter";
            public static string PlayerLeave = "PlayerLeave";
            public static string ReconnectOtherRegionStart = "ReconnectOtherRegionStart";
            public static string ReconnectOtherRegionEnd = "ReconnectOtherRegionEnd";
            public static string FallbackToOffline = "FallbackToOffline";
            public static string MatchInviteReceived = "MatchInviteReceived";
            
            public static string ParamPlayer = "ParamPlayer";
        }
    }
}