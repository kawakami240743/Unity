using UnityEngine;

public class FishSpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] fishPrefab;
    [SerializeField] Vector3[] spawnHeights;
    [SerializeField] float spawnInterval = 2f;
    [SerializeField] int spawn = 10;
    [SerializeField] int maxCount = 200;

    [SerializeField] GameObject firstSpawnFish;
    [SerializeField] int firstSpawnCount = 30;
    [SerializeField] private int fishCount = 0;

    private void Start()
    {
        for (int i = 0; i < firstSpawnCount; i++)
        {
            firstSpawn();
        }

        for (int i = 0; i < spawn; i++)
        {
            SpawnFish();
        }
        // スポーンを開始
        InvokeRepeating("SpawnFish", 0f, spawnInterval);
    }

    private void firstSpawn()
    {
        Vector3 spawnPosition = new Vector3
        (
            Random.Range(-200f, 200f),
            spawnHeights[0].y,
            Random.Range(-200, 200)
        );

        Instantiate(firstSpawnFish, spawnPosition, Quaternion.identity);

        fishCount++;
    }

    private void SpawnFish()
    {
        if (fishCount < maxCount)
        {
            // ランダムにプレハブを選択
            int randomIndex = Random.Range(0, fishPrefab.Length);
            GameObject fishToSpawn = fishPrefab[randomIndex];

            Vector3 spawnHeight = spawnHeights[randomIndex];

            Vector3 spawnPosition = new Vector3
            (
                Random.Range(-200f, 200f),
                spawnHeight.y,
                Random.Range(-200, 200)
             );

            // プレハブをスポーン
            Instantiate(fishToSpawn, spawnPosition, Quaternion.identity);
            fishCount++;
        }
    }
}
