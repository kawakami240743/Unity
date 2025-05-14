namespace MyGame.Items
{
    using UnityEngine;

    public class Item
    {
        public static int MaxQuantity { get; private set; } = 9;
        public string Name { get; private set; }
        public int Quantity { get; private set; }
        public int HealAmount { get; private set; } // 🔹 **回復量のみを保持**

        public Item(string name, int healAmount)
        {
            Name = name;
            HealAmount = healAmount;
            Quantity = 0;
        }

        public void AddQuantity(int amount)
        {
            Quantity += amount;
        }

        public bool UseQuantity()
        {
            if (Quantity > 0)
            {
                Quantity--;
                return true;
            }
            return false;
        }
    }
}
