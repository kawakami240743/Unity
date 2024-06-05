using UnityEngine;

public class OutOfBoundsDestroyer : MonoBehaviour
{
    private Vector2 direction;
    private float speed;

    public void Initialize(Vector2 dir, float spd)
    {
        direction = dir;
        speed = spd;
    }

    void Update()
    {
        Vector3 position = transform.position;

        //衝突したオブジェクトが敵かどうかをチェック
        MoveBullet();
    }

    void MoveBullet()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 衝突したオブジェクトが敵かどうかをチェック
        if (other.CompareTag("Bullet"))
        {
            Destroy(gameObject);
            Debug.Log("OH!");
        }
    }
}

