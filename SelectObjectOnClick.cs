using UnityEngine;

public class SelectObjectOnClick : MonoBehaviour
{
    [SerializeField] GameObject Stone;
    [SerializeField] AudioClip[] Se;
    AudioSource Asrc;

    int TurnNumber = 0;

    int[][] GameMatrix;

    private void Start()
    {
        Asrc = GetComponent<AudioSource>();
        GameMatrix = new int[8][];

        for (int i = 0; i < 8; i++)
        {
            GameMatrix[i] = new int[8];
            for (int j = 0; j < 8; j++)
            {
                GameMatrix[i][j] = -1;
            }
        }

        // ゲームの初期配置
        GameMatrix[3][3] = 0;
        GameMatrix[4][4] = 0;
        GameMatrix[3][4] = 1;
        GameMatrix[4][3] = 1;

        // 石の初期配置
        InstantiateStone(-3, 3, 0);
        InstantiateStone(-4, 4, 0);
        InstantiateStone(-3, 4, 1);
        InstantiateStone(-4, 3, 1);

    }

    private void InstantiateStone(int v, int h, int player)
    {
        float zz = player == 0 ? 180f : 0;
        GameObject newStone = Instantiate(Stone, new Vector3(v, -0.1f, h), Quaternion.Euler(0, 0, zz));
        newStone.transform.localScale *= 0.8f;
    }

    private int getValue(int v, int h)
    {
        if (v < 0 || v >= 8 || h < 0 || h >= 8)
            return -1;
        return GameMatrix[v][h];
    }

    private void setValue(int v, int h, int value)
    {
        if (v >= 0 && v < 8 && h >= 0 && h < 8)
        GameMatrix[v][h] = value;
    }

    private bool CheckDirection(int v, int h, int dv, int dh, int player)
    {
        int opponent = 1 - player;
        int nv = v + dv;
        int nh = h + dh;

        bool hasOpponentStone = false;
        int flippedCount = 0;

        while (nv >= 0 && nv < 8 && nh >= 0 && nh < 8)
        {
            if (getValue(nv, nh) == opponent)
            {
                hasOpponentStone = true;
                flippedCount++;
            }
            else if (getValue(nv, nh) == player)
            {
                break;
            }
            else
            {
                break;
            }
            nv += dv;
            nh += dh;
        }
        return hasOpponentStone && flippedCount > 0;
    }

    private void FlipDirection(int v, int h, int dv, int dh, int player)
    {
        int opponent = 1 - player;
        int nv = v + dv;
        int nh = h + dh;

        while (nv >= 0 && nv < 8 && nh >= 0 && nh < 8)
        {
            if (getValue(nv, nh) == opponent)
            {
                setValue(nv, nh, player);
                InstantiateStone(nv, nh, player);
                // 石のビジュアルを更新する必要がある場合はここで行う
                // 例: InstantiateStone(nv, nh, player);
            }
            else if (getValue(nv, nh) == player)
            {
                break;
            }
            nv += dv;
            nh += dh;
        }
    }

    void FlipStones(int v, int h, int player)
    {
        int[] directions = { -1, 0, 1 };
        foreach (int dv in directions)
        {
            foreach (int dh in directions)
            {
                if (dv == 0 && dh == 0) continue;
                if (CheckDirection(v, h, dv, dh, player))
                {
                    FlipDirection(v, h, dv, dh, player);
                }
            }
        }
    }

    void Update()
    {
        // マウスがクリックされたとき
        if (Input.GetMouseButtonDown(0))
        {
            // マウス位置から Ray を生成
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Ray が何かと衝突したかを確認
            if (Physics.Raycast(ray, out hit))
            {
                // 衝突したオブジェクトの情報を取得
                GameObject selectedObject = hit.transform.gameObject;
                MyBlocks mb = selectedObject.GetComponent<MyBlocks>();
                if (mb)
                {
                    Debug.Log("---> V:" + mb.Vertical + "H:" + mb.Horizon);

                    if (getValue(mb.Vertical, mb.Horizon) == -1)
                    {
                        //だったら石を置ける
                        Asrc.clip = Se[0];

                        TurnNumber++;
                        float zz = TurnNumber % 2 == 0 ? 180f : 0;
                        GameObject lastStone = Instantiate(Stone, selectedObject.
                        transform.position, selectedObject.
                        transform.rotation * Quaternion.Euler(0, 0, zz));

                        lastStone.transform.localScale *= 0.8f;

                        setValue(mb.Vertical, mb.Horizon, TurnNumber % 2);

                        FlipStones(mb.Vertical, mb.Horizon, TurnNumber % 2);

                        Debug.Log("Flipped Count" + GetFlippedCount(TurnNumber % 2));

                    }
                    else
                    {
                        Asrc.clip = Se[1];
                    }
                    Asrc.Play();
                    DebugMatrix();
                }
            }
        }
    }

    private int GetFlippedCount(int player)
    {
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (GameMatrix[i][j] == player)
                {
                    count++;
                }
            }
        }
        return count;
    }

    void DebugMatrix()
    {
        foreach (int[] m in GameMatrix)
        {
            string outStr = "";
            foreach (int m2 in m)
                outStr += "" + m2;

            Debug.Log("--->" + outStr);
        }
    }
}

