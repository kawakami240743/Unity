using UnityEngine;
using UnityEngine.UI;

public class PauseOptions
{
    public System.Action CloseDelegete;
}
public class PauseCanvasUI : ViewController
{
    public static string prefabName = "PauseCanvas";
    public static GameObject prefab;
    private PauseOptions pause;

    public static PauseCanvasUI Show(PauseOptions pg)
    {
        if (prefab == null)
        {
            prefab = Resources.Load(prefabName) as GameObject;
        }

        GameObject obj = Instantiate(prefab);
        PauseCanvasUI Dlog = obj.GetComponent<PauseCanvasUI>();

        Dlog.UpdateContent(pg);
        return Dlog;
    }


    void UpdateContent(PauseOptions pg)
    {
        Cursor.lockState = CursorLockMode.None;

        pause = pg;
    }

    public void OnTapClose()
    {
        if (pause.CloseDelegete != null)
            pause.CloseDelegete.Invoke();

        Destroy(gameObject);
    }

    public void OnTapAudio()
    {
        AudioCanvasController audioController = FindFirstObjectByType<AudioCanvasController>();
        audioController.PauseGame();
    }

    public void OnTapRB()
    {
        RBCanvasController rbController = FindFirstObjectByType<RBCanvasController>();
        rbController.PauseGame();
    }
}
