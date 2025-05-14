using UnityEngine;

public class GameClearController : MonoBehaviour
{
    private bool isGameClear = false;

    void Update()
    {
        if (isGameClear)
            return;
    }

    public void GameClear()
    {
        gameclearOptions gmclear = new gameclearOptions();
        GameClearUI.Show(gmclear);

        Time.timeScale = 0f;
        isGameClear = true;
    }
}
