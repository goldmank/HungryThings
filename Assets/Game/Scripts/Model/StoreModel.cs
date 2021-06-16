namespace Game.Scripts.Model
{
    public class StoreModel : PlayerPrefObject
    {
        public StoreModel() : base("store")
        {
            LoadKeys();
        }

        public bool IsItemPurchased(string key)
        {
            return 1 == GetInt("purchased_" + key);
        }

        public void MarkItemAsPurchased(string key)
        {
            Set("purchased_" + key, 1);
        }

        public int GetRvPurchaseProgress(string key)
        {
            return GetInt("rv_progress_" + key);
        }

        public int IncRvPurchaseProgress(string key)
        {
            var newValue = GetInt("rv_progress_" + key) + 1;
            Set("rv_progress_" + key, newValue);
            return newValue;
        }
    }
}