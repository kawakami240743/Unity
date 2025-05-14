using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Game;

public class BossEnemy : MonoBehaviour
{
    //名前
    public BossEnemyInfo BossInfo;　//ステータス
    private Transform player;
    public GameManager gameManager;
    private PlayerControl playerControl;
    private Collider2D enemyCollider;
    private BoxCollider2D AtkCollider;

    //スピードの調整
    private float normalSpeed;
    private float chargeSpeed;

    //攻撃の調整
    //近距離
    private bool BossFire = false;
    private enum BossState { Normal, Charging, Pausing }
    private BossState currentState = BossState.Normal;

    //遠距離
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float attackTimer = 5f;
    [SerializeField] int bulletCount = 3;
    [SerializeField] float bulletSpeed = 20f; // 弾の速度を設定

    //UIの調整
    public Canvas BossCanvas;
    public Slider BossHpSlider;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // プレイヤーを取得
        gameManager = FindFirstObjectByType<GameManager>();
        playerControl = player.GetComponent<PlayerControl>(); // プレイヤーのPlayerControllerを取得
        enemyCollider = GetComponent<Collider2D>(); // 自分のColliderを取得
        AtkCollider = GetComponent<BoxCollider2D>();
        AtkCollider.enabled = false;

        //ステータス
        BossInfo = new BossEnemyInfo();
        BossInfo.name = "BossBat";
        BossInfo.hp = 70;
        BossInfo.atk = 2;
        BossInfo.spd = 4;
        normalSpeed = BossInfo.spd;
        chargeSpeed = BossInfo.spd * 3;

        //HPゲージの初期
        BossHpSlider.value = 85;

        StartCoroutine(BossBehavior());
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            float yRotation = targetRotation.eulerAngles.y;

            if (direction.x < 0)
            {
                yRotation += 180f;
            }

            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            float currentSpeed = currentState == BossState.Charging ? chargeSpeed : normalSpeed;
            transform.position = Vector2.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);

            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f && BossFire == false)
            {
                BossFire = true;
                StartCoroutine (BossBulletFire());
                attackTimer = 5f;
            }
        }

        if (BossInfo.hp <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy Destroyed!");
        }

        if (player == null)
        {
            return;
        }

        //ボスHPの減少
        BossHpSlider.value = BossInfo.hp;
    }

    private IEnumerator BossBulletFire()
    {
        Debug.Log("BossBulletFire called"); // デバッグログを追加

        for (int i = 0; i < bulletCount; i++)
        {
            if (bulletPrefab != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.GetComponent<BossBullet>().SetSpeed(bulletSpeed); // 弾の方向と速度を設定
                yield return new WaitForSeconds(0.1f); // 0.1秒待つ
            }
        }

        BossFire = false;
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
        if (other.CompareTag("Bullet"))
        {
            if (playerControl != null && playerControl.playerInfo != null)
            {
                BossInfo.hp -= playerControl.playerInfo.atk;
                Debug.Log("BOSS HP: " + BossInfo.hp);
                if (BossInfo.hp <= 0)
                {
                    StartCoroutine(BossEnemyDie());
                }
            }
        }

        else if (other.CompareTag("Player") && playerControl.playerInfo.hp <= 0)
        {
            enemyCollider.enabled = false;
        }
    }

    private IEnumerator BossBehavior()
    {
        while (true)
        {
            currentState = BossState.Normal;
            yield return new WaitForSeconds(4f);

            currentState = BossState.Charging;
            yield return new WaitForSeconds(2f);

            currentState = BossState.Pausing;
            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator BossEnemyDie()
    {
        if (gameManager != null)
        {
            Debug.Log("GameManagerに転送します");
            gameManager.OnBossEnemyDestroyed();
        }

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
        Debug.Log("Enemy Destroyed!");
    }
}
