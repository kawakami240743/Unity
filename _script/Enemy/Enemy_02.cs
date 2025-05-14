using UnityEngine;
using UnityEngine.UI;
using Game;

public class Enemy_02 : MonoBehaviour
{
    private Transform player; // プレイヤーの位置
    public Enemies_02 enemy_02;
    public EnemySpawner enemyspawner;
    private PlayerControl playerControl;
    private BoxCollider2D AtkCollider;
    public Slider HpSlider_02;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // プレイヤーの位置を取得
        playerControl = player.GetComponent<PlayerControl>(); // プレイヤーのPlayerControllerを取得
        AtkCollider = GetComponent<BoxCollider2D>();
        AtkCollider.enabled = false;

        enemy_02 = new Enemies_02
        {
            name = "Dog",
            hp = 3,
            atk = 2,
            spd = 11
        };

        HpSlider_02.value = 3;

    }

    void Update()
    {
        if (player != null)
        {
            // プレイヤーの位置からエネミーの位置への方向ベクトルを計算
            Vector3 direction = (player.position - transform.position).normalized;

            // Y軸の回転角度を計算し、エネミーの向きをプレイヤーに向ける
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            float yRotation = targetRotation.eulerAngles.y;

            if (direction.x < 0)
            {
                yRotation += 180f;
            }

            // Y軸の回転角度を計算する際に、X軸とZ軸の回転角度は固定したままにする
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            // プレイヤーの方向に移動
            transform.position = Vector2.MoveTowards(transform.position, player.position, enemy_02.spd * Time.deltaTime);

        }

        if (enemy_02.hp <= 0)
        {
            enemyspawner.RemoveEnemy_02(this);
            Destroy(gameObject);
            Debug.Log("Enemy Destroyed!");
        }

        HpSlider_02.value = enemy_02.hp;

    }

    void OnCollisionEnter2D(Collision2D collision)
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
                enemy_02.hp -= playerControl.playerInfo.atk; // プレイヤーの攻撃力を使用する
                Debug.Log("Enemy HP: " + enemy_02.hp);
                if (enemy_02.hp <= 0)
                {
                    Destroy(gameObject);
                    Debug.Log("Enemy Destroyed!");
                }
            }
        }
    }
}
