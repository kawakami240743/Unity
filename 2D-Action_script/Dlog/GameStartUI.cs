using UnityEngine;

public class startOptions
{
    public System.Action CloseDelegete;
}

public class GameStartUI : ViewController
{
    public static string prefabName = "Start_Canvas";
    public static GameObject prefab;
    private startOptions start;
    public GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public static GameStartUI Show(startOptions st)
    {
        if(prefab == null)
        {
            prefab = Resources.Load(prefabName) as GameObject;
        }
        GameObject obj = Instantiate(prefab);
        GameStartUI Dlog = obj.GetComponent<GameStartUI>();

        Dlog.UpdateContent(st);
        return Dlog;
    }

    void UpdateContent(startOptions st)
    {
        Cursor.lockState = CursorLockMode.None;
        start = st;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnTapClose();
        }
    }

    void OnTapClose()
    {
        if (start.CloseDelegete != null)
            start.CloseDelegete.Invoke();

        gameManager.OnGameStartDestroyed();
        Destroy(gameObject);
    }
}
