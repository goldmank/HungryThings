using System;

namespace Game.Scripts.Model
{
    public class CurrencyModel : PlayerPrefObject
    {
        public event Action<int> OnCoinsAmountChanged;

        public CurrencyModel() : base("currency")
        {
            LoadKeys();
        }

        public int Coins => GetInt("coins");

        public void AddCoins(int amount)
        {
            var coins = Coins + amount;
            if (coins < 0)
            {
                coins = 0;
            }
            Set("coins", coins);
            OnCoinsAmountChanged?.Invoke(coins);
        }
    }
}