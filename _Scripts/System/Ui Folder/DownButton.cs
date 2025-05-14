using UnityEngine;

public class DownButton : MonoBehaviour
{
    [SerializeField] private Controller controller;
    public bool upButton = false;

    private void Start()
    {
        if(controller == null)
        {
            Debug.LogError("controllerコンポーネントが存在しません。");
        }
    }

    private void Update()
    {
        if (upButton)
        {
            controller.OnDownButton();
        }
    }

    public void pointerDown()
    {
        Debug.Log("downButton");
        upButton = true;
    }

    public void pointerUp()
    {
        Debug.Log("upButton");
        upButton = false;
    }
}
