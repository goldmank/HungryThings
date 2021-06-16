using Game.Scripts.Audio;
using Game.Scripts.Infra.Analytics;
using Game.Scripts.Model;
using Game.Scripts.Model.World;

namespace Game.Scripts.Ui
{
    public class StoreScreen : BaseGalleryScreen
    {
        protected override int GetCount()
        {
            return ModelManager.Get().Worlds.Worlds.Length;
        }

        protected override void InitItem(int index, GalleryButton item)
        {
            var standData = ModelManager.Get().Worlds.Worlds[index];
            item.Data = standData;

            if (ModelManager.Get().Store.IsItemPurchased("stand_" + standData.Type))
            {
                item.ShowPrice(0);
            }
            else if (standData.PriceType == PriceType.Coins)
            {
                item.ShowPrice(standData.Price);
            }
            else
            {
                item.ShowRv(ModelManager.Get().Store.GetRvPurchaseProgress("stand_" + standData.Type), standData.Price);
            }

            item.SetIcon(standData.Icon);
            item.SetSelected(ModelManager.Get().GlobalPref.World == standData.Type);
        }
        
        protected override void OnButtonClicked(GalleryButton item)
        {
            if (item.IsSelectable())
            {
                ModelManager.Get().AudioManager.PlayClick();
                OnStandSelected(((WorldData) item.Data).Type);
            }
            else
            {
                var standData = (WorldData) item.Data;
                if (standData.PriceType == PriceType.Coins)
                {
                    if (ModelManager.Get().Currency.Coins < standData.Price)
                    {
                        // not enough coins
                        ModelManager.Get().AudioManager.PlayClick();
                    }
                    else
                    {
                        ModelManager.Get().AudioManager.PlaySfx(SoundType.Buy);
                        OnPurchaseCompleted(item, standData);
                    }
                }
                else
                {
                    // watch rv
                    ModelManager.Get().AdManager.ShowRv(() =>
                    {
                        ModelManager.Get().AudioManager.PlaySfx(SoundType.Buy);
                        OnRvCompleted(item, standData);
                    });
                }
            }
        }

        private void OnRvCompleted(GalleryButton item, WorldData worldData)
        {
            var rvWatched = ModelManager.Get().Store.IncRvPurchaseProgress("stand_" + worldData.Type);
            item.ShowRv(rvWatched, worldData.Price);
            if (item.IsSelectable())
            {
                ModelManager.Get().Store.MarkItemAsPurchased("stand_" + worldData.Type);
                item.SetSelected(true);
                OnStandSelected(worldData.Type);
            }
        }

        private void OnPurchaseCompleted(GalleryButton item, WorldData worldData)
        {
            AnalyticsService.OnStandPurchase(worldData.Type);
            ModelManager.Get().Currency.AddCoins(-worldData.Price);
            ModelManager.Get().Store.MarkItemAsPurchased("stand_" + worldData.Type);
            item.ShowPrice(0);
            item.SetSelected(true);
            OnStandSelected(worldData.Type);
        }

        private void OnStandSelected(WorldType worldType)
        {
            ModelManager.Get().GlobalPref.World = worldType;
            GameManager.Get().LoadLevel();
            gameObject.SetActive(false);   
        }
    }
}