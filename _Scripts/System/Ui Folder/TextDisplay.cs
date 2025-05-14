using UnityEngine;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private Text theTitleText;
    [SerializeField] private Text scoreText;

    public void GMDisplay()
    {
        Debug.Log("呼び出されたよ");

        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("ゲームマネージャーが存在していません");
        }

        int score = gameManager.Score;
        string rank;
        switch (score)
        {
            case int n when (n >= 50000):
                rank = "Fish Predator";
                break;

            case int n when (n >= 25000):
                rank = "Cheeky Chomper";
                break;

            case int n when (n >= 5000):
                rank = "Feisty Fin";
                break;

            case int n when (n >= 100):
                rank = "Tiny Tasty Fish";
                break;

            default:
                rank = "Bite-Size Baby"; // 条件に満たない場合
                break;
        }

        scoreText.text = "Score :" + score.ToString();
        theTitleText.text = "Rank : " + rank;
    }
}