using UnityEngine;
using UnityEngine.UI;

public class DrOptions
{
    public System.Action CloseDelegete;
}

public class GameDrUI : MonoBehaviour
{
    [SerializeField] private Button drButton;
    public static string prefabName = "Describe_Canvas";
    public static GameObject prefab;
    private DrOptions Describe;

    private void Start()
    {
        drButton.onClick.AddListener(DrClose);
    }

    public static GameDrUI Show(DrOptions dr)
    {
        if(prefab == null)
        {
            prefab = Resources.Load(prefabName) as GameObject;
        }
        GameObject obj = Instantiate(prefab);
        GameDrUI drUI = obj.GetComponent<GameDrUI>();

        drUI.UpdateContent(dr);
        return drUI;
    }

    void UpdateContent(DrOptions dr)
    {
        Cursor.lockState = CursorLockMode.None;
        Describe = dr;
    }

    void DrClose()
    {
        if (Describe.CloseDelegete != null)
            Describe.CloseDelegete.Invoke();

        Destroy(gameObject);
    }
}
