using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //GameOver時の処理
    private GameOverController gameover;
    [SerializeField] private TextDisplay textDisplay;
    [SerializeField] private GameObject UICanvas;
    [SerializeField] public int Score;

    //Joystickの取得
    [SerializeField] private List<GameObject> canvasList;
    public JoystickController activeJoystick;

    private void Start()
    {
        gameover = GetComponent<GameOverController>();
        ActiveJoystickController();
    }

    //ゲーム内スコアの送信
    public void OnPlayerDestroyed()
    {
        Debug.Log("スコア" + Score);
        gameover.GameOver();

        DisplayScore();

        UICanvasHidden();
    }

    //ゲームオーバー画面の表示
    private void DisplayScore()
    {
        textDisplay = FindFirstObjectByType<TextDisplay>();
        if (textDisplay == null)
        {
            Debug.LogError("ゲームオーバー画面が存在していません");
        }

        else
        {
            textDisplay.GMDisplay();
        }
    }

    //ゲームオーバー時のUIの非表示
    private void UICanvasHidden()
    {
        if (UICanvas != null)
        {
            UICanvas.SetActive(false);
        }

        else
            return;
    }

    //ゲーム開始時にjoystickControllerの取得
    private void ActiveJoystickController()
    {
        foreach (GameObject canvas in canvasList)
        {
            if (canvas.activeSelf)
            {
                UICanvas = canvas;
                activeJoystick = canvas.GetComponentInChildren<JoystickController>();
                if (activeJoystick != null)
                {
                    Debug.Log("現在表示中のUI ->" + canvas.name);
                    return;
                }
            }
        }
    }
}
