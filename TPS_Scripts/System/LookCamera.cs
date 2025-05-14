using UnityEngine;

public class LookCamera : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        // **MainCamera を自動取得**
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("MainCamera が見つかりません！");
        }
    }

    void Update()
    {
        if (cameraTransform == null) return;

        transform.LookAt(cameraTransform);

        transform.rotation = Quaternion.LookRotation(transform.forward * -1);
    }
}
