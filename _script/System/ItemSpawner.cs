using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject ItemPrefab_01;
    [SerializeField] GameObject ItemPrefab_02;
    private float Itemtimer = 10f;
    void Start()
    {
        if (ItemPrefab_01 == null)
        {
            Debug.LogError("Item1が存在していません");
        }

        if (ItemPrefab_02 == null)
        {
            Debug.LogError("Item2が存在していません");
        }
    }

    void Update()
    {
        Itemtimer -= Time.deltaTime;

        if (Itemtimer <= 1f)
        {
            ItemSpawn();
            Itemtimer = 10f;
        }
    }

    void ItemSpawn()
    {
        for (int i = 0; i < 1; i++)
        {
            float x = Random.Range(-115f, -95f); // X座標を-80から80の間でランダムに選ぶ
            Vector3 position = new Vector3(x, -50f, 0f);

            GameObject itemToSpawn = Random.Range(0, 2) == 0 ? ItemPrefab_01 : ItemPrefab_02;
            Instantiate(itemToSpawn, position, Quaternion.identity);
        }
    }
}