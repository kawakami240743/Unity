using UnityEngine;


public class EnemySpawner : MonoBehaviour
{

    [SerializeField] GameObject prefab;

    private float timer = 0f;
    

    void Start()
    {

        // xとzの範囲でランダムな位置にプレハブをインスタンス化
        for (int i = 0; i < 5; i++) // 例として100個のインスタンスを作成
        {
            float x = Random.Range(0f, 50f); // X座標を0から1000の間でランダムに選ぶ
            float y = Random.Range(0f, 30f); // Z座標を0から1000の間でランダムに選ぶ
            Vector3 position = new Vector3(x, y, -7f); // Y座標は常に50

            Instantiate(prefab, position, Quaternion.identity); // プレハブのインスタンス化


        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 5f)
        {
            // xとzの範囲でランダムな位置にプレハブをインスタンス化
            for (int i = 0; i < 1; i++) // 例として100個のインスタンスを作成
            {
                float x = Random.Range(0f, 80f); // X座標を0から1000の間でランダムに選ぶ
                float y = Random.Range(0f, 30f); // Z座標を0から1000の間でランダムに選ぶ
                Vector3 position = new Vector3(x, y , -7f); // Y座標は常に50

                Instantiate(prefab, position, Quaternion.identity); // プレハブのインスタンス化

            }
            timer = 0f;
        }
    }
}