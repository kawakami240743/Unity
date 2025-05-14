namespace MyGame.Grenades
{
    using UnityEngine;

    public class Grenade
    {
        public static int MaxQuantity { get; private set; } = 9;
        public string Name { get; private set; }
        public int Damage { get; private set; }
        public int Quantity { get; set; }
        public GameObject GrenadePrefab { get; private set; } // 🔹 投げるグレネードのプレハブ

        public Grenade(string name, int damage, GameObject grenadePrefab)
        {
            Name = name;
            Damage = damage;
            GrenadePrefab = grenadePrefab;
            Quantity = 0;
        }

        public void AddQuantity(int amount)
        {
            Quantity += amount;
        }

        public bool UseQuantity(Vector3 throwPosition, Vector3 throwDirection)
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
