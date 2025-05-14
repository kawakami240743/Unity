using UnityEngine;

public class GameOverController : MonoBehaviour
{
    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver)
            return;
    }

    public void GameOver()
    {
        gameoverOptions gmover = new gameoverOptions();
        GameOverUI.Show(gmover);

        //Time.timeScale = 0f;
        isGameOver = true;
    }
}
