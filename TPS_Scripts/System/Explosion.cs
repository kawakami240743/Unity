using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour
{
    public float explosionDelay = 3f; // 💥 **爆発までの時間**
    public float explosionRadius = 5f; // 🌍 **爆発範囲**
    public float explosionForce = 700f; // 💨 **爆風の強さ**
    public int explosionDamage = 50; // 🔥 **爆発ダメージ**
    public GameObject explosionEffect; // 💥 **爆発エフェクト**
    public AudioSource audioSouce;
    public AudioClip se;

    private bool hasExploded = false;

    void Start()
    {
       audioSouce = GetComponent<AudioSource>();

        StartCoroutine(ExplodeAfterDelay());
    }

    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // 💥 **爆発エフェクトを生成**
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            audioSouce.PlayOneShot(se);
        }

        // 💥 **爆発範囲内のオブジェクトを取得**
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearby in colliders)
        {
            Rigidbody rb = nearby.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        // 💥 **爆発後にオブジェクトを削除**
        StartCoroutine(ObjectDestroy());
    }

    private IEnumerator ObjectDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }


    void OnCollisionEnter(Collision collision)
    {
        // 💥 **地面に当たったら爆発（オプション）**
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            Explode();
        }
    }
}
