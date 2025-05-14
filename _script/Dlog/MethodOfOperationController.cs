/*using UnityEngine;

public class MethodOfOperationController : MonoBehaviour
{
    private bool isMeOfOp = false;

    void Update()
    {
        if (isMeOfOp)
            return;
    }

    public void MethodOfOprerationPause()
    {
        MethodOfOperations method = new MethodOfOperations();
        method.CloseDelegete = () =>
        {
            MeOfOpStart();
        };
        MethodOfOperationUI.Show(method);

        Time.timeScale = 0f;
        isMeOfOp = true;
    }

    void MeOfOpStart()
    {
        Time.timeScale = 1f;
        isMeOfOp = true;
    }
}*/

using UnityEngine;

public class MethodOfOperationController : MonoBehaviour
{
    private bool isMeOfOp = false;

    void Start()
    {
        MethodOfOprerationPause();
    }

    void Update()
    {
        if (isMeOfOp)
            return;
    }

    public void MethodOfOprerationPause()
    {
        MethodOfOperations method = new MethodOfOperations();
        method.CloseDelegete = () =>
        {
            MeOfOpStart();
        };
        MethodOfOperationUI.Show(method);

        Time.timeScale = 0f;
        isMeOfOp = true;
    }

    void MeOfOpStart()
    {
        Time.timeScale = 1f;
        isMeOfOp = true;
    }
}
