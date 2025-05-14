using UnityEngine;
using System.Collections;
using Game;
using MyGame.Grenades;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private DFManager dfManager;
    private Vector3 targetPosition;
    private bool isJump, isInvisible;
    private float invisibleTimer = 1.5f;

    [SerializeField] private Transform cameraRotate;
    [SerializeField] public PlayerStatus playerStatus;
    [SerializeField] private PlayerHPSlider hpSlider;
    [SerializeField] private float moveSpeed = 3f, backSpeed = 2.5f, dashSpeed = 7f, backDashSpeed = 6.5f, jumpPower = 5f, rotationSpeed = 5f;

    [Header("Animator Speeds")]
    [SerializeField] private float animatorWalkSpeed = 0.5f, animatorDashSpeed = 1f;
    [SerializeField] private float animatorBackSpeed = -0.5f, animatorDashBackSpeed = -1f;
    [SerializeField] private float animatorLeftSpeed = -0.5f, animatorLeftDashSpeed = -1f;
    [SerializeField] private float animatorRightSpeed = 0.5f, animatorRightDashSpeed = 1f;

    [Header("武器のポジション設定")]
    [SerializeField] private Transform normalAssultPosition;
    [SerializeField] private Transform normalHandgunPosition;
    [SerializeField] private Transform normalShotgunPosition;
    [SerializeField] private Transform normalSubmachinegunPosition;

    [Header("射撃時のポジション設定")]
    [SerializeField] private Transform fireAssultPosition;
    [SerializeField] private Transform fireHandgunPosition;
    [SerializeField] private Transform fireShotgunPosition;
    [SerializeField] private Transform fireSubmachinegunPosition;

    [Header("武器の状態管理")]
    private bool isAssult;
    private bool isSubmachine;
    private bool isShot;
    private bool isHand = true; // 初期武器はハンドガン

    [Header("グレネード設定")]
    private GSelector grenadeSelector; // 🔹 **グレネード選択管理**
    [SerializeField] private Transform handTransform;  // 🔹 **投げる位置**
    [SerializeField] private float throwForce = 10f;   // 🔹 **投げる力**

    public int Gire = 0;
    private bool isDeath;
    private int currentDifficulty;
    private float takeDamage = 1;
    [SerializeField] private Transform playerTransform;

    private void Awake()
    {
        playerStatus = new PlayerStatus(300, 300, 10);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        dfManager = FindFirstObjectByType<DFManager>();
        grenadeSelector = FindFirstObjectByType<GSelector>(); // 🔹 **GSelector からデータ取得**

        if (!cameraRotate)
            Debug.LogError("Camera Rotate Transform is not assigned.");

        if (dfManager != null)
        {
            currentDifficulty = dfManager.GetCurrentDifficulty();

            switch (currentDifficulty)
            {
                case 1: // Easy
                    takeDamage *= 0.8f;
                    break;
                case 2: // Normal
                        // 変更なし
                    break;
                case 3: // Hard
                    takeDamage *= 3f;
                    break;
            }
        }
    }

    private void Update()
    {
       if (!isDeath)
        {

            if (isInvisible && (invisibleTimer -= Time.deltaTime) <= 0)
            {
                isInvisible = false;
                invisibleTimer = 1.5f;
            }

            UpdateAnimator();
        }

        else
        {
            fireAssultPosition.gameObject.SetActive(false);
            fireHandgunPosition.gameObject.SetActive(false);
            fireShotgunPosition.gameObject.SetActive(false);
            fireSubmachinegunPosition.gameObject.SetActive(false);
        }

       if (playerTransform.position.y <= -3)
        {
            Debug.Log("落ちたよ");
            playerTransform.position = new Vector3(361.6f, 3.33f, 415.5f);
            rb.linearVelocity = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        if (!isDeath) 
        {
            Vector3 moveDirection = CalculateMovement();
            PlayerRotate();

            float speed = GetMovementSpeed(moveDirection);

            int wallLayer = LayerMask.GetMask("Wall");

            Ray ray = new Ray(transform.position, moveDirection);
            Debug.DrawRay(ray.origin, ray.direction * 1.5f, Color.red);

            if (Physics.Raycast(ray, 1.5f, wallLayer))
            {
                Debug.Log("⚠️ Raycast ヒット！ 壁がある！");
                speed = moveSpeed;
            }

            targetPosition = rb.position + moveDirection * speed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);
        } 
    }

    private Vector3 CalculateMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = Vector3.Scale(cameraRotate.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 right = Vector3.Scale(cameraRotate.right, new Vector3(1, 0, 1)).normalized;

        Vector3 moveDirection = forward * vertical + right * horizontal;
        return moveDirection.magnitude > 1 ? moveDirection.normalized : moveDirection;
    }

    private float GetMovementSpeed(Vector3 moveDirection)
    {
        // 射撃中はダッシュ不可
        if (Input.GetMouseButton(0))
        {
            return moveSpeed; // 通常速度に固定
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            return Input.GetKey(KeyCode.S) ? backDashSpeed : dashSpeed;
        }

        return Input.GetKey(KeyCode.S) ? backSpeed : moveSpeed;
    }

    private void UpdateAnimator()
    {
        // 左クリックで武器構えと射撃アニメーション
        if (Input.GetMouseButton(0))
        {
            // 通常ポジションを無効化
            normalAssultPosition.gameObject.SetActive(false);
            normalHandgunPosition.gameObject.SetActive(false);
            normalShotgunPosition.gameObject.SetActive(false);
            normalSubmachinegunPosition.gameObject.SetActive(false);

            if (isAssult || isSubmachine)
            {
                animator.SetBool("isAssult", true);
                fireAssultPosition.gameObject.SetActive(true);
                animator.SetFloat("HorFiSpeed", Input.GetAxis("Horizontal")); // 水平方向（A/Dキーなど）
                animator.SetFloat("VerFiSpeed", Input.GetAxis("Vertical"));   // 垂直方向（W/Sキーなど）
            }
            else if (isHand)
            {
                animator.SetBool("isHand", true);
                fireHandgunPosition.gameObject.SetActive(true);
                animator.SetFloat("HorFiSpeed", Input.GetAxis("Horizontal")); // 水平方向（A/Dキーなど）
                animator.SetFloat("VerFiSpeed", Input.GetAxis("Vertical"));   // 垂直方向（W/Sキーなど）
            }
            else if (isShot)
            {
                animator.SetBool("isShot", true);
                fireShotgunPosition.gameObject.SetActive(true);
                animator.SetFloat("HorFiSpeed", Input.GetAxis("Horizontal")); // 水平方向（A/Dキーなど）
                animator.SetFloat("VerFiSpeed", Input.GetAxis("Vertical"));   // 垂直方向（W/Sキーなど）
            }
        }
        else
        {
            // 左クリックを離したらすべてfalseに
            animator.SetBool("isAssult", false);
            animator.SetBool("isHand", false);
            animator.SetBool("isShot", false);

            // 射撃ポジションをオフ
            fireAssultPosition.gameObject.SetActive(false);
            fireHandgunPosition.gameObject.SetActive(false);
            fireShotgunPosition.gameObject.SetActive(false);
            fireSubmachinegunPosition.gameObject.SetActive(false);

            // 通常ポジションを元に戻す
            normalAssultPosition.gameObject.SetActive(isAssult);
            normalHandgunPosition.gameObject.SetActive(isHand);
            normalShotgunPosition.gameObject.SetActive(isShot);
            normalSubmachinegunPosition.gameObject.SetActive(isSubmachine);
        }

        // 移動アニメーションの設定
        SetAnimatorSpeed("VerticalSpeed", Input.GetKey(KeyCode.W), Input.GetKey(KeyCode.S), animatorWalkSpeed, animatorBackSpeed, animatorDashSpeed, animatorDashBackSpeed);
        SetAnimatorSpeed("HorizontalSpeed", Input.GetKey(KeyCode.D), Input.GetKey(KeyCode.A), animatorRightSpeed, animatorLeftSpeed, animatorRightDashSpeed, animatorLeftDashSpeed);

        // ジャンプアニメーションの設定
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            // クリックしていない場合のみジャンプアニメーションを適用
            if (!Input.GetKey(KeyCode.Mouse0))
            {
                animator.SetTrigger("isJump");
            }

            else return;

            isJump = true;  // ジャンプフラグを設定
            StartCoroutine(PlayerJumping());
        }

    }

    private void SetAnimatorSpeed(string param, bool positiveKey, bool negativeKey, float positiveSpeed, float negativeSpeed, float positiveDashSpeed, float negativeDashSpeed)
    {
        if (positiveKey && Input.GetKey(KeyCode.LeftShift))
            animator.SetFloat(param, positiveDashSpeed, 0.1f, Time.deltaTime);
        else if (positiveKey)
            animator.SetFloat(param, positiveSpeed, 0.1f, Time.deltaTime);
        else if (negativeKey && Input.GetKey(KeyCode.LeftShift))
            animator.SetFloat(param, negativeDashSpeed, 0.1f, Time.deltaTime);
        else if (negativeKey)
            animator.SetFloat(param, negativeSpeed, 0.1f, Time.deltaTime);
        else
            animator.SetFloat(param, 0, 0.5f, Time.deltaTime);
    }

    private IEnumerator PlayerJumping()
    {
        yield return new WaitForSeconds(0.4f);
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpPower, rb.linearVelocity.z);
        isJump = true;
        yield return new WaitForSeconds(1.5f);
        isJump = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Assult"))
        {
            SwitchWeapon(true, false, false, false);
        }
        else if (collision.gameObject.CompareTag("Submachine"))
        {
            SwitchWeapon(false, true, false, false);
        }
        else if (collision.gameObject.CompareTag("Shot"))
        {
            SwitchWeapon(false, false, true, false);
        }
        else if (collision.gameObject.CompareTag("Hand"))
        {
            SwitchWeapon(false, false, false, true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ぶつかったよ");
        if (other.gameObject.CompareTag("AttackField"))
        {
            Debug.Log("攻撃を受けたよ");
            playerStatus.hp -= takeDamage;
            hpSlider.Damage();

            if (playerStatus.hp <= 0)
            {
                StartCoroutine(GameOver());
            }
        }

        if (other.gameObject.CompareTag("Clearzone"))
        {
            GameManager gamemanager = FindFirstObjectByType<GameManager>();
            gamemanager.GameClear();
        }
    }

    private void SwitchWeapon(bool assult, bool submachine, bool shot, bool hand)
    {
        isAssult = assult;
        isSubmachine = submachine;
        isShot = shot;
        isHand = hand;

        normalAssultPosition.gameObject.SetActive(assult);
        normalSubmachinegunPosition.gameObject.SetActive(submachine);
        normalShotgunPosition.gameObject.SetActive(shot);
        normalHandgunPosition.gameObject.SetActive(hand);

        // アニメーションのリセット
        animator.SetBool("isAssult", false);
        animator.SetBool("isHand", false);
        animator.SetBool("isShot", false);

        SwitchWeapon uiManager = FindFirstObjectByType<SwitchWeapon>();

        if (uiManager != null)
        {
            string weaponName = "";
            if (assult) weaponName = "Assult";
            else if (submachine) weaponName = "Submachine";
            else if (shot) weaponName = "Shotgun";
            else if (hand) weaponName = "Handgun";

            uiManager.UpdateWeaponSprite(assult, submachine, shot, hand);
        }
        else
        {
            Debug.LogError("SwitchWepon スクリプトがシーンに見つかりません！");
        }
    }

    private void PlayerRotate()
    {
        Vector3 cameraForward = Vector3.Scale(cameraRotate.forward, new Vector3(1, 0, 1)).normalized;

        if (cameraForward.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void Heal(int healAmount)
    {
        if (!isDeath) 
        {
            playerStatus.hp = Mathf.Min(playerStatus.hp + healAmount, playerStatus.maxhp);
            hpSlider.Damage(); // UI更新
            Debug.Log($"Player healed by {healAmount}. Current HP: {playerStatus.hp}");
        }
    }

    public bool IsHPFull()
    {
        return playerStatus.hp >= playerStatus.maxhp;
    }

    public void AddGire()
    {
        Gire++;
        GetGire get = FindFirstObjectByType<GetGire>();
        get.DIsplayGire();
    }

    private IEnumerator GameOver()
    {
        isDeath = true;
        animator.SetTrigger("isDeath");
        yield return new WaitForSeconds(3f);
        GameManager gamemanager = FindFirstObjectByType<GameManager>();
        gamemanager.GameOver();
        Destroy(gameObject);
    }

    public bool ThrowGrenade(Vector3 throwPosition, Vector3 throwDirection, string grenadeName)
    {
        if (!isDeath)
        {
            throwPosition = transform.position + transform.forward * 1.2f + Vector3.up * 1.5f;
            throwDirection = transform.forward + Vector3.up * 0.6f;

            if (grenadeSelector == null)
            {
                Debug.LogError("🚨 `ThrowGrenade()` - `grenadeSelector` が `null` です！");
                return false;
            }

            GameObject grenadePrefab = grenadeSelector.GetSelectedGrenadePrefab();
            if (grenadePrefab == null)
            {
                Debug.LogError($"🚨 `ThrowGrenade()` - `{grenadeName}` のプレハブが `null` です！");
                return false;
            }

            animator.SetTrigger("Throwing");

            // 🟢 `Coroutine` で 1 秒後に `Throw()` を呼ぶ（引数を保持できる）
            StartCoroutine(DelayedThrow(throwPosition, throwDirection, grenadePrefab));

            return true;
        }

        // 🔹 `isDeath == true` の場合は `false` を返すようにする
        return false;
    }


    // 🟢 `Coroutine` を使って 1 秒待つ
    private IEnumerator DelayedThrow(Vector3 throwPosition, Vector3 throwDirection, GameObject grenadePrefab)
    {
        yield return new WaitForSeconds(0.8f);

        if (grenadePrefab == null)
        {
            Debug.LogError("🚨 `DelayedThrow()` - `grenadePrefab` が `null` です！（Throw を呼ぶ直前）");
            yield break;
        }

        Throw(throwPosition, throwDirection, grenadePrefab);
    }

    // 🟢 実際にグレネードを投げる処理
    private void Throw(Vector3 throwPosition, Vector3 throwDirection, GameObject grenadePrefab)
    {
        GameObject grenade = Instantiate(grenadePrefab, throwPosition, Quaternion.identity);

        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError($"🚨 `{grenadePrefab.name}` に `Rigidbody` がアタッチされていません！");
        }

        Debug.Log($"💥 `{grenadePrefab.name}` を投擲！");
    }
}
