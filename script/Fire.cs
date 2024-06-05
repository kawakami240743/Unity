using UnityEngine;

public class Fire : MonoBehaviour
{
    private float xMin, xMax, yMin, yMax;

    void Start()
    {
        // カメラの境界を取得
        Camera cam = Camera.main;
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        xMin = bottomLeft.x;
        xMax = topRight.x;
        yMin = bottomLeft.y;
        yMax = topRight.y;
    }

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

        // オブジェクトが画面外に出たかどうかをチェック
        if (position.x < xMin || position.x > xMax || position.y < yMin || position.y > yMax)
        {
            Destroy(gameObject);
        }
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
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Debug.Log("Enemy hit (trigger)!");
        }
    }
}

