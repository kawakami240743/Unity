using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MyGame.Grenades;

public class GSlotUI : MonoBehaviour
{
    public Image icon;
    public Text quantityText;
    private GrenadeHolder grenadeHolder;
    private Grenade assignedGrenade;
    private Sprite originalSprite; // ✅ 元のスプライトを保存
    public Sprite hoverSprite; // 🔹 ホバー時のスプライト（Inspector で設定）

    void Awake()
    {
        grenadeHolder = FindFirstObjectByType<GrenadeHolder>();
        if (grenadeHolder == null)
        {
            Debug.LogError("🚨 `GrenadeHolder` が見つかりません！");
            return;
        }

        grenadeHolder.OnGrenadeChanged += UpdateSlotUI;
        UpdateSlotUI();
    }

    // 🔹 **特定のグレネードをこのスロットに割り当てる**
    public void AssignGrenade(Grenade grenade)
    {
        assignedGrenade = grenade;
        originalSprite = icon.sprite; // ✅ **初回のみ保存**
        UpdateSlotUI();
    }


    void UpdateSlotUI()
    {
        if (grenadeHolder == null || assignedGrenade == null) return;

        Dictionary<string, Grenade> grenades = grenadeHolder.GetGrenades();

        if (grenades.ContainsKey(assignedGrenade.Name) && grenades[assignedGrenade.Name].Quantity > 0)
        {
            // ✅ **アイコンの更新処理を削除！**
            quantityText.text = $"{grenades[assignedGrenade.Name].Quantity}";
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

    public void Deselect()
    {
        icon.sprite = originalSprite;
    }
}
