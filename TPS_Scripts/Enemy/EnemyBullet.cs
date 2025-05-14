using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float forcePower = 100f; // 発射の力
    [SerializeField] private float lifetime = 5f;    // 弾の寿命
    private Rigidbody rb;

    void Start()
    {
        // Rigidbodyの取得
        rb = GetComponent<Rigidbody>();

        // プレイヤーの腰（mixamorig:Hips）の位置を取得
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Transform playerHips = player.transform.Find("mixamorig:Hips"); // MixamoのHipsボーンを取得

            if (playerHips != null)
            {
                // Hipsの位置をターゲットに計算
                Vector3 directionToPlayer = (playerHips.position - transform.position).normalized;

                // 腰（Hips）の位置に向けて弾を発射
                rb.AddForce(directionToPlayer * forcePower, ForceMode.Impulse);
            }
            else
            {
                Debug.LogError("Playerにmixamorig:Hipsボーンが見つかりません");
            }
        }
        else
        {
            Debug.LogError("プレイヤーが見つかりません");
        }

        // 一定時間後に弾を削除
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Player hit!");
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
