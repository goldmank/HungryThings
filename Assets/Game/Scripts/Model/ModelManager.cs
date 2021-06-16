using Facebook.Unity;
using Firebase.Analytics;
using Game.Scripts.Audio;
using Game.Scripts.Infra;
using Game.Scripts.Infra.Ads;
using Game.Scripts.Model.Eater;
using Game.Scripts.Model.Food;
using Game.Scripts.Model.Level;
using Game.Scripts.Model.Vfx;
using Game.Scripts.Model.World;
using UnityEngine;

namespace Game.Scripts.Model
{
    public class ModelManager : MonoBehaviour
    {
        [SerializeField] private WorldCatalog _worldCatalog;
        [SerializeField] private FoodCatalog _foodCatalog;
        [SerializeField] private LevelCatalog _levelCatalog;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private VfxCatalog _vfxCatalog;
        [SerializeField] private EaterCatalog _eaterCatalog;

        public WorldCatalog Worlds => _worldCatalog;
        public FoodCatalog Foods => _foodCatalog;
        public LevelCatalog Levels => _levelCatalog;
        public VfxCatalog Vfx => _vfxCatalog;
        public EaterCatalog Eaters => _eaterCatalog;
        public AudioManager AudioManager => _audioManager;

        public Tasker Tasker => _tasker;
        public GlobalPref GlobalPref => _globalPref;
        public CurrencyModel Currency => _currency;
        public LevelsModel LevelsStatus => _levels;
        public StoreModel Store => _store;
        public IAdManager AdManager => _adManager;

        private GlobalPref _globalPref;
        private CurrencyModel _currency;
        private LevelsModel _levels;
        private StoreModel _store;
        private Tasker _tasker;
        private IAdManager _adManager;
        
        private static ModelManager _instance;

        public static ModelManager Get()
        {
            return _instance;
        }
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            Debug.unityLogger.logEnabled = Consts.Debug;
            
            Levels.SortByLevel();
            _foodCatalog.InitRarityMaterials();
            
            _globalPref = new GlobalPref();
            _currency = new CurrencyModel();
            _levels = new LevelsModel();
            _store = new StoreModel();
            _tasker = gameObject.AddComponent<Tasker>();

            _globalPref.SfxEnabled = true;
            _globalPref.VibrateEnabled = true;
        }

        private void Start()
        {
#if (UNITY_STANDALONE || UNITY_WEBGL)
            _adManager = gameObject.AddComponent<MockAdManager>();
#else
            //_adManager = gameObject.AddComponent<AdManagerAdMob>();
#endif

            InitFirebase();
            
            FB.Init(() =>
            {
                Debug.Log("FB init completed");
            });
        }
        
        private void InitFirebase()
        {
#if (UNITY_EDITOR)
            return;
#endif
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, new Parameter(FirebaseAnalytics.ParameterLevelName, "Start"));
                
                Debug.Log("dependencyStatus:" + task.Result);
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
        
                    // var app = Firebase.FirebaseApp.DefaultInstance;
                    // Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
                    // Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
                } else {
                    UnityEngine.Debug.LogError(System.String.Format(
                        "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is not safe to use here.
                }
            });
        }

        // private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        // {
        //     Debug.Log("OnMessageReceived");
        // }
        //
        // private void OnTokenReceived(object sender, TokenReceivedEventArgs e)
        // {
        //     if (e == null)
        //     {
        //         return;
        //     }
        //     Debug.Log("OnTokenReceived: " + e.Token);
        // }
    }
}