using UnityEngine;
using System;
using System.Collections.Generic;
using MyGame.Grenades;

public class GrenadeHolder : MonoBehaviour
{
    private Dictionary<string, Grenade> grenades = new Dictionary<string, Grenade>();

    // 🔹 グレネード変更イベント（UI更新用）
    public event Action OnGrenadeChanged;

    void Awake()
    {
        // 🔥 スプライトのロードを削除し、`null` にする
        grenades["grenade"] = new Grenade("bom@2x", 100, Resources.Load<GameObject>("Prefabs/Grenade"));
        grenades["stone"] = new Grenade("stone@2x", 20, Resources.Load<GameObject>("Prefabs/Stone"));

        grenades["grenade"].AddQuantity(1); // **最初に1つ持たせる**
    }

    /// <summary>
    /// 📌 **グレネードを追加する**
    /// </summary>
    public void AddGrenade(string grenadeName)
    {
        if (grenades.ContainsKey(grenadeName))
        {
            if (grenades[grenadeName].Quantity >= Grenade.MaxQuantity)
            {
                Debug.LogWarning($"⚠️ {grenadeName} はこれ以上持てません！（上限: {Grenade.MaxQuantity}）");
                return;
            }

            grenades[grenadeName].AddQuantity(1);
            Debug.Log($"✅ グレネード追加: {grenadeName} (合計: {grenades[grenadeName].Quantity})");

            // 🔹 **拾ったら UI を更新**
            OnGrenadeChanged?.Invoke();
        }
        else
        {
            Debug.LogError($"⚠️ 未知のアイテム: {grenadeName}");
        }
    }


    /// <summary>
    /// 📌 **投擲時にグレネードの所持数を減らす**
    /// </summary>
    public void UseGrenade(string grenadeName)
    {
        if (grenades.ContainsKey(grenadeName))
        {
            if (grenades[grenadeName].Quantity > 0)
            {
                grenades[grenadeName].Quantity--;
                Debug.Log($"💥 `{grenadeName}` を投げた！残り: {grenades[grenadeName].Quantity}");

                // 🔹 **投擲後に UI を更新**
                OnGrenadeChanged?.Invoke();
            }
            else
            {
                Debug.LogWarning($"⚠️ `{grenadeName}` の所持数がゼロ！");
            }
        }
        else
        {
            Debug.LogError($"🚨 `{grenadeName}` は存在しません！");
        }
    }

    /// <summary>
    /// 📌 **全グレネードの情報を取得**
    /// </summary>
    public Dictionary<string, Grenade> GetGrenades()
    {
        return grenades; // **🔹 グレネード一覧を返す**
    }
}
