using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText; // UI テキストオブジェクト
    private float timeRemaining = 5f; // 初期時間（1分）
    private bool isTimerActive = true;

    void Update()
    {
        if (isTimerActive)
        {
            timeRemaining -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);

            string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);

            timerText.text = timerString;

            if (timeRemaining <= 1f)
            {
                timerText.text = "Warning!!";
                isTimerActive = false;
            }
        }
    }
}