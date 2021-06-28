using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.Audio;
using Game.Scripts.Infra;
using Game.Scripts.Infra.Analytics;
using Game.Scripts.Infra.Events;
using Game.Scripts.Model;
using Game.Scripts.Model.Food;
using Game.Scripts.Model.HiddenObject;
using Game.Scripts.Model.Level;
using Game.Scripts.Model.Vfx;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;
using IO.Infra.Scripts.Events;
using UnityEngine;

namespace Game.Scripts.Level
{
    public class LevelManager : MonoBehaviour
    {
        private const float AutoRotateSpeed = 8;
        private const float DefaultCameraPosY = 2.44f;
        
        [SerializeField] private Transform _objectContainer;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private float _rotateSize;
        [SerializeField] private SwipeDetector _swipeDetector;
        [SerializeField] private Transform _zoomPos;
        
        private List<Eater> _ants = new List<Eater>();
        
        private bool _loaded;
        private LevelData _levelData;
        private FoodObject _food;
        private World _world;
        private bool _autoRotate;
        private bool _completed;

        public void Load(int levelIndex)
        {
            Debug.Log("Load: " + levelIndex);
            
            _loaded = false;
            
            _food = null;
            _world = null;
            foreach (Transform child in _objectContainer.transform) {
                Destroy(child.gameObject);
            }
            
            _levelData = ModelManager.Get().Levels.GetLevel(levelIndex);
            if (null == _levelData)
            {
                Debug.Log("levelData not found");
                return;
            }
            
            var worldData = ModelManager.Get().Worlds.GetWorld(ModelManager.Get().GlobalPref.World);
            if (null == worldData)
            {
                Debug.Log("stand not found");
                return;
            }
            
            // set random hidden object for level
            _levelData.Food = FoodType.Chick;// ModelManager.Get().Foods.GetRandomHiddenObjectType();
            
            var objectData = ModelManager.Get().Foods.GetFood(_levelData.Food);
            if (null == objectData)
            {
                return;
            }
            
            _world = Instantiate(worldData.Prefab, _objectContainer);

            //_food = FindObjectOfType<FoodObject>();
            _food = Instantiate(objectData.Prefab, _world.SpawnPoint, Quaternion.Euler(0, 0, 0));
            _food.transform.parent = _objectContainer;
            _food.FoodReady += (newFood) =>
            {
                Debug.Log("Food ready");
            };
            
            // GameManager.Get().Ui.GameHud.OnNewGame();
            // GameManager.Get().Ui.GameHud.SetLevelNumber(levelIndex + 1);
            // GameManager.Get().Ui.GameHud.LevelProgress.SetProgress(0, true);
            // GameManager.Get().Ui.GameHud.LevelProgress.HideMarkers();
            // if (_levelData.Markers != null)
            // {
            //     for (var i = 0; i < _levelData.Markers.Length; i++)
            //     {
            //         GameManager.Get().Ui.GameHud.LevelProgress.ShowMarker(i, _levelData.Markers[i]);
            //     }
            // }
            //
            // _timeLeft = 0;
            // _lastTimeUpdate = 0;
            // _autoComplete = false;
            // GameManager.Get().Ui.GameHud.ShowLevelDescIcon(false);
            // GameManager.Get().Ui.HideTimesUp();
            // GameManager.Get().Ui.GameHud.SetLevelDesc("");
            // GameManager.Get().Ui.GameHud.ShowObjectAmount(false);
            //
            // _spawnObjectTime = null;
            //
            // switch (_levelData.Type)
            // {
            //     case LevelType.Find:
            //     {
            //         var p = 0.1f;
            //         _spawnObjectTime = new List<float>();
            //         for (var i = 0; i < _levelData.SpawnedObjectsCount; i++)
            //         {
            //             var pLeft = 1.0f - p;
            //             var nextP = Random.Range(p + pLeft * 0.15f, p + pLeft * 0.5f);
            //             _spawnObjectTime.Add(nextP);
            //             p = nextP;
            //         }
            //         GameManager.Get().Ui.GameHud.SetObjectAmount(_levelData.SpawnedObjectsCount);
            //         GameManager.Get().Ui.GameHud.SetObjectAmountIcon(_levelData.SpawnObjectIcon);
            //         GameManager.Get().Ui.GameHud.ShowObjectAmount(true);
            //     }
            //         break;
            //     case LevelType.Layered:
            //     {
            //         GameManager.Get().Ui.GameHud.SetLevelDesc(Strings.Game.DescLayer);
            //     }
            //         break;
            //     case LevelType.Time:
            //     {
            //         _timeLeft = _levelData.Duration;
            //         GameManager.Get().Ui.GameHud.ShowLevelDescIcon(true);
            //         UpdateTimeLeft();
            //     }
            //         break;
            // }
            
            _loaded = true;
            //_autoRotate = true;
            EnableInput(_loaded);
            
            // AnalyticsService.OnLevelStart(levelIndex);
            //
            // // delayed report to let GameAnalytics initialzied
            // ModelManager.Get().Tasker.Run(() =>
            // {
            //     GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, levelIndex.ToString());
            // }, 0.3f);
        }
        
        public FoodObject GetFood()
        {
            return _food;
        }

        public void ShowGame(bool show)
        {
            _objectContainer.gameObject.SetActive(show);
        }

        private void Start()
        {
            _swipeDetector.OnSwipe += OnSwipeDetected;

            _autoRotate = true;
            EnableInput(_loaded);

            var eaterSpawner = gameObject.AddComponent<EaterSpawner>();
            eaterSpawner.Init(_objectContainer);
            
            SimpleEventManager.Get().Subscribe(Events.Game.LevelCompleted, OnLevelCompleted);
        }

