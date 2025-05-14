using UnityEngine;
using System.Collections;
using RayFire;

public class FragmentShrinkAndDestroy : MonoBehaviour
{
    public float shrinkSpeed = 0.5f; // 縮小スピード
    public float destroyThreshold = 0.05f; // 削除するサイズの閾値
    public float delayBeforeShrink = 2f; // 縮小開始までの遅延時間

    // Activation Eventから呼び出されるメソッド
    public void OnActivationEvent()
    {
        // 破片の縮小と削除を開始
        StartCoroutine(ShrinkAndDestroyFragments());
    }

    private IEnumerator ShrinkAndDestroyFragments()
    {
        // 遅延時間の待機
        yield return new WaitForSeconds(delayBeforeShrink);

        // 子オブジェクト（破片）を取得
        foreach (Transform fragment in transform)
        {
            StartCoroutine(ShrinkAndDestroy(fragment));
        }
    }

    private IEnumerator ShrinkAndDestroy(Transform fragment)
    {
        while (fragment != null && fragment.localScale.x > destroyThreshold)
        {
            // 徐々に縮小
            fragment.localScale *= 1 - (shrinkSpeed * Time.deltaTime);
            yield return null;
        }

        // 一定サイズ以下になったら削除
        if (fragment != null)
            Destroy(fragment.gameObject);
    }
}
