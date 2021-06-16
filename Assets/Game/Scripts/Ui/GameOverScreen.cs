using DG.Tweening;
using Game.Scripts.Infra;
using Game.Scripts.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Ui
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private Image _bg;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private Transform _line;
        [SerializeField] private GameObject _continue;
        [SerializeField] private Image _rarityLine;
        [SerializeField] private TextMeshProUGUI _rarityText;

        private void Update()
        {
            if (!Consts.Debug)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                OnContinue();
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);

            var currLevel = ModelManager.Get().LevelsStatus.CurrLevel;
            var levelData = ModelManager.Get().Levels.Levels[currLevel];
            var hiddenObject = ModelManager.Get().Foods.GetFood(levelData.Food);
            _title.text = hiddenObject.Name + " Unlocked!";
            
            _continue.gameObject.SetActive(false);
            
            _line.localScale = new Vector3(0, 1, 1);
            Utils.SetAlpha(_title, 0);
            
            _line.DOScale(1, 0.3f).OnComplete(() => { _title.DOFade(1, 0.3f); });
            
            _rarityLine.transform.localScale = new Vector3(0, 1, 1);
            _rarityLine.color = ModelManager.Get().Foods.RarityColor[(int) hiddenObject.Rarity];
            Utils.SetAlpha(_rarityText, 0);
            _rarityText.text = hiddenObject.Rarity.ToString();
            
            _rarityLine.transform.DOScale(1, 0.3f).SetDelay(0.15f).OnComplete(() => { 
                _rarityText.DOFade(1, 0.2f).OnComplete(() =>
                {
                    ModelManager.Get().Tasker.Run(() =>
                    {
                        _continue.gameObject.SetActive(true);
                    }, 0.3f);
                }); 
            });
            
            ModelManager.Get().Store.MarkItemAsPurchased("object_" + hiddenObject.Type);
            ModelManager.Get().GlobalPref.ObjectsUnlockCount++;
        }

        public void OnContinue()
        {
            ModelManager.Get().AudioManager.PlayClick();
            
            var numOfLevels = ModelManager.Get().Levels.Levels.Length;

            var levels = ModelManager.Get().LevelsStatus;
            levels.MarkLevelAsComplete(levels.CurrLevel);
            levels.MaxLevel = Mathf.Max(levels.MaxLevel, levels.CurrLevel);
            levels.CurrLevel = (levels.CurrLevel + 1) % numOfLevels;
            
            GameManager.Get().Ui.ShowOverlay(() =>
            {
                gameObject.SetActive(false);
                GameManager.Get().LoadLevel();
                GameManager.Get().Ui.HideOverlay(null);
            });
        }
    }
}