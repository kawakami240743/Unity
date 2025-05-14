using UnityEngine;
using UnityEngine.UI;

public class AudioCanvasController : MonoBehaviour
{
    private bool isPause = false;

    void Update()
    {
        if (isPause)
            return;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPause = true;

        AudioOptions ad= new AudioOptions();
        ad.CloseDelegete = () =>
        {
            StartGame();
        };
        AudioCanvasUI.Show(ad);
    }

    void StartGame()
    {
        isPause = false;
    }
}