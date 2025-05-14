using UnityEngine;
using UnityEngine.UI;
using Game;

public class Enemy_01 : MonoBehaviour
{
    //public float points = 1f; // この敵を破壊したときに得られるポイント
    //private PointCounter pointCounter;
    private Transform player; // プレイヤーの位置
    public Enemies_01 enemy_01;
    private PlayerControl playerControl;
    private BoxCollider2D AtkCollider;
    public EnemySpawner enemyspawner;
    public Slider HpSlider_01;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // プレイヤーの位置を取得
        //pointCounter = FindFirstObjectByType<PointCounter>();
        playerControl = player.GetComponent<PlayerControl>(); // プレイヤーのPlayerControllerを取得
        AtkCollider = GetComponent<BoxCollider2D>();
        AtkCollider.enabled = false;
        enemy_01 = new Enemies_01();
        enemy_01.name = "Bat";
        enemy_01.hp = 5;
        enemy_01.atk = 1;
        enemy_01.spd = 5;

        HpSlider_01.value = 5;
    }

    void Update()
    {

        if (player != null)
        {
            // プレイヤーの位置からエネミーの位置への方向ベクトルを計算し、Y軸の回転角度を求める
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            float yRotation = targetRotation.eulerAngles.y;

            if (direction.x < 0)
            {
                yRotation += 180f;
            }

            // Y軸の回転角度を計算する際に、X軸とZ軸の回転角度は固定したままにする
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            // プレイヤーの方向に移動
            transform.position = Vector2.MoveTowards(transform.position, player.position, enemy_01.spd * Time.deltaTime);
        }



        if (enemy_01.hp <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy Destroyed!");
        }

        if(player == null)
        {
            return;
        }

        HpSlider_01.value = enemy_01.hp;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AtkCollider.enabled = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AtkCollider.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 衝突したオブジェクトが弾かどうかをチェック
        if (other.CompareTag("Bullet"))
        {
            if (playerControl != null && playerControl.playerInfo != null) // playerControllerとplayerInfoの存在をチェック
            {
                enemy_01.hp -= playerControl.playerInfo.atk; // プレイヤーの攻撃力を使用する
                Debug.Log("Enemy HP: " + enemy_01.hp);
                if (enemy_01.hp <= 0)
                {
                    enemyspawner.RemoveEnemy_01(this);
                    Destroy(gameObject);
                    Debug.Log("Enemy Destroyed!");
                }
            }
        }
    }
}

/*private void OnDestroy()
{
    if (pointCounter != null)
    {
        pointCounter.AddPoints(points);
    }
}*/