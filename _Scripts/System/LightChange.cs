using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightChange : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Light directionalLight;
    private Vector3 middleClass = new Vector3(0, -1, 0);
    private Vector3 lowerClass = new Vector3(0, -150, 0);



    void Start()
    {
        if (directionalLight == null)
        {
            directionalLight = GameObject.FindFirstObjectByType<Light>();
        }
    }

    void Update()
    {
        if (playerTransform == null)
        {
            return;
        }

        if (playerTransform.position.y <= middleClass.y && directionalLight != null)
        {
            directionalLight.transform.rotation = Quaternion.Euler(0f, 0, 0);

            if (playerTransform.position.y <= lowerClass.y)
            {
                directionalLight.transform.rotation = Quaternion.Euler(-45f, 0, 0);
            }
        }

        else
        {
            directionalLight.transform.rotation = Quaternion.Euler(45f, 0, 0);
        }
    }
}
