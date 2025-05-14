using UnityEngine;
using UnityEngine.UI;

public class GameDrController : MonoBehaviour
{
    [SerializeField] private GameObject startButtonPrefab;
    private Button startButton;

    private bool isDr = false;

    private void Start()
    {
        if (startButtonPrefab != null)
        {
            GameObject buttonInstance = Instantiate(startButtonPrefab);
            startButton = buttonInstance.GetComponent<Button>();
        }

        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }


        PauseGame();
    }

    private void Update()
    {
        if (isDr)
            return;
    }

    void PauseGame()
    {
        DrOptions dr = new DrOptions();
        dr.CloseDelegete = () =>
        {
            StartGame();
        };
        GameDrUI.Show(dr);

        Time.timeScale = 0f;
        isDr = true;
    }

    void StartGame()
    {
        Time.timeScale = 1f;
        isDr = false;
    }
}
