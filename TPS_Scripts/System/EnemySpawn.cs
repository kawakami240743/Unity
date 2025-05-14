using UnityEngine;
using System.Collections.Generic;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject prefab; // スポーンするエネミーのプレハブ
    [SerializeField] private int maxEnemies = 10; // 最大エネミー数
    [SerializeField] private float spawnInterval = 30f; // スポーン間隔
    [SerializeField] private float spawnRadius = 5f; // スポーン範囲

    private List<GameObject> activeEnemies = new List<GameObject>(); // 現在存在するエネミー
    private float spawnTimer;

    void Start()
    {
        spawnTimer = spawnInterval; // 初回のスポーン時間を設定
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval; // 次のスポーン時間をリセット
        }

        // リストを更新して、倒されたエネミーを削除
        activeEnemies.RemoveAll(enemy => enemy == null);
    }

    void SpawnEnemy()
    {
        if (activeEnemies.Count >= maxEnemies) return; // 上限に達している場合はスポーンしない

        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        spawnPosition.y = transform.position.y; // Y座標を固定

        GameObject enemy = Instantiate(prefab, spawnPosition, Quaternion.identity);
        activeEnemies.Add(enemy); // エネミーをリストに追加
    }
}
