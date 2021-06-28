using Game.Scripts.Infra;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Ui
{
    public class GameHud : MonoBehaviour
    {
        [SerializeField] private CoinsView _coins;
        
        [SerializeField] private MenuButton _levelsButton;
        //[SerializeField] private MenuButton _containerButton;
        [SerializeField] private MenuButton _standsButton;
        [SerializeField] private MenuButton _objectsButton;
        [SerializeField] private TextMeshProUGUI _levelNumber;
        [SerializeField] private TextMeshProUGUI _levelDesc;
        [SerializeField] private TextMeshProUGUI _objectAmount;
        [SerializeField] private Image _objectAmountIcon;
        [SerializeField] private Image _levelDescIcon;
        [SerializeField] private GameObject _settingsButton;
        
        [SerializeField] private LevelProgress _levelProgress;
        [SerializeField] private GameObject _spawnButton;

        public LevelProgress LevelProgress => _levelProgress;

        public Image LevelDescIcon => _levelDescIcon;

        public void ShowGameOver()
        {
            _levelProgress.Hide();
            Utils.FadeTo(_spawnButton, 0.3f, 0);
        }
        
        public void SetLevelNumber(int level)
        {
            _levelNumber.text = "LEVEL " + level;
        }

        public void SetLevelDesc(string text)
        {
            _levelDesc.text = text;
        }

        public void ShowLevelDescIcon(bool show)
        {
            _levelDescIcon.gameObject.SetActive(show);
        }

        public void SetObjectAmount(int count)
        {
            _objectAmount.text = "x" + count;
        }
        
        public void SetObjectAmountIcon(Sprite icon)
        {
            _objectAmountIcon.sprite = icon;
        }

        public void ShowObjectAmount(bool show)
        {
            _objectAmount.transform.parent.gameObject.SetActive(show);
        }

        public void RefreshCounters()
        {
            // _levelsButton.SetAlert(0);
            // _standsButton.SetAlert(0);
            // _objectsButton.SetAlert(0);
        }

        public bool IsGameOver()
        {
            return !_levelProgress.gameObject.activeSelf;
        }

        public void OnGameOver()
        {
            _levelProgress.gameObject.SetActive(false);
            _levelsButton.gameObject.SetActive(false);
            //_containerButton.gameObject.SetActive(false);
            _standsButton.gameObject.SetActive(false);
            _objectsButton.gameObject.SetActive(false);
            _settingsButton.gameObject.SetActive(false);
        }

        public void OnNewGame()
        {
            gameObject.SetActive(true);
            _levelProgress.gameObject.SetActive(true);
            _levelsButton.gameObject.SetActive(true);
            //_containerButton.gameObject.SetActive(true);
            _standsButton.gameObject.SetActive(true);
            _objectsButton.gameObject.SetActive(true);
            _settingsButton.gameObject.SetActive(true);
        }
    }
}