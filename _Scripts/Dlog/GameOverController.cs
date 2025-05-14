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
        if (isGameOver)
            return;

        gameoverOptions gmover = new gameoverOptions();
        GameOverUI.Show(gmover);

        isGameOver = true;
    }
}
