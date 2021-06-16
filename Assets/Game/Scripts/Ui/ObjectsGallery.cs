using Game.Scripts.Model;

namespace Game.Scripts.Ui
{
    public class ObjectsGallery : BaseGalleryScreen
    {
        protected override int GetCount()
        {
            return ModelManager.Get().Foods.Objects.Length;
        }

        protected override void InitItem(int index, GalleryButton item)
        {
            var hiddenObject = ModelManager.Get().Foods.Objects[index];
            item.Data = hiddenObject;
            
            item.SetIcon(hiddenObject.Icon);
            
            ((ObjectGalleryButton)item).SetRarity(hiddenObject.Rarity);

            if (ModelManager.Get().Store.IsItemPurchased("object_" + hiddenObject.Type))
            {
                item.ShowLabel(hiddenObject.Name);
                item.ShowHiddenIcon(false);
            }
            else
            {
                item.ShowLabel(hiddenObject.Rarity.ToString());
                item.ShowHiddenIcon(true);
            }
        }

        protected override void OnButtonClicked(GalleryButton item)
        {
        }
        
        protected override bool AlwaysCreate()
        {
            return true;
        }
    }
}