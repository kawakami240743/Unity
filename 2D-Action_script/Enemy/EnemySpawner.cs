using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameManager gameManager;
    [SerializeField] GameObject enemyprefab_01;
    [SerializeField] GameObject enemyprefab_02;
    [SerializeField] GameObject Boss;
    [SerializeField] Transform player;
    private float timer = 5f;　//通常エネミースポーン
    private float SpawnEnemyTimer = 60f; //ボスエネミースポーン
    private bool isEnemyTimer = true;
    private bool isBossEnemy = false;
    public List<Enemy_01> enemies_01 = new List<Enemy_01>();
    public List<Enemy_02> enemies_02 = new List<Enemy_02>();

    private float destroyTimer = 60f; // 追加：エネミー削除タイマー

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        if (enemyprefab_01 == null)
        {
            Debug.LogError("エネミー01のプレハブが存在しません");
            return;
        }

        if (enemyprefab_02 == null)
        {
            Debug.LogError("エネミー02のプレハブが存在しません");
            return;
        }

        if (Boss == null)
        {
            Debug.LogError("ボスのプレハブが存在しません");
            return;
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;
        SpawnEnemyTimer -= Time.deltaTime;

        if (timer <= 1f && isEnemyTimer)
        {
            SpawnEnemy_01();
            SpawnEnemy_02();
            timer = 5f; // タイマーをリセット
        }

        if (SpawnEnemyTimer <= 0f && !isBossEnemy)
        {
            isEnemyTimer = false;
            isBossEnemy = true;
            gameManager.JoinsBoss_SE();
            SpawnBossEnemy(); // ボスをスポーンする
        }

        destroyTimer -= Time.deltaTime; // エネミー削除タイマーを減少させる

        if (destroyTimer <= 1f)
        {
            DestroyAllEnemies(); // エネミーをすべて削除する
        }


    }

    void SpawnEnemy_01()
    {
        if (isEnemyTimer)
        {
            for (int i = 0; i < 1; i++) // 例として2個のインスタンスを作成
            {
                float x = Random.Range(-130f, 0f); // X座標を-80から80の間でランダムに選ぶ
                float y = Random.Range(-30f, 0f); // Y座標を-10から30の間でランダムに選ぶ
                float z = 0f; // Z座標を0から50の間でランダムに選ぶ
                Vector3 position = new Vector3(x, y, z);

                Vector3 direction = player.position - position;
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

                GameObject spawnedEnemy_01 = Instantiate(enemyprefab_01, position, rotation); // プレハブのインスタンス化

                Enemy_01 enemy_01 = spawnedEnemy_01.GetComponent<Enemy_01>();
                if(enemy_01 != null)
                {
                    enemy_01.enemyspawner = this;
                }

                enemies_01.Add(enemy_01);
            }
        }
    }

    void SpawnEnemy_02()
    {
        if (isEnemyTimer) // isEnemyTimer == true と同じ意味
        {
            for (int i = 0; i < 1; i++) // 例として2個のインスタンスを作成
            {
                float x = Random.Range(-130f, 0f); // X座標を-80から80の間でランダムに選ぶ
                //float y = Random.Range(-30f, -30f); // Y座標を-10から30の間でランダムに選ぶ
                float z = 0f; // Z座標を0から50の間でランダムに選ぶ
                Vector3 position = new Vector3(x, -40f, z);

                Vector3 direction = player.position - position;
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

                GameObject spawnedEnemy_02 = Instantiate(enemyprefab_02, position, rotation); // プレハブのインスタンス化

                Enemy_02 enemy_02 = spawnedEnemy_02.GetComponent<Enemy_02>();
                if(enemy_02 != null)
                {
                    enemy_02.enemyspawner = this;
                }

                enemies_02.Add(enemy_02);
            }
        }
    }




    void SpawnBossEnemy()
    {
        if (isBossEnemy)
        {
            float x = 10f;
            float y = 0f;
            float z = 0f;
            Vector3 position = new Vector3(x, y, z);

            Vector3 direction = player.position - position;
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

            GameObject spawnedBoss = Instantiate(Boss, position, rotation); // ボスのインスタンス化
        }
    }

    void DestroyAllEnemies()
    {
        GameObject[] enemies_01 = GameObject.FindGameObjectsWithTag("Enemy_01");
        GameObject[] enemies_02 = GameObject.FindGameObjectsWithTag("Enemy_02");

        // リストを作ってその中に消したいキャラクターを追加していく
        List<GameObject> allEnemies = new List<GameObject>();
        allEnemies.AddRange(enemies_01);
        allEnemies.AddRange(enemies_02);

        foreach (GameObject enemy in allEnemies)
        {
            if (enemy.tag != "Boss") // タグが "Boss" でないオブジェクトのみを削除
            {
                Destroy(enemy); // エネミーを削除
            }
        }
    }

    public void RemoveEnemy_01(Enemy_01 enemy_01)
    {
        enemies_01.Remove(enemy_01);
    }

    public void RemoveEnemy_02(Enemy_02 enemy_02)
    {
        enemies_02.Remove(enemy_02);
    }
    public List<Enemy_01> GetEnemies_01()
    {
        return enemies_01;
    }

    public List<Enemy_02> GetEnemies_02()
    {
        return enemies_02;
    }
}
