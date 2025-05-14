using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GetGire : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI GireCountText; // UIのTextMeshPro
    [SerializeField] private PlayerController player;
    [SerializeField] private Text FinishText;
    private int wantGire;

    private void Start()
    {
        wantGire = player.Gire;

        GireCountText.text = "Total Gears" + " " + wantGire.ToString() + "/ 2";
    }

    public void DIsplayGire()
    {
        wantGire = player.Gire;

        GireCountText.text = "Total Gears" + " " + wantGire.ToString() + "/ 2";

        if (wantGire >= 2)
        {
            FinishText.gameObject.SetActive(true);
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            gameManager.FinishGire();
        }
    }
}
