using UnityEngine;
using MyGame.Items;

public class ItemPickup : MonoBehaviour
{
    private bool isPickedUp = false; // 🔹 **アイテムが取得されたかどうかを管理**

    private void OnCollisionEnter(Collision collision)
    {
        if (isPickedUp) return; // **🔹 すでに拾われていたら処理しない**

        if (collision.gameObject.CompareTag("Player"))
        {
            ItemHolder itemHolder = collision.gameObject.GetComponentInParent<ItemHolder>(); // 👈 **`GetComponentInParent()` を使う**
            if (itemHolder != null)
            {
                string randomItem = GetRandomItem();

                int beforeQuantity = itemHolder.GetItems()[randomItem].Quantity; // **取得前の個数を記録**
                itemHolder.AddItem(randomItem);
                int afterQuantity = itemHolder.GetItems()[randomItem].Quantity; // **取得後の個数を記録**

                if (beforeQuantity == afterQuantity)
                {
                    Debug.Log($"⚠️ {collision.gameObject.name} は {randomItem} をこれ以上持てません！");
                    return; // **🔹 取得できなかったら削除しない**
                }

                Debug.Log($"{collision.gameObject.name} がアイテムを獲得: {randomItem}");

                isPickedUp = true; // **🔹 アイテムが取得されたことを記録**
                Destroy(gameObject); // **🔹 即座に削除**
            }
        }
    }

    private string GetRandomItem()
    {
        int rand = Random.Range(0, 3);
        Debug.Log($"🎲 ランダム値: {rand}"); // ✅ 確認用ログ

        switch (rand)
        {
            case 0: return "small";
            case 1: return "medium";
            case 2: return "large";
            default: return "small";
        }
    }
}
