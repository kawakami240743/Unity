using UnityEngine;
using UnityEngine.UI;

public class AudioOptions
{
    public System.Action CloseDelegete;
}

public class AudioCanvasUI : ViewController
{
    public static string prefabName = "AudioCanvas";
    public static GameObject prefab;
    private AudioOptions audio;

    public static AudioCanvasUI Show(AudioOptions ad)
    {
        if (prefab == null)
        {
            prefab = Resources.Load(prefabName) as GameObject;
        }

        GameObject obj = Instantiate(prefab);
        AudioCanvasUI Dlog = obj.GetComponent<AudioCanvasUI>();

        Dlog.UpdateContent(ad);
        return Dlog;
    }


    void UpdateContent(AudioOptions ad)
    {
        Cursor.lockState = CursorLockMode.None;

        audio = ad;
    }

    public void OnTapClose()
    {
        Debug.Log("呼ばれたよ");

        if (audio.CloseDelegete != null)
            audio.CloseDelegete.Invoke();

        Destroy(gameObject);
    }
}
