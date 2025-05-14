using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance { get; private set; }

    [SerializeField] private AudioSource seSourcePrefab; // SE用のAudioSource（プレハブ）
    [SerializeField] private List<AudioClip> seClips; // SEリスト
    [SerializeField] private float defaultVolume = 0.5f; // デフォルト音量

    private const string SEVolumeKey = "SEVolume"; // SE音量のPlayerPrefsキー
    private float currentSEVolume; // 現在のSE音量
    private Dictionary<string, AudioClip> seDictionary = new Dictionary<string, AudioClip>();
    private List<AudioSource> seSources = new List<AudioSource>(); // SE用AudioSourceプール

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // SE用Dictionary作成
            foreach (var clip in seClips)
            {
                seDictionary[clip.name] = clip;
            }

            // PlayerPrefs から SE の音量をロード
            currentSEVolume = PlayerPrefs.GetFloat(SEVolumeKey, defaultVolume);
            Debug.Log($"[SEManager] 初期音量: {currentSEVolume}");
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// スライダーからSEの音量を変更
    /// </summary>
    public void SetSEVolume(float volume)
    {
        currentSEVolume = volume;
        PlayerPrefs.SetFloat(SEVolumeKey, volume);
        PlayerPrefs.Save();

        foreach (var source in seSources)
        {
            if (source != null) source.volume = currentSEVolume;
        }

        Debug.Log($"[SEManager] SE音量を変更: {currentSEVolume}");
    }

    /// <summary>
    /// 指定したSEを再生
    /// </summary>
    public void PlaySE(string seName)
    {
        if (!seDictionary.ContainsKey(seName))
        {
            Debug.LogWarning($"[SEManager] SE '{seName}' が見つかりません！");
            return;
        }

        AudioSource seSource = GetAvailableSESource();
        if (seSource == null)
        {
            Debug.LogError("[SEManager] AudioSource が取得できませんでした！");
            return;
        }

        // SE をセットして再生
        seSource.clip = seDictionary[seName];
        seSource.volume = currentSEVolume;
        seSource.mute = false; // ミュート解除
        seSource.enabled = true;
        seSource.gameObject.SetActive(true);
        seSource.Play();

        // デバッグ用ログ
        Debug.Log($"[SEManager] {seName} を再生します。");
        Debug.Log($"[SEManager] AudioClip: {seSource.clip}");
        Debug.Log($"[SEManager] Volume: {seSource.volume}");
        Debug.Log($"[SEManager] AudioSource が有効か: {seSource.enabled}");
    }

    /// <summary>
    /// SEを再生するAudioSourceを取得（なければ新しく作る）
    /// </summary>
    private AudioSource GetAvailableSESource()
    {
        seSources.RemoveAll(source => source == null);

        foreach (var source in seSources)
        {
            if (source != null && !source.isPlaying)
            {
                source.enabled = true;
                source.gameObject.SetActive(true);
                Debug.Log($"[SEManager] 既存の AudioSource を再利用");
                return source;
            }
        }

        if (seSourcePrefab == null)
        {
            Debug.LogError("[SEManager] seSourcePrefab が設定されていません！");
            return null;
        }

        AudioSource newSource = Instantiate(seSourcePrefab, transform);
        newSource.enabled = true;
        newSource.gameObject.SetActive(true);
        newSource.playOnAwake = false;
        seSources.Add(newSource);

        Debug.Log($"[SEManager] 新しい AudioSource を作成");
        return newSource;
    }
}
