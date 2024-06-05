using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f; // エネミーの移動速度
    private Transform player; // プレイヤーの位置

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // プレイヤーの位置を取得
    }

    void Update()
    {
        if (player != null)
        {
            // プレイヤーの方向を向く
            Vector2 direction = (player.position - transform.position).normalized;
            transform.up = direction;

            // プレイヤーに向かって移動
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }
}
