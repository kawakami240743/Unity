using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript Instance { get; private set; }
    public string CurrentSceneName { get; private set; }

    [SerializeField] private int StartScene = 0;
    [SerializeField] private int SettingScene = 1;
    [SerializeField] public int MapScene = 2;
    [SerializeField] public int DifficultyScene = 3;
    [SerializeField] public int GameScene = 4;
    [SerializeField] public int GameOverScene = 5;
    [SerializeField] public int GameClearScene = 6;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int sceneIndex = scene.buildIndex; // 現在のシーンのインデックスを取得
        CurrentSceneName = scene.name; // 現在のシーン名を記録
        Debug.Log($"シーン変更: {scene.name} (Index: {sceneIndex})");

        // シーンごとの BGM を設定
        switch (sceneIndex)
        {
            case 0: // スタート画面
                AudioManager.Instance.PlaySceneBGM(0);
                break;

            case 1: // 設定画面
                AudioManager.Instance.PlaySceneBGM(1);
                break;

            case 2: // マップシーン
                AudioManager.Instance.PlaySceneBGM(2);
                break;

            case 3: // 難易度選択画面
                AudioManager.Instance.PlaySceneBGM(3);
                break;

            case 4: // ゲームプレイ
                AudioManager.Instance.PlaySceneBGM(4);
                break;

            case 5: // ゲームオーバー
                AudioManager.Instance.PlayGameOverBGM();
                break;

            case 6: // ゲームクリア
                AudioManager.Instance.PlayGameClearBGM();
                break;

            default: // 未設定のシーン
                Debug.LogWarning($"未設定のシーン: {scene.name}");
                break;
        }
    }
}
