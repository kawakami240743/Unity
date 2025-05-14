using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class DashGaugeController : MonoBehaviour
{
    [SerializeField] private DashButton dashButton;
    [SerializeField] private Image dashGauge;
    [SerializeField] public GameObject StaminaGauge;
    [SerializeField] private float gauge = 1f;
    [SerializeField] private float decreaseGauge = 0.2f;
    [SerializeField] private float increaseGauge = 0.1f;
    [SerializeField] private float maxGauge = 1f;
    [SerializeField] private float interval = 3f;


    private void Start()
    {
        dashGauge.fillAmount = gauge;
        StaminaGauge.gameObject.SetActive(false);
    }

    private void Update()
    {
       if (gauge >= 1f)
        {
            StaminaGauge.gameObject.SetActive(false);
        }
    }

    public void GaugeDecrease()
    {
        if (gauge <= 0)
        {
            dashButton.pointerUp();
        }

        else if (dashButton.onDashButton)
        {
            gauge -= decreaseGauge * Time.deltaTime;
            gauge = Mathf.Clamp(gauge, 0f, 1f);

            dashGauge.fillAmount = gauge;
            Debug.Log("スタミナ残量:" + gauge);
        }
    }

    public void GaugeIncrease()
    {

        if (dashButton.Timer >= interval && gauge < maxGauge)
        {
            gauge += increaseGauge * Time.deltaTime;
            gauge = Mathf.Clamp(gauge, 0f, maxGauge);

            dashGauge.fillAmount = gauge;
            Debug.Log("スタミナ残量" + gauge);
        }
    }
}
