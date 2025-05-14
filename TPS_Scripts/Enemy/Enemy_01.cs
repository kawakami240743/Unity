using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Collections;
using Game;
using RayFire;
using System.Buffers.Text;

public class Enemy_01 : MonoBehaviour
{
    [Header("基本ステータス")]
    [SerializeField] public EnemyStatus enemyStatus;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private HPSlider hpslider;

    private float enemyHP;

    [Header("移動ポイント設定")]
    [SerializeField] private Transform[] targetObjects;  // 移動ポイントのリスト
    private int currentTargetIndex = 0;                 // 現在の移動ポイント
    private float arrivalThreshold = 0.5f;              // 到達判定用の閾値


    [Header("プレイヤー設定")]
    private Rigidbody rb;
    private Transform playerTransform;

    [Header("アニメーター設定")]
    private Animator animator;
    private float FollowDistance = 20f;

    [Header("視野角設定")]
    [SerializeField] private float viewAngle = 60f; // 視野角
    [SerializeField] private float viewDistance = 15f; // 視認距離
    [SerializeField] private LayerMask obstacleMask; // 障害物レイヤー
    private bool playerInSight = false;
    private float sightLostTimer = 0f;
    [SerializeField] private float sightLostDuration = 2f; // プレイヤーを見失っても2秒間は追跡

    [Header("攻撃設定")]
    public int attackType;
    [SerializeField] private float distancePlayer;

    [Header("攻撃距離とタイマー")]
    private bool isAttack;
    [SerializeField] private float attackTime;
    [SerializeField] private float jumpAttackDistance = 8f;
    [SerializeField] private float punchAttackDistance = 4f;
    [SerializeField] private float kickAttackDistance = 3f;
    [SerializeField] private float jumpAttackTime;
    [SerializeField] private float punchAttackTime;
    [SerializeField] private float kickAttackTime;
    [SerializeField] private float attackRotation = 30f;

    [Header("攻撃コライダー設定")]
    [SerializeField] private Collider rightHandCollider;
    [SerializeField] private Collider leftHandCollider;
    [SerializeField] private Collider rightLegCollider;
    [SerializeField] private Collider leftLegCollider;
    [SerializeField] private Collider jumpAttackCollider;

    [Header("攻撃判定")]
    [SerializeField] private GameObject jumpAttackField;
    [SerializeField] private GameObject punchAttackField;
    [SerializeField] private GameObject kickAttackField;

    [Header("死亡時設定")]
    private bool isDying;
    private float dyingTImer = 4.5f;
    public static event Action OnEnemyDefeated; // イベントを発行

    [Header("難易度調整")]
    private int enemyDifficulty;
    private DFManager dfManager;


    private void Awake()
    {
        // EnemyStatus の初期設定
        enemyStatus = new EnemyStatus(1000, 10, 7, 6);

        // EnemyStatus からステータスを取得
        moveSpeed = enemyStatus.move;
        rotationSpeed = enemyStatus.rtn;
        enemyHP = enemyStatus.hp;
    }

