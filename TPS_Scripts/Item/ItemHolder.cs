using UnityEngine;
using System;
using System.Collections.Generic;
using MyGame.Items;

public class ItemHolder : MonoBehaviour
{
    private Dictionary<string, Item> items = new Dictionary<string, Item>();

    // 🔹 **アイテム変更イベント**
    public event Action OnItemChanged;

    void Awake()
    {
        items["small"] = new Item("band@2x", 60); // 🔥 スプライトを `null` にする
        items["medium"] = new Item("potion@2x", 120);
        items["large"] = new Item("first-aid@2x", 200);

        items["small"].AddQuantity(1);
    }

    public void AddItem(string itemName)
    {
        if (items.ContainsKey(itemName))
        {
            if (items[itemName].Quantity >= Item.MaxQuantity)
            {
                Debug.LogWarning($"⚠️ {itemName} はこれ以上持てません！（所持上限: {Item.MaxQuantity}）");
                return; // **🔹 所持上限に達している場合は追加しない**
            }

            items[itemName].AddQuantity(1);
            Debug.Log($"✅ アイテム追加: {itemName} (合計: {items[itemName].Quantity})");

            OnItemChanged?.Invoke(); // **🔹 UI に変更を通知**
        }
        else
        {
            Debug.LogError($"⚠️ 未知のアイテム: {itemName}");
        }
    }

    public bool UseItem(string itemName, GameObject user)
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogError("🚨 `itemName` が `null` または 空文字です！");
            return false;
        }

        if (!items.ContainsKey(itemName))
        {
            Debug.LogError($"🚨 `items` に `{itemName}` が存在しません！");
            return false;
        }

        if (items[itemName] == null)
        {
            Debug.LogError($"🚨 `items[{itemName}]` が `null` です！");
            return false;
        }

        Debug.Log($"🛠️ {itemName} の使用開始！ 残り: {items[itemName].Quantity}");

        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player == null)
        {
            Debug.LogError("🚨 `PlayerController` が見つかりません！");
            return false;
        }

        if (player.IsHPFull())
        {
            Debug.Log($"⚠️ {itemName} を使えません！HPが最大です！");
            return false;
        }

        if (items[itemName].UseQuantity())
        {
            int healAmount = items[itemName].HealAmount;
            player.Heal(healAmount);

            Debug.Log($"✅ {itemName} を使用！ {healAmount} 回復！ 残り: {items[itemName].Quantity}");
            OnItemChanged?.Invoke();
            return true;
        }

        return false;
    }



    public Dictionary<string, Item> GetItems()
    {
        return items; // **アイテム一覧を返す**
    }
}
