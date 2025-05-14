using UnityEngine;

public class BossBullet : MonoBehaviour
{
    private Transform player;
    private float xMin, xMax, yMin, yMax;
    private Rigidbody2D rb;
    private float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // カメラの境界を取得
        Camera cam = Camera.main;
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        xMin = bottomLeft.x;
        xMax = topRight.x;
        yMin = bottomLeft.y;
        yMax = topRight.y;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
    }

    void Update()
    {
        Vector3 position = transform.position;

        // オブジェクトが画面外に出たかどうかをチェック
        if (position.x < xMin || position.x > xMax || position.y < yMin || position.y > yMax)
        {
            Destroy(gameObject);
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject); // 地面に当たったら弾を消滅させる
        }
    }

    public void SetDirection(Vector2 direction, float speed)
    {
        rb.velocity = direction * speed; // 弾の速度を設定
    }

    public void SetSpeed(float bulletSpeed)
    {
        speed = bulletSpeed; // 弾の速度を設定
    }

}
