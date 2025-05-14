using UnityEngine;
using UnityEngine.UI;

public class PauseCanvasController : MonoBehaviour
{
    public bool isPause = false;

    void Update()
    {
        if (isPause)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            gameManager.pauseMenu = true;
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPause = true;

        PauseOptions pg= new PauseOptions();
        pg.CloseDelegete = () =>
        {
            StartGame();
        };
        PauseCanvasUI.Show(pg);
    }

    void StartGame()
    {
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        gameManager.pauseMenu = false;

        Time.timeScale = 1f;
        isPause = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}