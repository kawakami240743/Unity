using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] GameObject NormalBullet; // 発射する物体Bのプレハブ
    [SerializeField] GameObject ChargepBullet;
    [SerializeField] float projectileSpeed = 20f; // 物体Bの速度
    [SerializeField] float fireRate = 0.07f; // 発射の間隔
    private PlayerControl playerController;
    private float homingDuratin = 5f;
    //private float ChargeTimer = 2f;
    private bool isFiring = false; // 発射中かどうかのフラグ
    private bool isCharge = false;
    private bool isHoming = false;
    private Vector2 shootingDirection = Vector2.right; // デフォルトの発射方向は右
    private Vector3 shootingPositionOffset = Vector3.zero; // 射撃位置のオフセット

    private void Start()
    {
        playerController = GetComponent<PlayerControl>();
        if(playerController == null)
        {
            Debug.LogError("取得できていません");
        }
    }

    void Update()
    {
        //通常攻撃のスクリプト
        if (Input.GetKeyDown(KeyCode.F) && !isCharge)
        {
            if (!isFiring)
            {
                StartFiring();
            }
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            if (isFiring)
            {
                StopFiring();
            }
        }

        //チャージ攻撃のスクリプト

        /*if (Input.GetKeyDown(KeyCode.R) && !isCharge)
        {
                StartCharge();
                Debug.Log("チャージ開始");
        }

        if (Input.GetKeyUp(KeyCode.R) && isCharge)
        {
                ShootCharging();
                StopCharge();
                Debug.Log("チャージ停止");
        }

        if (isCharge)
        {
            ChargeTimer -= Time.deltaTime;
            if (ChargeTimer <= 0f)
            {
                Debug.Log("チャージ完了");
                playerController.playerInfo.atk = 3;
                isCharge = false;
                ChargeTimer = 2f;
            }
        }*/

        //攻撃の射出角度

        if (isFiring)
        {
            bool right = Input.GetKey(KeyCode.RightArrow);
            bool left = Input.GetKey(KeyCode.LeftArrow);
            bool up = Input.GetKey(KeyCode.UpArrow);
            bool down = Input.GetKey(KeyCode.DownArrow);

            if (right && up)
            {
                shootingDirection = new Vector2(1, 1).normalized;
                shootingPositionOffset = Vector3.zero; //右上
            }

            else if (left && up)
            {
                shootingDirection = new Vector2(-1, 1).normalized;
                shootingPositionOffset = Vector3.zero; //左上
            }

            else if (down && left)
            {
                shootingDirection = Vector2.left;
                shootingPositionOffset = new Vector3(0, -1, 0); //下（前方向）
            }

            else if (down && right)
            {
                shootingPositionOffset = new Vector3(0, -1, 0); //下（前方向）
            }

            else if (right)
            {
                shootingDirection = Vector2.right;
                shootingPositionOffset = Vector3.zero; //右
            }

            else if (left)
            {
                shootingDirection = Vector2.left;
                shootingPositionOffset = Vector3.zero; //左
            }

            else if (up)
            {
                shootingDirection = Vector2.up;
                shootingPositionOffset = Vector3.zero; //上
            }

            else if (down)
            {
                shootingPositionOffset = new Vector3(0, -1, 0); //下（前方向）
            }
        }
    }

    void StartFiring()
    {
        isFiring = true;
        StartCoroutine(FireContinuously());
    }

    void StopFiring()
    {
        isFiring = false;
    }

    /*void StartCharge()
    {
        isCharge = true;
    }

    void StopCharge()
    {
        isCharge = false;
        ChargeTimer = 2f;
    }*/

    void ShootProjectile()
    {
        Vector3 spawnPosition = transform.position + shootingPositionOffset + new Vector3(0, 3f, 0);
        GameObject projectile = Instantiate(NormalBullet, spawnPosition, Quaternion.identity); // 物体Bを生成
        projectile.GetComponent<Fire>().Initialize(shootingDirection, projectileSpeed, isHoming, homingDuratin); // 物体Bにスクリプトを追加して、発射方向を設定
    }

    public void EnableHoming(float duration)
    {
        isHoming = true;
        homingDuratin = duration;
        StartCoroutine(DisableHomingAfterTime(duration));
    }

    IEnumerator DisableHomingAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        isHoming = false;
    }

    IEnumerator FireContinuously()
    {
        while (isFiring)
        {
            ShootProjectile();
            yield return new WaitForSeconds(fireRate); // 発射までの間隔は fireRate を参考にする
        }
    }
}
