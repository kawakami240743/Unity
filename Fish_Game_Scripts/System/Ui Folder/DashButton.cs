using UnityEngine;

public class DashButton : MonoBehaviour
{
    [SerializeField] private Controller controller;
    [SerializeField] private DashGaugeController dashGauge;
    public bool onDashButton = false;
    public float Timer;

    private void Start()
    {
        if (controller == null)
        {
            Debug.LogError("controllerコンポーネントが存在しません。");
        }

        if (dashGauge == null)
        {
            Debug.LogError("dashGaugeコンポーネントが存在しません。");
        }
    }

    private void Update()
    {
        if (onDashButton)
        {
            controller.OnDashButton();
            dashGauge.GaugeDecrease();
            Timer = 0f;
        }

        if (onDashButton == false)
        {
            dashGauge.GaugeIncrease();
            Timer += Time.deltaTime;
        }
    }

    public void pointerDown()
    {
        Debug.Log("downButton");
        onDashButton = true;
        dashGauge.StaminaGauge.gameObject.SetActive(true);
    }

    public void pointerUp()
    {
        Debug.Log("upButton");
        onDashButton = false;
    }
}
