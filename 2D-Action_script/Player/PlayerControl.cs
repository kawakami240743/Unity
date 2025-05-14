using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Game;

public class PlayerControl : MonoBehaviour
{
    //名前
    private GameManager gameManager;
    [SerializeField] private ItemDisplay itemDisplay; // ItemDisplayへの参照をInspectorから設定可能に
    private Rigidbody2D rigidbody2d;
    public Animator animator;
    public Player playerInfo; //ステータス


    //スピードの調整
    private float NomalSpeed = 10f;
    private float ChangeSpeed = 15f;

    //ジャンプの調整
    private float jumpPower = 25.0f;
    public bool isJump;

    //アイテム
    public int UseitemId; // アイテムIDを保持する変数を追加
    private bool GetItems = false;

    //HPの調整
    public Slider HPbar;

    //レイヤー

    private const int Player_Layer = 9; //プレイヤーのレイヤー
    private const int Enemy_Layer = 6; //エネミーのレイヤー
    private const int Boss_Layer = 7; //ボスのレイヤー

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>(); // GameManagerのインスタンスを取得
        itemDisplay = FindFirstObjectByType<ItemDisplay>(); // ItemDisplayのインスタンスを取得
        rigidbody2d = GetComponent<Rigidbody2D>();

        playerInfo = new Player(); // プレイヤーのステータスを初期化
        playerInfo.name = "Player";
        playerInfo.hp = 10;
        playerInfo.atk = 1;
        playerInfo.spd = NomalSpeed;

        HPbar.value = 10;

