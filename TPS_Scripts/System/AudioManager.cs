using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip[] sceneBGMs;
    [SerializeField] private AudioClip gameOverBGM;
    [SerializeField] private AudioClip gameClearBGM;
    [SerializeField] private float defaultVolume = 0.5f; // デフォルト音量

    private bool isGameOverOrClear = false;
    private const string VolumeKey = "BGMVolume"; // PlayerPrefs のキー
    private float currentVolume; // 現在の音量を保持

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // PlayerPrefs から音量をロード（デフォルト値 0.5f）
            currentVolume = PlayerPrefs.GetFloat(VolumeKey, defaultVolume);
            bgmSource.volume = currentVolume; // 正しい音量をセット
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// スライダーから音量を変更する
    /// </summary>
    public void SetVolume(float volume)
    {
        currentVolume = volume;
        bgmSource.volume = volume; // すぐに適用
        PlayerPrefs.SetFloat(VolumeKey, volume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// シーンごとの BGM を即時再生（音量を正しく維持）
    /// </summary>
    public void PlaySceneBGM(int sceneIndex)
    {
        if (sceneIndex < 0 || sceneIndex >= sceneBGMs.Length)
        {
            Debug.LogWarning($"BGMが設定されていないシーン Index: {sceneIndex}");
            return;
        }

        if (bgmSource.clip == sceneBGMs[sceneIndex]) return; // 同じBGMなら何もしない

        // BGM を即時変更
        bgmSource.Stop();
        bgmSource.clip = sceneBGMs[sceneIndex];
        bgmSource.loop = true;
        bgmSource.Play();

        // 音量を保持（リセットされないようにする）
        bgmSource.volume = currentVolume;
    }

    /// <summary>
    /// ゲームオーバーBGMを1回だけ再生
    /// </summary>
    public void PlayGameOverBGM()
    {
        if (isGameOverOrClear) return;
        isGameOverOrClear = true;

        bgmSource.Stop();
        bgmSource.clip = gameOverBGM;
        bgmSource.loop = false;
        bgmSource.Play();
        bgmSource.volume = currentVolume; // 音量を維持
    }

    /// <summary>
    /// ゲームクリアBGMを1回だけ再生
    /// </summary>
    public void PlayGameClearBGM()
    {
        if (isGameOverOrClear) return;
        isGameOverOrClear = true;

        bgmSource.Stop();
        bgmSource.clip = gameClearBGM;
        bgmSource.loop = false;
        bgmSource.Play();
        bgmSource.volume = currentVolume; // 音量を維持
    }

    public AudioSource GetAudioSource()
    {
        return bgmSource;
    }
}
