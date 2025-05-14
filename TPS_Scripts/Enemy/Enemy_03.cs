using UnityEngine;
using System;
using System.Collections;
using Game;
using UnityEngine.VFX;

public class Enemy_03 : MonoBehaviour
{
    private EnemyStatus enemyStatus;

    [Header("基本設定")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 1f;
    private float enemyHP;

    [Header("視野角設定")]
    public float viewAngle = 90f;
    public float viewDistance = 15f;
    public LayerMask obstacleMask;
    private bool playerInSight = false;
    public float sightLostTimer = 0f;
    private float sightLostDuration = 2f;

    [Header("攻撃処理")]
    [SerializeField] private float attackDistance = 8f;
    private bool isAttacking = false;
    private float attackStartTime;
    private Vector3 attackDirection;
    private Vector3 lastTargetPosition;

    private enum AttackState { None, Preparing, Dashing, Stopping, Rotating, Recovering }
    private AttackState attackState = AttackState.None;

    private Transform playerTransform;
    private Rigidbody rb;
    private Animator animator;
    [SerializeField] private HPSlider_03 slider_03;
    private bool isDeath;

    private Vector3 wanderDirection;
    private float wanderTimer = 0f;
    private float changeDirectionTime = 3f; // 3秒ごとに方向を変更

    [Header("難易度調整")]
    private int enemyDifficulty;
    private DFManager dfManager;


    void Start()
    {
        dfManager = FindFirstObjectByType<DFManager>();

        if (dfManager != null)
        {
            enemyDifficulty = dfManager.GetCurrentDifficulty();
        }

        AdjustEnemyStats();

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        enemyStatus = new EnemyStatus(50, 10, 3, 1);

        enemyHP = enemyStatus.hp;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("プレイヤーが存在していません");
            return;
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

        Debug.Log("現在のHP" + enemyHP);

    }

    void FixedUpdate()
    {
        if (isDeath) return;

        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        playerInSight = CheckForPlayer();

        if (playerInSight)
        {
            sightLostTimer = 0f;
        }
        else
        {
            sightLostTimer += Time.fixedDeltaTime;
        }

        switch (attackState)
        {
            case AttackState.None:
                if ((playerInSight || sightLostTimer < sightLostDuration) && distanceToPlayer <= attackDistance && !isAttacking)
                {
                    StartAttack();
                }
                else if ((playerInSight || sightLostTimer < sightLostDuration) && distanceToPlayer <= viewDistance)
                {
                    FollowPlayer();
                }
                else if (!playerInSight && sightLostTimer >= sightLostDuration)
                {
                    Wandering();
                }
                break;

            case AttackState.Preparing:
                if (Time.time >= attackStartTime + 0.5f) // **攻撃準備時間**
                {
                    StartDash();
                }
                break;

            case AttackState.Dashing:
                if (Time.time >= attackStartTime + 1.5f) // **突進時間**
                {
                    StopDash();
                }
                break;

            case AttackState.Stopping:
                if (Time.time >= attackStartTime + 0.25f) // **停止時間（硬直）**
                {
                    StartRotating();
                }
                break;

            case AttackState.Rotating:
                if (RotateToTargetSmoothly())
                {
                    StartRecovering();
                }
                break;

            case AttackState.Recovering:
                if (Time.time >= attackStartTime + 0.5f) // **攻撃後の硬直**
                {
                    isAttacking = false;
                    attackState = AttackState.None;
                }
                break;
        }
    }

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

        if (!Physics.Raycast(transform.position, directionNormalized, distanceToPlayer, obstacleMask))
        {
            return true;
        }

        return false;
    }

    private void StartAttack()
    {
        isAttacking = true;
        attackState = AttackState.Preparing;
        attackStartTime = Time.time;

        animator.SetFloat("Speed", 0f);
        animator.speed = 1f;
        animator.SetTrigger("Attack");

        rb.linearVelocity = Vector3.zero;

        // **攻撃開始時のプレイヤーの位置を記録**
        lastTargetPosition = playerTransform.position;
        attackDirection = (lastTargetPosition - transform.position).normalized;
    }

    private void StartDash()
    {
        attackState = AttackState.Dashing;
        attackStartTime = Time.time;

        animator.SetFloat("Speed", 1f);
        animator.speed = 3.0f;
        rb.linearVelocity = attackDirection * moveSpeed * 5.0f;
    }

    private void StopDash()
    {
        attackState = AttackState.Stopping;
        attackStartTime = Time.time;

        rb.linearVelocity = Vector3.zero;
        animator.SetFloat("Speed", 0f);
        animator.speed = 1f;
    }

    private void StartRotating()
    {
        attackState = AttackState.Rotating;
        attackStartTime = Time.time;
    }

    private bool RotateToTargetSmoothly()
    {
        Vector3 directionToTarget = (playerTransform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        // **一定の角度差以下になったら回転完了**
        return Quaternion.Angle(transform.rotation, targetRotation) < 5f;
    }

    private void StartRecovering()
    {
        attackState = AttackState.Recovering;
        attackStartTime = Time.time;
    }

    private void FollowPlayer()
    {
        animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), 1f, Time.fixedDeltaTime * 5f));

        Vector3 directionPlayer = (playerTransform.position - transform.position).normalized;
        RotateToPlayer(directionPlayer);

        rb.linearVelocity = directionPlayer * moveSpeed;
    }

    private void Stop()
    {
        animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), 0f, Time.fixedDeltaTime * 3f));
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime * 5f);
    }

    private void Wandering()
    {
        // 一定時間ごとにランダムな方向へ移動
        wanderTimer -= Time.fixedDeltaTime;
        if (wanderTimer <= 0f)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere;
            randomDirection.y = 0; // 水平方向の移動に限定
            wanderDirection = randomDirection.normalized; // 正規化
            wanderTimer = changeDirectionTime; // タイマーリセット
        }

        Quaternion targetRotation = Quaternion.LookRotation(wanderDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        // アニメーション処理
        animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), 0.5f, Time.fixedDeltaTime * 3f));

        // 自由移動
        rb.linearVelocity = wanderDirection * moveSpeed * 0.5f; // 速度を抑えめに
    }

    private void RotateToPlayer(Vector3 directionToPlayer)
    {
        Vector3 rotationPlayer = new Vector3(directionToPlayer.x, 0, directionToPlayer.z).normalized;
        if (rotationPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rotationPlayer);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // 弾丸に応じたHP減少処理
        if (other.gameObject.CompareTag("SubmachinegunBullet"))
        {
            ApplyDamage(2);
        }
        else if (other.gameObject.CompareTag("ShotGunBullet"))
        {
            ApplyDamage(5);
        }
        else if (other.gameObject.CompareTag("HandGunBullet"))
        {
            ApplyDamage(10);
        }
        else if (other.gameObject.CompareTag("AssalutBullet"))
        {
            ApplyDamage(4);
        }

        else if (other.gameObject.CompareTag("grenade"))
        {
            ApplyDamage(25);
        }

        else if (other.gameObject.CompareTag("stone"))
        {
            ApplyDamage(5);
        }
    }

    private void ApplyDamage(int damage)
    {
        if (isDeath) return;

        enemyHP -= damage;
        slider_03.Damage(enemyHP);

        if (enemyHP <= 0)
        {
            StartCoroutine(EnemyDestroyd());
        }
    }

    private IEnumerator EnemyDestroyd()
    {
        isDeath = true;
        animator.SetTrigger("Death");
        rb.linearVelocity = Vector3.zero;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
