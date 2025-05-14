using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject Itemprefab;
    [SerializeField] private int firstSpawn = 5;
    [SerializeField] private int maxCount = 10;
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private int ItemCount;

    void Start()
    {
        for (int i = 0; i < firstSpawn; i++)
        {
            FirstSpawnItems();
        }

        InvokeRepeating("SpawnItems", 0f, spawnInterval);
    }

    private void FirstSpawnItems()
    {
        if (ItemCount < maxCount)
        {
            Vector3 spawnPosition = new Vector3
            (
                Random.Range(-200, 200),
                Random.Range(-300, 80),
                Random.Range(-200, 200)
            );

            Instantiate(Itemprefab, spawnPosition, Quaternion.identity);
        }

        ItemCount++;
    }

    private void SpawnItems()
    {
        if (ItemCount < maxCount)
        {
            Vector3 spawnPosition = new Vector3
            (
                Random.Range(-200, 200),
                Random.Range(-300, 80),
                Random.Range(-200, 200)
            );

            Instantiate(Itemprefab, spawnPosition, Quaternion.identity);
        }

        ItemCount++;
    }
}
