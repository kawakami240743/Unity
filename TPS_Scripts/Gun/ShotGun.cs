using UnityEngine;

public class ShotGun : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gunTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private int pelletCount = 7;
    private float nextFireTime = 0f;

    private void Start()
    {
        bullet = Resources.Load<GameObject>("ShotGunBullet");
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        nextFireTime += Time.deltaTime;

        if (Input.GetMouseButton(0) && nextFireTime >= fireRate)
        {
            FireShotGun();

            nextFireTime = 0;
        }
    }

    void FireShotGun()
    {
        for (int i = 0; i < pelletCount; i++)
        {
            GameObject fireBullet = Instantiate(bullet, gunTransform.position , gunTransform.rotation);
            Invoke(nameof(PlayGunSound), 0.01f);
        }
    }

    void PlayGunSound()
    {
        SEManager.Instance.PlaySE("ShotGun");
    }
}
