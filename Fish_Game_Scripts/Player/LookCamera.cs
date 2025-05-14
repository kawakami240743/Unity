using UnityEngine;

public class LookCamera : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

    private void Start()
    {
        targetCamera = Camera.main;
    }

    void Update()
    {
        if (targetCamera != null)
        {
            Vector3 direction = targetCamera.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(direction);

            // 180度回転させて文字の向きを修正
            transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
        }

        else
        {
            return;
        }
    }
}
