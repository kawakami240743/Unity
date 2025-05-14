using UnityEngine;
using UnityEngine.UI;

public class PlayerHPSlider : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Slider slider;
    void Start()
    {
        slider.maxValue = 300;
        slider.value = 300;

        if (playerController == null || playerController.playerStatus == null)
        {
            Debug.LogError("PlayerController または playerStatus が設定されていません");
            return;
        }

        slider.value = playerController.playerStatus.hp;
    }

    public void Damage()
    {
        slider.value = playerController.playerStatus.hp;
    }
}
