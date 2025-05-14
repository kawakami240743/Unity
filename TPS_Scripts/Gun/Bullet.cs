using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float forcePower = 30f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float spreadAngle = 5f;
    private Rigidbody rb;
    private Collider bulletCollider;
    private float Timer = 5f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<Collider>();
        cameraTransform = Camera.main.transform;
        float randomPitch = Random.Range(-spreadAngle, spreadAngle);
        float randomYaw = Random.Range(-spreadAngle, spreadAngle);

        Vector3 angleDirection = Quaternion.Euler(randomPitch, randomYaw, 0) * cameraTransform.forward;

        rb.AddForce(angleDirection * forcePower, ForceMode.Impulse);
    }

    private void Update()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            HitMark.OnHit();
            Debug.Log("エネミーにヒット");
        }

        if (other.gameObject.CompareTag("Enemy_01"))
        {
            Destroy(gameObject);
            HitMark.OnHit();
            Debug.Log("エネミーにヒット");
        }

        if (other.gameObject.CompareTag("Enemy_02"))
        {
            Destroy(gameObject);
            HitMark.OnHit();
            Debug.Log("エネミーにヒット");
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            bulletCollider.isTrigger = false;
        }

        if (!other.gameObject.CompareTag("ShotGunBullet"))
        {
            Debug.Log("HIT");

            Destroy(gameObject);
        }
    }
}
