using System.Collections.Generic;

namespace Messages
{
    public static class ItemPrices
    {
        public static Dictionary<string, int> Prices { get; } 
                = new Dictionary<string, int>();

        static ItemPrices()
        {
            Prices.Add("iPad", 100);
            Prices.Add("Macbook", 200);
        }
    }
}
