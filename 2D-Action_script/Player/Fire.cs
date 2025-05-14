using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fire : MonoBehaviour
{
    public GameManager gameManager;
    private float xMin, xMax, yMin, yMax;
    private float speed;
    private float HomingDuration;
    private bool isHoming;
    public Shooting shooting;
    private Vector3 direction;
    private Collider2D bulletCollider;
    private Transform target;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        bulletCollider = GetComponent<Collider2D>(); // コライダーの取得

        // カメラの境界を取得
        Camera cam = Camera.main;
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        xMin = bottomLeft.x;
        xMax = topRight.x;
        yMin = bottomLeft.y;
        yMax = topRight.y;

        // 初期位置の Z 座標を 0 に設定
        transform.position = new Vector3(transform.position.x, transform.position.y, -5f);

        if (isHoming)
        {
            StartCoroutine(HomingCoroutine());
        }
    }

    public void Initialize(Vector3 dir, float spd, bool isHoming = false, float HomingDuration = 0f)
    {
        direction = dir.normalized; // 方向ベクトルを正規化する
        speed = spd;
        this.isHoming = isHoming;
        this.HomingDuration = HomingDuration;

        // 方向ベクトルの Z 座標を 0 に設定（Z 軸方向の移動を防ぐため）
        direction.z = 0;

        // プロジェクタイルの向きを設定
        transform.up = direction; // プロジェクタイルの正面方向を設定するので、ここではup方向にdirectionを設定します
        transform.Rotate(0, 0, -90); // 180度回転させて正しい方向を向くようにする
    }

    void Update()
    {
        Vector3 position = transform.position;

        // オブジェクトが画面外に出たかどうかをチェック
        if (position.x < xMin || position.x > xMax || position.y < yMin || position.y > yMax)
        {
            Destroy(gameObject);
        }

        if (!isHoming)
        {
            MoveBullet();
        }

    }

    private void MoveBullet()
    {
        // 現在の位置を取得し、Z 座標を 0 に固定
        Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;
        newPosition.z = 0f; // Z 座標を 0 に固定
        transform.position = newPosition;
    }

    private IEnumerator HomingCoroutine()
    {
        float homingTime = 0f;
        while (homingTime < HomingDuration)
        {
            target = FindClosestEnemy();
            if (target != null)
            {
                Vector3 targetDirection = (target.position - transform.position).normalized;
                direction = Vector3.Lerp(direction, targetDirection, 0.1f);
                transform.right= targetDirection;
                transform.Rotate(0, 0, 180);
            }
            MoveBullet();
            homingTime += Time.deltaTime;
            yield return null;
        }

        isHoming = false;

    }

    private Transform FindClosestEnemy()
    {
        GameObject[] enemies_01 = GameObject.FindGameObjectsWithTag("Enemy_01");
        GameObject[] enemies_02 = GameObject.FindGameObjectsWithTag("Enemy_02");
        GameObject[] boss = GameObject.FindGameObjectsWithTag("Boss");

        List<Transform> allEnemies = new List<Transform>();

        foreach (GameObject enemy in enemies_01)
        {
            allEnemies.Add(enemy.transform);
        }
        foreach (GameObject enemy in enemies_02)
        {
            allEnemies.Add(enemy.transform);
        }
        foreach (GameObject enemy in boss)
        {
            allEnemies.Add(enemy.transform);
        }

        Transform closestEnemy = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(Transform enemy in allEnemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, currentPosition);
            if(distance < minDistance)
            {
                closestEnemy = enemy.transform;
                minDistance = distance;
            }
        }

        return closestEnemy;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        gameManager.Hit_SE();

        // 衝突したオブジェクトが敵かどうかをチェック
        if (other.CompareTag("Enemy_01"))
        {
            Destroy(gameObject);
            Debug.Log("敵に当たったよ!");
            bulletCollider.enabled = false;
        }

        else if (other.CompareTag("Enemy_02"))
        {
            Destroy(gameObject);
            Debug.Log("敵に当たったよ!");
            bulletCollider.enabled = false;
        }

        else if (other.CompareTag("Boss"))
        {
            Destroy(gameObject);
            Debug.Log("ボスに当たったよ!");
            bulletCollider.enabled = false;
        }

        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

    }
}
