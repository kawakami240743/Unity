using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Controller : MonoBehaviour
{
    //Playerの移動に関する設定
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float upMove = 2f;
    [SerializeField] private float downMove = 2f;
 
    //UIに関する設定
    [SerializeField] private JoystickController joystick;
    [SerializeField] private Button up;
    [SerializeField] private Button down;
    [SerializeField] private Button dash;
    [SerializeField] public int fishPower = 2;
    [SerializeField] private TMP_Text fishPowerText;

    //ゲーム全体の管理に関する設定
    private GameManager gameManager;
    [SerializeField] private Camera mainCamera;
    private Rigidbody rb;
    private Animator animator;
    bool playerDestroy = false;

    private Vector3 lastPosition;
    private AudioSource[] audioSource;
    [SerializeField] public AudioClip[] SE = new AudioClip[2];
    [SerializeField] private bool isPlaying;

    private void Start()
    {
        //コンポーネントの取得
        gameManager = FindFirstObjectByType<GameManager>();
        audioSource = GetComponents<AudioSource>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
        joystick = gameManager.activeJoystick;

        //クリック時のイベント追加
        up.onClick.AddListener(OnUpButton);
        down.onClick.AddListener(OnDownButton);
        dash.onClick.AddListener(OnDashButton);

        //最初の行動
        UpdateUI();
    }

    private void FixedUpdate()
    {
        Vector3 forward = mainCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = mainCamera.transform.right;
        right.y = 0;
        right.Normalize();

        Vector3 move = forward * joystick.Vertical() + right * joystick.Horizontal();
        Vector3 targetPosition = rb.position + move * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(targetPosition);

        if(move != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(move);
            rb.rotation = Quaternion.Slerp(rb.rotation, rotation , Time.fixedDeltaTime * rotationSpeed);
        }

        PlaySE();

    }

    private void PlaySE()
    {
        if (lastPosition != rb.position)
        {
            if (!isPlaying)
            {
                audioSource[0].clip = SE[0];
                audioSource[0].loop = true;
                audioSource[0].Play();
                isPlaying = true;
            }

            lastPosition = transform.position;
        }

        else if (lastPosition == rb.position && isPlaying)
        {
            audioSource[0].Stop();
            isPlaying = false;
            audioSource[0].loop = false;
        }
    }

    public void OnUpButton()
    {

            Vector3 forward = mainCamera.transform.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = mainCamera.transform.right;
            right.y = 0;
            right.Normalize();

            Vector3 move = forward * joystick.Vertical() + right * joystick.Horizontal();

            Vector3 forwardDirection = transform.forward;

            Vector3 upmove = forwardDirection + Vector3.up * upMove;

            Vector3 playerMove = move + upmove;

            rb.MovePosition(rb.position + upmove * moveSpeed/2 * Time.fixedDeltaTime);
            Debug.Log("上昇中");

            if (playerMove != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(playerMove);
                rb.rotation = Quaternion.Slerp(rb.rotation, rotation, Time.fixedDeltaTime * rotationSpeed);
            }
    }

    public void OnDownButton()
    {
            Vector3 forward = mainCamera.transform.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = mainCamera.transform.right;
            right.y = 0;
            right.Normalize();

            Vector3 move = forward * joystick.Vertical() + right * joystick.Horizontal();

            Vector3 forwardDirection = transform.forward;

            Vector3 downmove = forwardDirection + Vector3.down * downMove;

            Vector3 playerMove = move + downmove;

            rb.MovePosition(rb.position + downmove * moveSpeed * Time.fixedDeltaTime);
            Debug.Log("下降中");

            if (playerMove != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(playerMove);
                rb.rotation = Quaternion.Slerp(rb.rotation, rotation, Time.fixedDeltaTime * rotationSpeed);
            }
    }

    public void OnDashButton()
    {
        Vector3 forward = mainCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = mainCamera.transform.right;
        right.y = 0;
        right.Normalize();

        Vector3 dashmove = forward * joystick.Vertical() + right * joystick.Horizontal();
        rb.MovePosition(rb.position + dashmove * dashSpeed * Time.fixedDeltaTime);
        //Debug.Log("加速中");

        if (dashmove != Vector3.zero)
        {
            animator.SetFloat("Speed", 1);
            Quaternion rotation = Quaternion.LookRotation(dashmove);
            rb.rotation = Quaternion.Slerp(rb.rotation, rotation, Time.fixedDeltaTime * rotationSpeed);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            audioSource[1].PlayOneShot(SE[1]);
            EnemyMove enemy = other.gameObject.GetComponent<EnemyMove>();

            if (enemy.enemyPower <= fishPower && enemy != null)
            {
                animator.SetTrigger("Attack");
                fishPower += enemy.enemyPower;
                UpdateUI();
            }

            else if (enemy.enemyPower > fishPower)
            {
                playerDestroy = true;
                Destroy(gameObject);
            }

            else
            {
                Debug.LogError("エネミーが見つかりません");
            }

            ChangeScale();
        }
    }

    private void ChangeScale()
    {
        float scaleUp = 0.5f + (fishPower -1) * 0.00003f;
        transform.localScale = new Vector3(scaleUp, scaleUp, scaleUp);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();

            fishPower *= item.itemPower;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        fishPowerText.text = fishPower.ToString();
    }

    private void OnDestroy()
    {
        if (gameManager != null && playerDestroy)
        {
            gameManager.Score = fishPower;
            gameManager.OnPlayerDestroyed();
        }
    }
}