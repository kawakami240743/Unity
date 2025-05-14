using UnityEngine;

public class gameclearOptions
{
    public System.Action CloseDelegete;
}

public class GameClearUI : ViewController
{
    public static string prefabName = "GameClear_Canvas";
    public static GameObject prefab;
    private gameclearOptions gameclear;

    public static GameClearUI Show(gameclearOptions gmclear)
    {
        if(prefab == null)
        {
            prefab = Resources.Load(prefabName) as GameObject;
        }
        GameObject obj = Instantiate(prefab);
        GameClearUI gameClearUI = obj.GetComponent<GameClearUI>();

        gameClearUI.UpdateContent(gmclear);
        return gameClearUI;
    }

    void UpdateContent(gameclearOptions gmclear)
    {
        Cursor.lockState = CursorLockMode.None;
        gameclear = gmclear;
    }
}
