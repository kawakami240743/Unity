using UnityEngine;
using UnityEngine.UI;
using Game;

public class HPSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private EnemyStatus enemyStatus; // Inspectorで設定するのではなく、コードで渡す
    private float currentEnemyHP;

    // EnemyStatus を設定する
    public void SetTarget(float enemyHP)
    {
            currentEnemyHP = enemyHP;

            slider.maxValue = currentEnemyHP;
            slider.value = currentEnemyHP;
    }

    public void Damage(float damageAmount)
    {

        currentEnemyHP -= (int)damageAmount;

        if (currentEnemyHP < 0)
        {
            currentEnemyHP = 0;
        }

        slider.value = currentEnemyHP; // スライダーの値を更新
    }
}
