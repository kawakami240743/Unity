using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyMove : MonoBehaviour
{
    //エネミーの強さ・数字の設定
    [SerializeField] private int minPower = 1;
    [SerializeField] private int maxPower = 99999;
    [SerializeField] private TMP_Text enemyPowerText;
    public int enemyPower;

    //エネミーの向かう場所の設定・移動
    private Vector3 targetPosition;
    [SerializeField] private float moveInterval = 10f;
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float enemySpeed = 2f;
    [SerializeField] private float rotationSpeed = 8f;
    private float timer;

    //プレイヤーの位置の取得・追跡
    public Controller player;
    private Transform playerTransform;
    [SerializeField] private float playerDetection = 10f;
    [SerializeField] private float viewingAngle = 90f;

    private Animator animator;
    private AudioSource audioSource;
    [SerializeField] private AudioClip se;

    private void Start()
    {
        //コンポーネントが存在しているかの確認
        playerTransform = GameObject.FindWithTag("Player").transform;
        if (playerTransform == null || GameObject.FindWithTag("Player") == null)
        {
            Debug.LogError("プレイヤーが存在していません");
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("アニメーターが存在していません");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("オーディオが存在していません");
        }

        //エネミーが最初に取る行動
        GetRandomPower();
        UpdateUI();
        SetRandomPosition();
    }

    private void Update()
    {
        //エネミーの移動時間の設定
        timer += Time.deltaTime;

        if (playerTransform == null)
        {
            if (timer >= moveInterval)
            {
                SetRandomPosition();
                timer = 0f;
            }
        }

        //プレイヤーの位置の確認
        if (IsPlayerInFront())
        {
            targetPosition = playerTransform.position;
            MoveToTarget();
        }

        //もし違ったらランダム移動の実行
        else
        {
            if (timer >= moveInterval)
            {
                SetRandomPosition();
                timer = 0f;
            }

            MoveToTarget();
        }
    }

    //プレイヤーが前方にいるかの確認
    private bool IsPlayerInFront()
    {
        if (playerTransform == null || playerTransform.gameObject == null) return false;

        float distancetoPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distancetoPlayer > playerDetection)
        {
            return false;
        }

        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        return angleToPlayer < viewingAngle / 2;
    }

    //エネミーが向かう場所の決定
    private void SetRandomPosition()
    {
        targetPosition = new Vector3(
            transform.position.x + Random.Range(-moveDistance, moveDistance),
            transform.position.y + Random.Range(-moveDistance, moveDistance),
            transform.position.z + Random.Range(-moveDistance, moveDistance)
            );

        moveInterval = Random.Range(1, 10);
    }

    //エネミーの移動
    private void MoveToTarget()
    {
        float step = enemySpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step) ;

        if (transform.position != targetPosition)
        {
            // 移動方向を計算
            Vector3 direction = (targetPosition - transform.position).normalized;
            // Quaternionを計算
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            // スムーズに回転させる
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    //コライダーにぶつかったとき
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Controller　player = other.gameObject.GetComponent<Controller>();

            if (player.fishPower < enemyPower && player != null)
            {
                animator.SetTrigger("Attack");
                audioSource.PlayOneShot(se);
                enemyPower += player.fishPower;
                UpdateUI();
            }

            else if (player.fishPower >= enemyPower && player != null)
            {
                Destroy(gameObject);
            }

            else
            {
                Debug.LogError("プレイヤーが存在していません");
            }

        }

        else if (other.gameObject.CompareTag("Field"))
        {
            SetRandomPosition();
            MoveToTarget();
        }
    }

    //まず最初にエネミーの数字の取得
    private void GetRandomPower()
    {
        enemyPower = Random.Range(minPower, maxPower + 1);
    }

    //それをenemyPowerに返す
    public int GetEnemyPower()
    {
        return enemyPower;
    }

    //それをさらにUI.Textへと表示
    private void UpdateUI()
    {
        enemyPowerText.text = enemyPower.ToString(); //intをString型へ変換
    }
}
