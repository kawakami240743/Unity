using UnityEngine;
using RayFire;

public class RayfireCollisionDestroyer : MonoBehaviour
{
    private RayfireRigid rf;

    void Start()
    {
        // `RayfireRigid` コンポーネントを取得
        rf = GetComponent<RayfireRigid>();

        if (rf != null)
        {
            Debug.Log("🟢 `RayfireRigid` の衝突待機中: " + gameObject.name);
        }
        else
        {
            Debug.LogError("❌ `RayfireRigid` が見つかりません！");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (rf != null)
        {
            Debug.Log("💥 衝突検知！ `" + gameObject.name + "` に `" + collision.gameObject.name + "` がぶつかった！");

            // `Demolition Type` を確認
            Debug.Log("🚀 `Initialize()` 実行前: `Demolition Type` = " + rf.demolitionType);

            // `Demolition Type` を `Runtime` に強制設定（念のため）
            rf.demolitionType = DemolitionType.Runtime;

            // `Initialize()` を実行
            rf.Initialize();

            // 破壊が実行されたか確認
            Debug.Log("⚡ `Initialize()` 実行後: `Demolition Type` = " + rf.demolitionType);
        }
        else
        {
            Debug.LogError("❌ `OnCollisionEnter` が発火したが、`RayfireRigid` が `null` だった！");
        }
    }
}
