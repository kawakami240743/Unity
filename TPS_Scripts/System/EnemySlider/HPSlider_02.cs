using UnityEngine;
using UnityEngine.UI;

public class HPSlider_02 : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private float maxHP; // 最大HPを保持

    // HPを直接設定する
    public void SetTarget(float maxHealth)
    {
        maxHP = maxHealth;
        slider.maxValue = maxHP;
        slider.value = maxHP;
    }

    public void Damage(float currentHP)
    {
        if (slider == null)
        {
            Debug.LogError("スライダーが設定されていません");
            return;
        }

        if (currentHP < 0)
        {
            currentHP = 0;
        }

        slider.value = currentHP; // スライダーの値を更新
    }
}
