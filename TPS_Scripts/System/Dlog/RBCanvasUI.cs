using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class RBOptions
{
    public System.Action CloseDelegete;
}
public class RBCanvasUI : ViewController
{
    public static string prefabName = "RBCanvas";
    public static GameObject prefab;
    private RBOptions role;
    private bool isDownButton = false;

    public static RBCanvasUI Show(RBOptions rb)
    {
        if (prefab == null)
        {
            prefab = Resources.Load(prefabName) as GameObject;
        }

        GameObject obj = Instantiate(prefab);
        RBCanvasUI Dlog = obj.GetComponent<RBCanvasUI>();

        Dlog.UpdateContent(rb);
        return Dlog;
    }


    void UpdateContent(RBOptions rb)
    {
        Cursor.lockState = CursorLockMode.None;

        role = rb;
    }

    public void OnTapClose()
    {
        Debug.Log("呼ばれたよ");

        if (role.CloseDelegete != null)
            role.CloseDelegete.Invoke();

        Destroy(gameObject);
    }

    public void OnTapYes()
    {
        if (isDownButton == false)
        {
            Debug.Log("呼び出されたよ");
            isDownButton = true;
            SceneManager.LoadScene("StartScene");
            Time.timeScale = 1.0f;
        }
    }
}
