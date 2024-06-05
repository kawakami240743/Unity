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

        // 初期位置の Z 座標を 5 に設定
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    private Vector3 direction;
    private float speed;

    public void Initialize(Vector3 dir, float spd)
    {
        direction = dir;
        speed = spd;

        // 方向ベクトルの Z 座標を 0 に設定（Z 軸方向の移動を防ぐため）
        direction.z = 0;
    }

    void Update()
    {
        Vector3 position = transform.position;

        // オブジェクトが画面外に出たかどうかをチェック
        if (position.x < xMin || position.x > xMax || position.y < yMin || position.y > yMax)
        {
            Destroy(gameObject);
        }

        MoveBullet();
    }

    void MoveBullet()
    {
        // 現在の位置を取得し、Z 座標を 5 に固定
        Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;
        newPosition.z = 0f; // Z 座標を 5 に固定
        transform.position = newPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        // 衝突したオブジェクトが敵かどうかをチェック
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Debug.Log("Enemy hit (trigger)!");
        }
    }
}
