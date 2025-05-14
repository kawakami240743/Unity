using UnityEngine;
using System.Collections;
using RayFire;

public class ImpactDamageHandler : MonoBehaviour
{
    public float impactThreshold = 1.0f; // 衝撃がこれ以上でダメージ発生
    public float waitBeforeShrink = 5.0f; // 縮小を開始する前の待機時間
    public float shrinkDuration = 1.5f; // 縮小する時間
    public float shrinkThreshold = 0.05f; // これ以下のサイズになったら削除

    private void OnCollisionEnter(Collision collision)
    {
        // 衝突の相手オブジェクト
        GameObject otherObject = collision.gameObject;

        // 衝突の強さを取得
        float impactForce = collision.relativeVelocity.magnitude * 3f;

        // 衝撃が一定以下なら処理をしない
        if (impactForce < impactThreshold)
        {
            return;
        }


        if (collision.gameObject.CompareTag("Enemy_01"))
        {
            Enemy_01 enemy = otherObject.GetComponent<Enemy_01>();
            if (enemy != null)
            {
                enemy.TakeDamage(impactForce);
            }
        }

        if (collision.gameObject.CompareTag("Enemy_02"))
        {
            Enemy_02 enemy = otherObject.GetComponent<Enemy_02>();
            if (enemy != null)
            {
                enemy.Damage(impactForce);
            }
        }
        // 敵にダメージを与える

        // 5秒後に縮小＆削除を開始
        StartCoroutine(ShrinkAndDestroy(gameObject));
    }

    // 🛠 破片を 5 秒待ってから縮小→削除するコルーチン
    IEnumerator ShrinkAndDestroy(GameObject obj)
    {
        yield return new WaitForSeconds(waitBeforeShrink); // 5秒待機

        Vector3 originalScale = obj.transform.localScale;
        float timer = 0;

        while (timer < shrinkDuration)
        {
            timer += Time.deltaTime;
            float scaleMultiplier = Mathf.Lerp(1, 0, timer / shrinkDuration);
            obj.transform.localScale = originalScale * scaleMultiplier;

            if (obj.transform.localScale.magnitude < shrinkThreshold)
            {
                Destroy(obj);
                yield break;
            }

            yield return null;
        }

        Destroy(obj);
    }
}
