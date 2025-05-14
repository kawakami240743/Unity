using UnityEngine;
using UnityEngine.UI;

public class RBCanvasController : MonoBehaviour
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

        RBOptions rb= new RBOptions();
        rb.CloseDelegete = () =>
        {
            StartGame();
        };
        RBCanvasUI.Show(rb);
    }

    void StartGame()
    {
        isPause = false;
    }
}