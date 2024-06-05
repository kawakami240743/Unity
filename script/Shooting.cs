using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab; // 発射する物体Bのプレハブ
    [SerializeField] float projectileSpeed = 20f; // 物体Bの速度
    [SerializeField] float fireRate = 0.07f; // 発射の間隔
    private bool isFiring = false; // 発射中かどうかのフラグ

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider = Physics2D.OverlapPoint(mousePosition);

        if (collider != null && collider.CompareTag("Enemy") && Input.GetMouseButton(0)) // 左クリックを検出
        {
            if (!isFiring)
            {
                StartFiring();
                Debug.Log("Fire");
            }
        }
        else
        {
            if (isFiring)
            {
                StopFiring();
                Debug.Log("stop");
            }
        }
    }

    void StartFiring()
    {
        if (!isFiring)
        {
            isFiring = true;
            StartCoroutine(FireContinuously());
        }
    }

    void StopFiring()
    {
        isFiring = false;
    }

    IEnumerator FireContinuously()
    {
        while (isFiring)
        {
            ShootProjectile();
            yield return new WaitForSeconds(fireRate); // 発射までの間隔は fireRate を参考にする
        }
    }

    void ShootProjectile()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // マウスカーソルの位置を取得
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized; // 発射方向を計算

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity); // 物体Bを生成
        projectile.GetComponent<Fire>().Initialize(direction, projectileSpeed); // 物体Bにスクリプトを追加して、発射方向を設定
    }
}
