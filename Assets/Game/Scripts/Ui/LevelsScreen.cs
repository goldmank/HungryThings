using Game.Scripts.Model;
using Game.Scripts.Model.Level;

namespace Game.Scripts.Ui
{
    public class LevelsScreen : BaseGalleryScreen
    {
        protected override int GetCount()
        {
            return ModelManager.Get().Levels.Levels.Length;
        }

        protected override void InitItem(int index, GalleryButton item)
        {
            // var levelData = ModelManager.Get().Levels.Levels[index];
            // item.Data = levelData;
            // item.ShowLabel("Level " + (levelData.LevelNumber+1));
            // item.SetSelected(ModelManager.Get().LevelsStatus.CurrLevel == levelData.LevelNumber);
            //
            // var containerData = ModelManager.Get().Containers.GetContainer(levelData.Container);
            // item.SetIcon(containerData.Icon);
        }
        
        protected override void OnButtonClicked(GalleryButton item)
        {
            ModelManager.Get().AudioManager.PlayClick();
            ModelManager.Get().LevelsStatus.CurrLevel = ((LevelData) item.Data).LevelNumber;
            GameManager.Get().LoadLevel();
            gameObject.SetActive(false);
        }
    }
}