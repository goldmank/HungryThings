using System.Linq;
using UnityEngine;

namespace Game.Scripts.Infra
{
    public class ValueDistributor
    {
        public static T GetRandomValue<T>(IDistributable[] values)
        {
            var totalAmount = 0;
            foreach (var value in values)
            {
                totalAmount += value.Amount;
            }

            var randValue = Random.Range(0, totalAmount);
            foreach (var value in values)
            {
                randValue -= value.Amount;
                if (randValue <= 0)
                {
                    return (T)value.Value;
                }
            }

            return (T)values.Last().Value;
        }
    }
}