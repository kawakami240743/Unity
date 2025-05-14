using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab; // スポーンするプレハブ
    [SerializeField] private Vector3 offset = Vector3.up; // スポーン位置のオフセット

    void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        if (prefab != null)
        {
            Instantiate(prefab, transform.position + offset, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Prefabが設定されていません！");
        }
    }
}
