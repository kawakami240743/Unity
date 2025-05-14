using UnityEngine;

public class HandGun : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gunTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float fireRate = 0.5f;
    private float nextFireTime = 0f;

    private void Start()
    {
        bullet = Resources.Load<GameObject>("HandGunBullet");
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            // 弾丸を生成
            Instantiate(bullet, gunTransform.position, gunTransform.rotation);
            Invoke(nameof(PlayGunSound), 0.01f);

            // 次の発射可能時間を更新
            nextFireTime = Time.time + fireRate;
        }
    }

    void PlayGunSound()
    {
        SEManager.Instance.PlaySE("Gun");
    }
}
