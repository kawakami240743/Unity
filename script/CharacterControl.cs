using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        // Rigidbody2D コンポーネントを取得
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 水平方向の入力を取得
        float moveHorizontal = Input.GetAxis("Horizontal");

        // 水平方向の移動ベクトルを作成
        Vector2 movement = new Vector2(moveHorizontal, 0.0f);

        // 移動を適用
        rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);

        // ジャンプ入力をチェック
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // ジャンプを実行
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面に接触しているかどうかをチェック
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // 地面から離れたかどうかをチェック
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }
}
