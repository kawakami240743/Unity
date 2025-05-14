using UnityEngine;

public class GameStartController : MonoBehaviour
{
    private bool isStart = false;
    void Start()
    {
        PauseGame();
    }

    void Update()
    {
        if (isStart)
            return;
    }

    void PauseGame()
    {
        startOptions st = new startOptions();
        st.CloseDelegete = () =>
        {
            StartGame();
        };
        GameStartUI.Show(st);

        Time.timeScale = 0f;
        isStart = true;
    }

    void StartGame()
    {
        Time.timeScale = 1f;
        isStart = false;
    }
}
