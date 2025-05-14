using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
    public Text timerText; // UI テキストオブジェクト
    private float timeRemaining = 60f; // 初期時間（1分）
    private bool isTimerActive = true;
    private bool isWarningActive = false;

    void Update()
    {
        if (isTimerActive && !isWarningActive)
        {
            timeRemaining -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);

            string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);

            timerText.text = timerString;

            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                isTimerActive = false;
                StartCoroutine(ShowWarning());
            }
        }
    }

    IEnumerator ShowWarning()
    {
        isWarningActive = true;
        timerText.text = "Warning!!";
        yield return new WaitForSeconds(3f); // 3秒間待つ
        timerText.enabled = false;
        isTimerActive = false;
    }
}
