using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool isfinishGire;
    public bool pauseMenu;

    private void Start()
    {
        pauseMenu = false;
        Cursor.visible = false;     
    }

    void Update()
    {
        CursorLock();
    }


    private void CursorLock()
    {
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || pauseMenu)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void FinishGire()
    {
        isfinishGire = true;
        Clearzone clear = FindFirstObjectByType<Clearzone>();

        if (isfinishGire)
        {
            clear.Clear();
        }
    }

    public void GameClear()
    {
        
        Debug.Log("GameClear!");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("GameClearScene");
        Debug.Log("シーンをロード");
    }

    public void GameOver()
    {
        Debug.Log("GameOver!");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("GameOverScene");
    }
}
