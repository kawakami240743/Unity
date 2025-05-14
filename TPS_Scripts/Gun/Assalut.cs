using UnityEngine;

public class Assalut : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gunTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float fireRate = 0.5f;
    private float nextFireTime = 0f;

    private void Start()
    {
        bullet = Resources.Load<GameObject>("AssalutBullet");
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Instantiate(bullet, gunTransform.position, gunTransform.rotation);

            // 0.01秒後に音を鳴らす（直接実行しない）
            Invoke(nameof(PlayGunSound), 0.01f);

            nextFireTime = Time.time + fireRate;
        }
    }

    void PlayGunSound()
    {
        SEManager.Instance.PlaySE("Gun");
    }


}