        private void OnLevelCompleted(EventParams obj)
        {
            if (_completed)
            {
                return;
            }
            _completed = true;
            
            GameManager.Get().Ui.GameHud.ShowGameOver();

            var eaterSpawner = gameObject.GetComponent<EaterSpawner>();
            var eaters = eaterSpawner.Eaters;
            var pos = Vector3.zero;
            foreach (var eater in eaters)
            {
                pos += eater.transform.position;
            }

            pos /= eaters.Count;
            Debug.Log("point camera to: " + pos);

            var posA = 0.0f;
            foreach (var eater in eaters)
            {
                eater.Finish();
                
                var target = Vector3.zero;
                var currPos = eater.transform.position;
                var r = Random.Range(-2.2f, 2.2f);
                target.x += Mathf.Cos(posA) * r;
                target.z += Mathf.Sin(posA) * r;
                target.y = currPos.y;
                posA += Mathf.PI * 0.2f;

                var d = _zoomPos.position - target;
                var a = Mathf.Atan2(d.z, d.x);
                eater.transform.DOMove(target, 0.5f);
                eater.transform.DOLocalRotate(new Vector3(-90, 180 + -140 + a * Mathf.Rad2Deg, 0), 0.5f);
            }

            GameManager.Get().Camera.transform.DOMove(_zoomPos.position, 1f);
            GameManager.Get().Camera.transform.DORotate(_zoomPos.rotation.eulerAngles, 1f);
            
            ModelManager.Get().Tasker.Run(() =>
            {
                ModelManager.Get().Vfx.Create(VfxType.Pop, Vector3.zero, 3);
            }, 1);
            ModelManager.Get().Tasker.Run(() =>
            {
                ModelManager.Get().Vfx.Create(VfxType.Pop, new Vector3(0, 0.5f, 0), 3);
            }, 1.2f);
            ModelManager.Get().Tasker.Run(() =>
            {
                ModelManager.Get().Vfx.Create(VfxType.Pop, Vector3.zero, 3);
            }, 1.4f);

            ModelManager.Get().Tasker.Run(() =>
            {
                foreach (var eater in eaters)
                {
                    eater.transform.DOJump(eater.transform.position, 0.6f, 4, 1.6f).SetDelay(Random.Range(0, 0.5f)).SetEase(Ease.Linear);
                }
            }, 1.0f);
        }

        private void OnDestroy()
        {
            _swipeDetector.OnSwipe -= OnSwipeDetected;
        }

        private void Update()
        {
            if (_completed)
            {
                return;
            }
            if (!GameManager.Get().Ui.GameHud.gameObject.activeSelf)
            {
                return;
            }
            
            if (!_loaded)
            {
                return;
            }

            // if (_autoRotate)
            // {
            //     _objectContainer.eulerAngles += new Vector3(0, Time.deltaTime * AutoRotateSpeed, 0);
            // }
        }

        private void OnTimeOut()
        {
            if (GameManager.Get().Ui.GameHud.IsGameOver())
            {
                return;
            }
            GameManager.Get().Ui.ShowTimesUp();
        }

        private void OnZoomDetected(float amount)
        {
            //_camera.fieldOfView += amount;
        }
        
        private void OnSwipeDetected(SwipeDetector.SwipeDirection swipeDirection)
        {
            // if (!_autoRotate)
            // {
            //     Debug.Log("in middle of swipe rotate");
            //     return;
            // }
            //
            // var rotation = transform.rotation.eulerAngles;
            // Debug.Log("OnSwipeDetected: " + swipeDirection);
            // Debug.Log("rotation: " + rotation);
            //
            // switch (swipeDirection)
            // {
            //     case SwipeDetector.SwipeDirection.Left:
            //     {
            //         SwipeRotate(rotation + Vector3.up * _rotateSize);
            //     }
            //         break;
            //     case SwipeDetector.SwipeDirection.Right:
            //     {
            //         SwipeRotate(rotation + Vector3.down * _rotateSize);
            //     }
            //         break;
            //     case SwipeDetector.SwipeDirection.Up:
            //     {
            //         //SwipeRotate(rotation + Vector3.right * _rotateSize);
            //     }
            //         break;
            //     case SwipeDetector.SwipeDirection.Down:
            //     {
            //         //SwipeRotate(rotation + Vector3.left * _rotateSize);
            //     }
            //         break;
            // }
        }

        private void SwipeRotate(Vector3 target)
        {
            _autoRotate = false;
            _objectContainer.transform.DORotate(target, _rotateSpeed, RotateMode.WorldAxisAdd).OnComplete(() =>
            {
                _autoRotate = true;
            });
        }
        
        private void OnObjectRevealProgress(float progress)
        {
            GameManager.Get().Ui.GameHud.LevelProgress.SetProgress(progress);
        }

        private void EnableInput(bool enable)
        {
            _swipeDetector.enabled = enable;
        }
        
        // private void SpawnAnt()
        // {
        //     var a = Random.Range(0, 360) * Mathf.Deg2Rad;
        //     var r = Random.Range(2.0f, 3.0f);
        //     var x = Mathf.Cos(a) * r;
        //     var z = Mathf.Sin(a) * r;
        //     var ant = Instantiate(pfEater, new Vector3(x, 2, z), Quaternion.identity);
        //     ant.transform.parent = _world;
        //     _ants.Add(ant);
        //
        //     ant.Init(Random.Range(3,6));
        // }
    }
}