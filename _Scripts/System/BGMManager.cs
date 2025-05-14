using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] bgm;

    public static BGMManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        PlayBGM();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)　//Sceneロード時
    {
        PlayBGM();
    }

    private void PlayBGM()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        int bgmIndex = GetBGMIndexForScene(sceneName);

        if (bgmIndex != -1)
        {
            audioSource.clip = bgm[bgmIndex];
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("対応するBGMが見つかりません: " + sceneName);
        }
    }

    private int GetBGMIndexForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "StartScene":
                return 0;
            case "GameScene":
                return 1;
            case "RankingScene":
                return 2;
            default:
                return -1;
        }
    }

    public void StopSound()
    {
        audioSource.loop = false;
        audioSource.Stop();
    }
}