        Physics2D.IgnoreLayerCollision(Player_Layer, Enemy_Layer, false);
        Physics2D.IgnoreLayerCollision(Player_Layer, Boss_Layer, false);
    }

    void Update()
    {
        if (rigidbody2d == null || playerInfo.hp <= 0)
        {
            return;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerInfo.spd = ChangeSpeed;
        }
        else
        {
            playerInfo.spd = NomalSpeed;

        }

        // 移動
        var horizontal = Input.GetKey(KeyCode.LeftArrow) ? -1 : (Input.GetKey(KeyCode.RightArrow) ? 1 : 0);
        var velocity = new Vector2(horizontal, 0).normalized;
        if(Input.GetKey(KeyCode.LeftArrow) || (Input.GetKey(KeyCode.RightArrow)))
        {
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }
        rigidbody2d.velocity = new Vector2(velocity.x * playerInfo.spd, rigidbody2d.velocity.y);

        // 攻撃
        if (Input.GetKey(KeyCode.F))
        {
            animator.SetTrigger("attack");
        }

        // 上を向くアニメーション
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            animator.SetBool("isLookUp", true);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            animator.SetBool("isLookUp", false);
        }

        // ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)  // ジャンプ中でないことを確認
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpPower); // y方向にジャンプパワーを与える
            Debug.Log("ジャンプ");
            animator.SetBool("isJump", true); // ジャンプ直後は空中にいる状態にする
            isJump = true; // ジャンプ中に設定
        }

        // アイテム使用時の処理
        if (Input.GetKeyDown(KeyCode.E) && GetItems)
        {
            Debug.Log("UsingItem");
            gameManager.ProcessItem(UseitemId);// アイテムIDを渡してGameManagerに通知
            GetItems = false; // アイテムを使用したのでフラグをリセット
            itemDisplay.ItemDestroy();
            Debug.Log("GetItems flag set to false");
        }

        // プレイヤーの位置が画面外に行ったら消滅させる
        if (!IsVisible())
        {
            Destroy(gameObject);
            gameManager.OnPlayerDestroyed();
        }

        if (horizontal != 0)
        {
            transform.rotation = Quaternion.Euler(0, horizontal < 0 ? 180 : 0, 0);
        }

        //HPゲージ
        HPbar.value = playerInfo.hp;
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // ジャンプ
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("isJump", false); // 地面に着地したときにジャンプフラグをリセット
            isJump = false; // 着地時にジャンプフラグをリセット
        }

        //アイテム取得時
        if (collision.gameObject.CompareTag("Items"))
        {
            GetItems = true;
            Items itemComponent = collision.gameObject.GetComponent<Items>();
            if (itemComponent != null)
            {
                UseitemId = itemComponent.itemId; // アイテムのIDを取得
                if (itemDisplay != null)
                {
                    itemDisplay.ItemSet(UseitemId);
                }
                Debug.Log("Itemid:" + UseitemId);
                Debug.Log("GetItems flag set to true");
            }
            else
            {
                Debug.LogError("Items component not found on collided object!");
            }
        }




        if (collision.gameObject.CompareTag("Boss"))
        {
            animator.SetBool("isJump", false); // 地面に着地したときにジャンプフラグをリセット
            isJump = false; // 着地時にジャンプフラグをリセット
        }
    }

    bool IsVisible()
    {
        // プレイヤーの位置が画面内にあるかどうかをチェックする
        Vector3 playerViewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        return playerViewportPosition.x > 0 && playerViewportPosition.x < 1 && playerViewportPosition.y > 0 && playerViewportPosition.y < 1;
    }

    // ダメージを受けた時の処理
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy_01") || other.CompareTag("Enemy_02") || other.CompareTag("Boss") || other.CompareTag("BossBullet"))
        {
            StartCoroutine(InvisibleTime(other.gameObject));
        }
    }

    private IEnumerator InvisibleTime(GameObject other)
    {
        Damage(other);

        Physics2D.IgnoreLayerCollision(Player_Layer, Enemy_Layer, true);
        Physics2D.IgnoreLayerCollision(Player_Layer, Boss_Layer, true);

        yield return new WaitForSeconds(1.5f);

        Physics2D.IgnoreLayerCollision(Player_Layer, Enemy_Layer, false);
        Physics2D.IgnoreLayerCollision(Player_Layer, Boss_Layer, false);
    }

    void Damage(GameObject other)
    {
        animator.SetTrigger("hurt");

        if (other.CompareTag("Enemy_01"))
        {
            Enemy_01 enemyComponent = other.GetComponent<Enemy_01>();
            if (enemyComponent != null && enemyComponent.enemy_01 != null)
            {
                playerInfo.hp -= enemyComponent.enemy_01.atk; // エネミーの攻撃力を使用する
                Debug.Log("Player HP: " + playerInfo.hp);

                if (playerInfo.hp <= 0)
                {
                    PlayerDestroy();
                }
            }
            else
            {
                Debug.LogWarning("Enemy component or EnemyesInfo not found on the collided object!");
            }
        }

        if (other.CompareTag("Enemy_02"))
        {
            Enemy_02 enemyComponent = other.GetComponent<Enemy_02>();
            if (enemyComponent != null && enemyComponent.enemy_02 != null)
            {
                playerInfo.hp -= enemyComponent.enemy_02.atk; // エネミーの攻撃力を使用する
                Debug.Log("Player HP: " + playerInfo.hp);

                if (playerInfo.hp <= 0)
                {
                    PlayerDestroy();
                }
            }
        }

        if (other.CompareTag("Boss"))
        {
            BossEnemy bossComponent = other.GetComponent<BossEnemy>();
            if (bossComponent != null && bossComponent.BossInfo != null)
            {
                playerInfo.hp -= bossComponent.BossInfo.atk; // エネミーの攻撃力を使用する
                Debug.Log("Player HP: " + playerInfo.hp);

                if (playerInfo.hp <= 0)
                {
                    PlayerDestroy();
                }
            }
        }

        if (other.CompareTag("BossBullet"))
        {
            playerInfo.hp -= 2;
            Debug.Log("Player HP:" + playerInfo.hp);

            if (playerInfo.hp <= 0)
            {
                PlayerDestroy();
            }
        }
    }

    void PlayerDestroy()
    {
        gameManager.OnPlayerDestroyed(); // ゲームマネージャーにプレイヤーが破壊されたことを通知
        Destroy(gameObject); // プレイヤーを破壊
    }


}
