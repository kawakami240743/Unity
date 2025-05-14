using UnityEngine;
using UnityEngine.UI;

public class ChangeCanvas : MonoBehaviour
{
    [SerializeField] Canvas[] canvas;

    void Start()
    {
        if (canvas == null)
        {
            Debug.LogError("canvasコンポーネントが存在しません");
        }

        if (canvas.Length > 0)
        {
            int randomIndex = Random.Range(0, canvas.Length);

            Canvas selectCanvas = canvas[randomIndex];

            for (int i = 0; i < canvas.Length; i++)
            {
                canvas[i].gameObject.SetActive(i == randomIndex);
            }
        }
    }
}
