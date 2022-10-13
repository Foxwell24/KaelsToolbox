using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KaelsToolbox.Math
{
    public static class IEnumerableExtensions
    {

        // ** Implementation ** //
        //Dictionary<string, float> foo = new Dictionary<string, float>();
        //foo.Add("Item 25% 1", 0.5f);
        //foo.Add("Item 25% 2", 0.5f);
        //foo.Add("Item 50%", 1f);

        //for (int i = 0; i < 10; i++)// ** "(e => e.Value)" says that the weight is the value part ** //
        //    Console.WriteLine(this, "Item Chosen {0}", foo.RandomElementByWeight(e => e.Value).Key);
        public static T RandomElementByWeight<T>(this IEnumerable<T> sequence, Func<T, float> weightSelector)
        {
            float totalWeight = sequence.Sum(weightSelector);
            // The weight we are after...
            float itemWeightIndex = (float)new Random().NextDouble() * totalWeight;
            float currentWeightIndex = 0;

            foreach (var item in from weightedItem in sequence select new { Value = weightedItem, Weight = weightSelector(weightedItem) })
            {
                currentWeightIndex += item.Weight;

                // If we've hit or passed the weight we are after for this item then it's the one we want....
                if (currentWeightIndex >= itemWeightIndex)
                    return item.Value;

            }

            return default(T);

        }
    }
}
