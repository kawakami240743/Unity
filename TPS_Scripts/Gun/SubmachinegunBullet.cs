using UnityEngine;

public class SubmachinegunBullet : MonoBehaviour
{
    [SerializeField] private float forcePower = 30f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float spreadAngle = 5f;
    private Rigidbody rb;
    private float Timer = 5f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

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

    private void OnTriggerEnter(Collider other)
    {
            Destroy(gameObject);
    }
}
