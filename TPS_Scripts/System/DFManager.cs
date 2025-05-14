using UnityEngine;

public class DFManager : MonoBehaviour
{
    private static DFManager instance;
    private int currentDifficulty;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GetDifficulty(int difficulty)
    {
        currentDifficulty = difficulty;
        Debug.Log("現在の難易度" +  currentDifficulty);
    }

    public int GetCurrentDifficulty()
    {
        return this.currentDifficulty;
    }
}
