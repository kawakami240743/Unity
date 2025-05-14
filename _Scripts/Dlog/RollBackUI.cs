using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RollBackOptions
{
    public System.Action rbClose;
}

public class RollBackUI : MonoBehaviour
{
    [SerializeField] private Button callTitleButton;
    [SerializeField] private Button backPauseButton;
    [SerializeField] private string SceneName;
    public static string prefabName = "RollBack_Canvas";
    public static string pausePrefab = "Pause_Canvas";
    public static GameObject prefab;
    private RollBackOptions rollback;

    private void Start()
    {
        callTitleButton.onClick.AddListener(CallTitle);
        backPauseButton.onClick.AddListener(CallPause);
    }

    public static RollBackUI Show(RollBackOptions Rb)
    {
        if (prefab == null)
        {
            prefab = Resources.Load(prefabName) as GameObject;
        }
        GameObject obj = Instantiate(prefab);
        RollBackUI roll = obj.GetComponent<RollBackUI>();

        roll.UpdateContent(Rb);
        return roll;
    }

    void UpdateContent(RollBackOptions Rb)
    {
        Cursor.lockState = CursorLockMode.None;
        rollback = Rb;
    }

    void CallTitle()
    {
        SceneManager.LoadScene(SceneName);
    }

    void CallPause()
    {
        if (rollback.rbClose != null)
            rollback.rbClose.Invoke();

        Destroy(gameObject);
    }
}
