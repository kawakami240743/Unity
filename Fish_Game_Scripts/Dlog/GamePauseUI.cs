using UnityEngine;
using UnityEngine.UI;

public class startOptions
{
    public System.Action CloseDelegete;
}

public class GamePauseUI : ViewController
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button rollbackButton;
    public static string prefabName = "Pause_Canvas";
    public static GameObject prefab;
    private startOptions start;

    private void Start()
    {
        startButton.onClick.AddListener(OnTapClose);
        rollbackButton.onClick.AddListener(CallRB);
    }

    public static GamePauseUI Show(startOptions st)
    {
        if (prefab == null)
        {
            prefab = Resources.Load(prefabName) as GameObject;
        }
        GameObject obj = Instantiate(prefab);
        GamePauseUI Dlog = obj.GetComponent<GamePauseUI>();

        Dlog.UpdateContent(st);
        return Dlog;
    }

    void UpdateContent(startOptions st)
    {
        Cursor.lockState = CursorLockMode.None;
        start = st;
    }

    void OnTapClose()
    {
        if (start.CloseDelegete != null)
            start.CloseDelegete.Invoke();

        Destroy(gameObject);
    }

    void CallRB()
    {
        RollBackOptions Rb = new RollBackOptions();
        RollBackUI.Show(Rb);
    }
}
