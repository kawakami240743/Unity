using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MyGame.Items;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour
{
    public Image icon;
    public Text quantityText;
    private ItemHolder itemHolder;
    private Item assignedItem; // 🔹 このスロットに対応するアイテム
    private Sprite originalSprite; // ✅ 元のスプライトを保存
    public Sprite hoverSprite; // 🔹 ホバー時のスプライト（Inspector で設定）

    void Awake() // ✅ **Start() → Awake() に変更**
    {
        itemHolder = FindFirstObjectByType<ItemHolder>();
        if (itemHolder == null)
        {
            Debug.LogError("ItemHolder が見つかりません！");
            return;
        }

        itemHolder.OnItemChanged += UpdateSlotUI; // 🔹 **アイテム変更時に UI を更新**
        UpdateSlotUI(); // **初期状態の UI を更新**
    }

    // 🔹 **特定のアイテムをこのスロットに割り当てる**
    public void AssignItem(Item item)
    {
        assignedItem = item;
        originalSprite = icon.sprite; // ✅ 初期状態のスプライトを保存
        UpdateSlotUI();
    }

    void UpdateSlotUI()
    {
        if (itemHolder == null || assignedItem == null) return;

        Dictionary<string, Item> items = itemHolder.GetItems();

        if (items.ContainsKey(assignedItem.Name) && items[assignedItem.Name].Quantity > 0)
        {
            // ✅ **アイコンの更新処理を削除！**
            quantityText.text = $"{items[assignedItem.Name].Quantity}";
        }
        else
        {
            quantityText.text = "0";
        }
    }

    public void Highlight()
    {
        if (hoverSprite != null) icon.sprite = hoverSprite;
    }

    // 🔹 **ハイライト解除時に元のスプライトに戻す**
    public void Deselect()
    {
        icon.sprite = originalSprite;
    }

}





