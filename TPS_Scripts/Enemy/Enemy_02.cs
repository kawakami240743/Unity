using UnityEngine;
using System;
using System.Collections;
using Game;
using UnityEngine.VFX;

public class Enemy_02 : MonoBehaviour
{
    [Header("基本ステータス")]
    [SerializeField] public EnemyStatus enemyStatus;
    [SerializeField] private float enemyHP;
    [SerializeField] private HPSlider_02 hpslider;

    [Header("距離設定")]
    [SerializeField] private float attackRange = 20f;
    [SerializeField] private float followDistance = 40f;
    private Transform playerTransform;
    [SerializeField] private float distanceToPlayer;

    [Header("移動ポイント設定")]
    [SerializeField] private Transform[] targetObjects;  // 移動ポイントリスト
    private int currentTargetIndex = 0; // 現在の移動ポイント
    private bool isReversing = false;   // 逆走中かどうかのフラグ
    private float arrivalThreshold = 0.5f; // 到達判定用の閾値
    private bool isWaiting = false; // 停止中かどうか
    [SerializeField] private float stopTime = 2f; // 端での停止時間

    [Header("攻撃設定")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform headTransform;
    [SerializeField] private float fireRate = 0.5f; // 発射間隔
    private float fireRateTimer = 0f;
    [SerializeField] private float attackDuration = 5f; // 攻撃時間
    [SerializeField] private float reloadDuration = 2f; // リロード時間
    [SerializeField] private float attackTimer = 0f;
    [SerializeField] private float reloadTimer = 0f;
    private bool isReloading = false;
    private float speedModifierTimer = 0f; // 移動速度半減のタイマー

    [Header("視野角設定")] // 🔹 視野角追加
    [SerializeField] private float viewAngle = 60f; // 視野角
    [SerializeField] private float viewDistance = 15f; // 視認距離
    [SerializeField] private LayerMask obstacleMask; // 障害物レイヤー
    private bool playerInSight = false;
    private float sightLostTimer = 0f;
    [SerializeField] private float sightLostDuration = 2f; // 視認を失っても2秒間は追跡

    [Header("移動設定")]
    private Rigidbody rb;
    private Animator animator;

    private bool isDying;
    public static event Action OnEnemyDefeated; // イベントを発行

    [Header("難易度調整")]
    private int enemyDifficulty;
    private DFManager dfManager;

    private void Awake()
    {
        // 初期設定
        enemyStatus = new EnemyStatus(2000, 10, 6, 5);
        enemyHP = enemyStatus.hp;
    }

    private void Start()
    {
        dfManager = FindFirstObjectByType<DFManager>();

        if (dfManager != null)
        {
            enemyDifficulty = dfManager.GetCurrentDifficulty();
        }

        AdjustEnemyStats();

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (rb == null) Debug.LogError("リジッドボディが存在していません");
        if (animator == null) Debug.LogError("アニメーターが存在していません");

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("プレイヤーが存在していません");
        }

        hpslider.GetComponent<HPSlider_02>();
        if (hpslider != null)
        {
            hpslider.SetTarget(enemyHP);
        }

        else
        {
            Debug.LogError("スライダーが存在していません");
        }

    }

    private void FixedUpdate()
    {
        if (isDying || playerTransform == null) return;

        distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        playerInSight = CheckForPlayer(); // 🔹 視野角チェックを追加

        if (isReloading)
        {
            HandleReload();
            return;
        }

        if (fireRateTimer > 0)
        {
            fireRateTimer -= Time.fixedDeltaTime;
        }

        if (speedModifierTimer > 0)
        {
            speedModifierTimer -= Time.fixedDeltaTime;
        }

        // 🔹 プレイヤーを視認したらタイマーをリセット
        if (playerInSight)
        {
            sightLostTimer = 0f;
        }
        else
        {
            sightLostTimer += Time.fixedDeltaTime;
        }

        if (distanceToPlayer <= attackRange)
        {
            AttackMove();
        }
        else if (distanceToPlayer <= followDistance && distanceToPlayer > attackRange && playerInSight)
        {
            FollowPlayer();
        }
        else
        {
            MoveToTarget();
        }
    }

    private void AdjustEnemyStats()
    {
        // 難易度に応じてエネミーのステータスを変更する処理
        switch (enemyDifficulty)
        {
            case 1: // Easy
                enemyHP *= 0.75f;
                break;
            case 2: // Normal
                // 変更なし
                break;
            case 3: // Hard
                enemyHP *= 1.5f;
                break;
        }



    }

    /// <summary>
    /// 🔹 プレイヤーが視野角内かつ視認距離内にいるかチェック
    /// </summary>
    private bool CheckForPlayer()
    {
        if (playerTransform == null) return false;

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > viewDistance) return false;

        Vector3 forward = transform.forward;
        Vector3 directionNormalized = directionToPlayer.normalized;
        float angleToPlayer = Vector3.Angle(forward, directionNormalized);

