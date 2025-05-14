using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private bool isDownButton = false;

    void Awake()
    {
        Time.timeScale = 1f;
    }

    public void OnDownButton()
    {
        if (isDownButton == false)
        {
            Debug.Log("呼び出されたよ");
            isDownButton = true;
            StartCoroutine(WaitForSwitchScene(1.2f));
        }

    }

    private IEnumerator WaitForSwitchScene(float waitTime)
    {
        Debug.Log("コルーチンスタート");
        yield return new WaitForSeconds(waitTime);

        isDownButton = false;

        SceneManager.LoadScene(sceneName);
    }

    public void ButtonSE()
    {
        SEManager.Instance.PlaySE("Button");
    }

    public void BackButton()
    {
        SEManager.Instance.PlaySE("cansel");
    }

}
