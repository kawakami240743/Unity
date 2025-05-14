using UnityEngine;

public class gameoverOptions
{
    public System.Action CloseDelegete;
}

public class GameOverUI : ViewController
{
    public static string prefabName = "Ogata_GameOver_Canvas";
    public static GameObject prefab;
    private gameoverOptions gameover;

    public static GameOverUI Show(gameoverOptions gmover)
    {
        if (prefab == null)
        {
            prefab = Resources.Load(prefabName) as GameObject;
        }
        GameObject obj = Instantiate(prefab);
        GameOverUI gameOverUI = obj.GetComponent<GameOverUI>();

        gameOverUI.UpdateContent(gmover);
        return gameOverUI;
    }

    void UpdateContent(gameoverOptions gmover)
    {
        Cursor.lockState = CursorLockMode.None;
        gameover = gmover;
    }
}