        if (angleToPlayer > viewAngle / 2) return false;

        // Raycastで障害物をチェック
        if (!Physics.Raycast(transform.position, directionNormalized, distanceToPlayer, obstacleMask))
        {
            return true; // 視認成功
        }

        return false;
    }

    private void AttackMove()
    {
        animator.SetBool("isAttack", true);
        RotateTowardsPlayer();

        if (fireRateTimer <= 0f)
        {
            if (bulletPrefab != null && headTransform != null)
            {
                Instantiate(bulletPrefab, headTransform.position, headTransform.rotation);
                fireRateTimer = fireRate; // 発射間隔をリセット
                speedModifierTimer = 5f; // 移動速度を半減するタイマーをリセット
            }
            else
            {
                Debug.LogError("[ERROR] 弾または頭のTransformが設定されていません。");
            }
        }

        attackTimer += Time.fixedDeltaTime;

        if (attackTimer >= attackDuration)
        {
            StartReloading();
        }
    }

    private void StartReloading()
    {
        isReloading = true;
        reloadTimer = reloadDuration;
        attackTimer = 0f;
        animator.SetBool("isAttack", false);
        animator.SetBool("isReload", true);
    }

    private void HandleReload()
    {
        reloadTimer -= Time.fixedDeltaTime;

        if (reloadTimer <= 0f)
        {
            isReloading = false;
            animator.SetBool("isReload", false);
        }
    }

    private void FollowPlayer()
    {
        if (isReloading) return; // リロードが終わってから追いかける

        animator.SetBool("isAttack", false);
        float speedModifier = speedModifierTimer > 0 ? 0.5f : 1f; // 移動速度を半減
        animator.SetFloat("Speed", speedModifier);

        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        Vector3 movePosition = rb.position + directionToPlayer * enemyStatus.move * speedModifier * Time.fixedDeltaTime;
        rb.MovePosition(movePosition);

        RotateTowardsPlayer();
    }

    /// <summary>
    /// 目標地点に向かって移動する処理
    /// </summary>
    private void MoveToTarget()
    {
        animator.SetBool("isAttack", false);
        if (targetObjects.Length == 0) return;

        Transform target = targetObjects[currentTargetIndex];
        Vector3 direction = (target.position - transform.position).normalized;

        // 回転をスムーズにする
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, enemyStatus.rtn * Time.fixedDeltaTime);

        animator.SetFloat("Speed", 0.5f);
        // 移動処理
        rb.linearVelocity = direction * enemyStatus.move/2;

        // 目標地点に到達したら処理
        if (Vector3.Distance(transform.position, target.position) <= arrivalThreshold)
        {
            HandleArrival();
        }
    }

    /// <summary>
    /// 目標地点に到達した際の処理（停止・方向転換）
    /// </summary>
    private void HandleArrival()
    {
        if (!isReversing)
        {
            // 順方向へ
            currentTargetIndex++;
            if (currentTargetIndex >= targetObjects.Length)
            {
                currentTargetIndex = targetObjects.Length - 2; // 最後の1つ手前に戻る
                isReversing = true; // 逆方向へ
            }
        }
        else
        {
            // 逆方向へ
            currentTargetIndex--;
            if (currentTargetIndex < 0)
            {
                currentTargetIndex = 1; // 2番目のターゲットへ
                isReversing = false; // 順方向へ
            }
        }
    }


    private void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        Vector3 flatDirection = new Vector3(directionToPlayer.x, 0, directionToPlayer.z).normalized;

        if (flatDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, enemyStatus.rtn * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enemyHP <= 0) return;

        if (!collision.gameObject.CompareTag("Ground"))
        {
            followDistance = followDistance + 10;
            if (followDistance <= 40)
            {
                followDistance = 40;
            }
        }

        if (collision.gameObject.CompareTag("AssalutBullet"))
        {
            Damage(7);
        }
        else if (collision.gameObject.CompareTag("HandGunBullet"))
        {
            Damage(20);
        }
        else if (collision.gameObject.CompareTag("ShotGunBullet"))
        {
            Damage(5);
        }
        else if (collision.gameObject.CompareTag("SubmachinegunBullet"))
        {
            Damage(3);
        }

        else if (collision.gameObject.CompareTag("grenade"))
        {
            Damage(25);
        }

        else if (collision.gameObject.CompareTag("stone"))
        {
            Damage(5);
        }
    }

    public void Damage(float damage)
    {
        if (!isDying)
        {
            enemyHP -= damage;

            if (hpslider != null)
            {
                hpslider.Damage(enemyHP);
            }

            if (enemyHP <= 0)
            {
                StartCoroutine(HandleDeath());
            }
        }
    }

    private IEnumerator HandleDeath()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        player.AddGire();
        isDying = true;
        animator.SetTrigger("isDying");
        yield return new WaitForSeconds(3f); // アニメーション終了まで待機
        Destroy(gameObject);
    }
}