    void Start()
    {
        // クラスと機能のnullチェック
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("リジッドボディが存在していません");
        }

        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("アニメーターが存在していません");
        }

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


        dfManager = FindFirstObjectByType<DFManager>();

        if (dfManager != null)
        {
            enemyDifficulty = dfManager.GetCurrentDifficulty();
        }

        AdjustEnemyStats();


        if (hpslider == null)
        {
            Debug.LogError("HPバーが存在してません");
        }

        else
        {
            hpslider.SetTarget(enemyHP);
        }

        // コライダーのnullチェック
        InitializeCollider(rightHandCollider);
        InitializeCollider(leftHandCollider);
        InitializeCollider(rightLegCollider);
        InitializeCollider(leftLegCollider);
        InitializeCollider(jumpAttackCollider);

        CheckAttackField(jumpAttackField);
        CheckAttackField(punchAttackField);
        CheckAttackField(kickAttackField);


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

        Debug.Log("現在のHP" +  enemyHP);
    }

    private void InitializeCollider(Collider collider)
    {
        if (collider != null)
        {
            collider.enabled = false;
        }
        else
        {
            Debug.LogError("Collider が設定されていません");
        }
    }

    private void CheckAttackField(GameObject attackField)
    {
        if (attackField != null)
        {
            attackField.SetActive(false);
        }

        else
        {
            Debug.LogError("attackFieldが設定されていません");
        }
    }

    private void FixedUpdate()
    {
        if (!isDying)
        {
            distancePlayer = Vector3.Distance(transform.position, playerTransform.position);
            playerInSight = CheckForPlayer();

            // プレイヤーを視認したらタイマーをリセット
            if (playerInSight)
            {
                sightLostTimer = 0f;
            }
            else
            {
                sightLostTimer += Time.fixedDeltaTime;
            }

            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

            if (isAttack)
            {
                animator.SetFloat("Speed", 0f, 0.1f, Time.fixedDeltaTime);
                return;
            }

            if (distancePlayer <= jumpAttackDistance)
            {
                if (attackType == 0)
                {
                    attackType = DetermineAttackType(distancePlayer);
                }
                PerformAttack(attackType, distancePlayer, directionToPlayer);
            }
            // **視認を失っても 2 秒間は追跡する**
            else if ((playerInSight || sightLostTimer < sightLostDuration) && distancePlayer <= FollowDistance && distancePlayer > jumpAttackDistance)
            {
                FollowPlayer();
            }
            else
            {
                MoveToTarget();
            }
        }
    }

    /// <summary>
    /// プレイヤーが視野角内かつ視認距離内にいるかチェック
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

    private int DetermineAttackType(float distancePlayer)
    {
        if (distancePlayer <= jumpAttackDistance - 1)
        {
            return UnityEngine.Random.Range(1, 3);
        }
        return UnityEngine.Random.Range(1, 4); // 1: パンチ, 2: キック, 3: ジャンプ
    }

    private void PerformAttack(int attackType, float distancePlayer, Vector3 directionToPlayer)
    {
        switch (attackType)
        {
            case 1:
                PunchAttacktoPlayer(distancePlayer, directionToPlayer);
                break;
            case 2:
                KickAttacktoPlayer(distancePlayer, directionToPlayer);
                break;
            case 3:
                JumpAttacktoPlayer(distancePlayer, directionToPlayer);
                break;
        }
    }

    private void PunchAttacktoPlayer(float distancePlayer, Vector3 directionToPlayer)
    {
        if (!isAttack)
        {
            if (distancePlayer >= punchAttackDistance)
            {
                AttackToMove(directionToPlayer);
                animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
            }
            else
            {
                RotateToPlayer(directionToPlayer);
                animator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
                animator.SetTrigger("isPunchAttack");
                leftHandCollider.enabled = true;
                leftLegCollider.enabled = true;
                punchAttackField.SetActive(true);
                isAttack = true;
                StartCoroutine(ResetAttack(punchAttackTime));
            }
        }
    }

    private void KickAttacktoPlayer(float distancePlayer, Vector3 directionToPlayer)
    {
        if (!isAttack)
        {
            if (distancePlayer >= kickAttackDistance)
            {
                AttackToMove(directionToPlayer);
                animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
            }
            else
            {
                RotateToPlayer(directionToPlayer);
                animator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
                animator.SetTrigger("isKickAttack");
                rightLegCollider.enabled = true;
                kickAttackField.SetActive(true);
                isAttack = true;
                StartCoroutine(ResetAttack(kickAttackTime));
            }
        }
    }

    private void JumpAttacktoPlayer(float distancePlayer, Vector3 directionToPlayer)
    {
        if (!isAttack)
        {
            if (distancePlayer >= jumpAttackDistance)
            {
                AttackToMove(directionToPlayer);
                animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
            }
            else
            {
                RotateToPlayer(directionToPlayer);
                animator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
                animator.SetTrigger("isJumpAttack");

                if (jumpAttackField != null)
                {
                    jumpAttackField.SetActive(true); // ジャンプアニメーション開始時に有効化
                }

                rb.isKinematic = true;
                isAttack = true;
                StartCoroutine(EnableJumpAttackCollider(1.5f)); // 0.3秒後にコライダーを有効化
                StartCoroutine(ResetAttack(jumpAttackTime));
            }
        }
    }

    private IEnumerator EnableJumpAttackCollider(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (jumpAttackCollider != null)
        {
            jumpAttackCollider.enabled = true;
            rightHandCollider.enabled = true;
            leftHandCollider.enabled = true;
        }
    }

    private IEnumerator ResetAttack(float time)
    {
        yield return new WaitForSeconds(time);
        attackType = 0;
        isAttack = false;

        // JumpAttackCollider と JumpAttackField を無効化
        if (jumpAttackCollider != null)
        {
            jumpAttackCollider.enabled = false;
        }

        if (jumpAttackField != null)
        {
            jumpAttackField.SetActive(false);
        }

        punchAttackField.SetActive(false);
        kickAttackField.SetActive(false);

        rightHandCollider.enabled = false;
        rightLegCollider.enabled = false;
        leftHandCollider.enabled = false;
        leftLegCollider.enabled = false;
        rb.isKinematic = false;
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

    private void AttackRotateToPlayer(Vector3 directionToPlayer)
    {
        Vector3 rotationPlayer = new Vector3(directionToPlayer.x, 0, directionToPlayer.z).normalized;
        if (rotationPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rotationPlayer);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, attackRotation * Time.fixedDeltaTime);
        }
    }

    private void AttackToMove(Vector3 directionToPlayer)
    {
        AttackRotateToPlayer(directionToPlayer);
        Vector3 movePosition = rb.position + directionToPlayer * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(movePosition);
    }

    private void MoveToTarget()
    {
        if (targetObjects.Length == 0) return;

        Transform target = targetObjects[currentTargetIndex];
        Vector3 direction = (target.position - transform.position).normalized;

        // 回転をスムーズにする
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        animator.SetFloat("Speed", 0.5f);
        // 移動処理
        rb.linearVelocity = direction * moveSpeed/2;

        // 目標地点に到達したら次のポイントへ
        if (Vector3.Distance(transform.position, target.position) <= arrivalThreshold)
        {
            currentTargetIndex = (currentTargetIndex + 1) % targetObjects.Length; // ループ移動
        }
    }

    private void FollowPlayer()
    {
        animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), 1f, Time.fixedDeltaTime * 5f));

        Vector3 directionPlayer = (playerTransform.position - transform.position).normalized;
        RotateToPlayer(directionPlayer);

        // **ガタつきを防ぐため MovePosition ではなく velocity を使う**
        rb.linearVelocity = directionPlayer * moveSpeed;
    }


    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Ground"))
        {
            FollowDistance = distancePlayer + 10f;
            viewAngle = 360;
            viewDistance = distancePlayer + 10f;
            if (distancePlayer <= 8f)
            {
                FollowDistance = 20f;
                viewAngle = 90;
                viewDistance = 15f;
            }
        }

        // 弾丸に応じたHP減少処理
        if (other.gameObject.CompareTag("SubmachinegunBullet"))
        {
            ApplyDamage(3);
        }
        else if (other.gameObject.CompareTag("ShotGunBullet"))
        {
            ApplyDamage(10);
        }
        else if (other.gameObject.CompareTag("HandGunBullet"))
        {
            ApplyDamage(20);
        }
        else if (other.gameObject.CompareTag("AssalutBullet"))
        {
            ApplyDamage(7);
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

    public void TakeDamage(float damage)
    {
        ApplyDamage(damage);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("近接攻撃を受けました！");
        }
    }

    private void ApplyDamage(float damage)
    {
        if (!isDying)
        {
            enemyHP -= damage;
            hpslider.Damage(damage);

            if (enemyHP <= 0)
            {
                EnemyDestroyd();
            }
        }

    }

    private void EnemyDestroyd()
    {

        animator.SetTrigger("isDying");
        isDying = true;
        StartCoroutine(DyingEnemy());
    }

    private IEnumerator DyingEnemy()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        player.AddGire();
        yield return new WaitForSeconds(dyingTImer);
        Destroy(gameObject);
    }
}
