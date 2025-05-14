using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float distance = 5f;
    [SerializeField] float xSpeed = 10f;
    [SerializeField] float ySpeed = 10f;
    [SerializeField] float yMinLimit = -50f;
    [SerializeField] float yMaxLimit = 80f;
    [SerializeField] RectTransform cameraSwipe;

    private float xRotate;
    private float yRotate;
    private int joystickTouchID = -1;
    private int cameraTouchID = -1;

    private void Start()
    {
        xRotate = transform.eulerAngles.y;
        yRotate = transform.eulerAngles.x;
        UpdateCameraPosition();
    }

    private void Update()
    {
        HandleTouchInput();

        UpdateCameraPosition();
    }

    private void HandleTouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                if (!IsTouchCamera(touch.position))
                {
                    joystickTouchID = touch.fingerId;

                }

                else if (cameraTouchID == -1)
                {
                    cameraTouchID = touch.fingerId;
                }
            }

            else if (touch.phase == TouchPhase.Moved)
            {
                if (touch.fingerId == cameraTouchID)
                {
                    xRotate += touch.deltaPosition.x * xSpeed * Time.deltaTime;
                    yRotate -= touch.deltaPosition.y * ySpeed * Time.deltaTime;

                    yRotate = Mathf.Clamp(yRotate, yMinLimit, yMaxLimit);
                }
            }

            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (touch.fingerId == joystickTouchID)
                {
                    joystickTouchID = -1;
                }
                else if (touch.fingerId == cameraTouchID)
                {
                    cameraTouchID = -1;
                }
            }
        } 
    }

    private bool IsTouchCamera(Vector2 touchPosition)
    {
        Vector3[] swipeCamera = new Vector3[4];
        cameraSwipe.GetWorldCorners(swipeCamera);

        float left = swipeCamera[0].x;
        float right = swipeCamera[2].x;
        float bottom = swipeCamera[0].y;
        float top = swipeCamera[1].y;

        return touchPosition.x >= left && touchPosition.x <= right &&
               touchPosition.y >= bottom && touchPosition.y <= top;
    }

    private void UpdateCameraPosition()
    {
        if (playerTransform == null) return;

        UpdateCameraDistance();

        Quaternion rotation = Quaternion.Euler(yRotate, xRotate, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        transform.position = playerTransform.position + offset;

        transform.LookAt(playerTransform);
    }

    private void UpdateCameraDistance()
    {
            float scaleFactor = playerTransform.localScale.magnitude;
            distance = 6f + (scaleFactor - 1f) * 4.5f;
        if (distance < 10f)
        {
            distance = 10f;
        }
    }
}
