using UnityEngine;
using UnityEngine.UI;

public class GamePauseController : MonoBehaviour
{
    [SerializeField] public Button Ogata_pauseButton;
    [SerializeField] public Button Ono_pauseButton;

    private bool isPause = false;

    void Start()
    {
        Ogata_pauseButton.onClick.AddListener(PauseGame);
        Ono_pauseButton.onClick.AddListener(PauseGame);
    }

    void Update()
    {
        if (isPause)
            return;
    }

    void PauseGame()
    {
        startOptions st = new startOptions();
        st.CloseDelegete = () =>
        {
            StartGame();
        };
        GamePauseUI.Show(st);

        Time.timeScale = 0f;
        isPause = true;
    }

    void StartGame()
    {
        Time.timeScale = 1f;
        isPause = false;
    }
}
