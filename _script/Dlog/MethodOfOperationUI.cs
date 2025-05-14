using UnityEngine;

public class MethodOfOperations
{
    public System.Action CloseDelegete;
}

public class MethodOfOperationUI : ViewController
{
    public static string prefabName = "MethodOfOperation_Canvas";
    public static GameObject prefab;
    private MethodOfOperations method;

    public static MethodOfOperationUI Show(MethodOfOperations methodOf)
    {
        if(prefab == null)
        {
            prefab = Resources.Load(prefabName) as GameObject;
        }
        GameObject obj = Instantiate(prefab);
        MethodOfOperationUI methodUI = obj.GetComponent<MethodOfOperationUI>();

        methodUI.UpdateContent(methodOf);
        return methodUI;
    }

    void UpdateContent(MethodOfOperations methodOf)
    {
        Cursor.lockState = CursorLockMode.None;
        method = methodOf;
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
        if (method.CloseDelegete != null)
            method.CloseDelegete.Invoke();

        Destroy(gameObject);
    }
}
