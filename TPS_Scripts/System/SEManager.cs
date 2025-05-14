using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance { get; private set; }

    [SerializeField] private AudioSource seSourcePrefab; // SE�p��AudioSource�i�v���n�u�j
    [SerializeField] private List<AudioClip> seClips; // SE���X�g
    [SerializeField] private float defaultVolume = 0.5f; // �f�t�H���g����

    private const string SEVolumeKey = "SEVolume"; // SE���ʂ�PlayerPrefs�L�[
    private float currentSEVolume; // ���݂�SE����
    private Dictionary<string, AudioClip> seDictionary = new Dictionary<string, AudioClip>();
    private List<AudioSource> seSources = new List<AudioSource>(); // SE�pAudioSource�v�[��

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // SE�pDictionary�쐬
            foreach (var clip in seClips)
            {
                seDictionary[clip.name] = clip;
            }

            // PlayerPrefs ���� SE �̉��ʂ����[�h
            currentSEVolume = PlayerPrefs.GetFloat(SEVolumeKey, defaultVolume);
            Debug.Log($"[SEManager] ��������: {currentSEVolume}");
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// �X���C�_�[����SE�̉��ʂ�ύX
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

        Debug.Log($"[SEManager] SE���ʂ�ύX: {currentSEVolume}");
    }

    /// <summary>
    /// �w�肵��SE���Đ�
    /// </summary>
    public void PlaySE(string seName)
    {
        if (!seDictionary.ContainsKey(seName))
        {
            Debug.LogWarning($"[SEManager] SE '{seName}' ��������܂���I");
            return;
        }

        AudioSource seSource = GetAvailableSESource();
        if (seSource == null)
        {
            Debug.LogError("[SEManager] AudioSource ���擾�ł��܂���ł����I");
            return;
        }

        // SE ���Z�b�g���čĐ�
        seSource.clip = seDictionary[seName];
        seSource.volume = currentSEVolume;
        seSource.mute = false; // �~���[�g����
        seSource.enabled = true;
        seSource.gameObject.SetActive(true);
        seSource.Play();

        // �f�o�b�O�p���O
        Debug.Log($"[SEManager] {seName} ���Đ����܂��B");
        Debug.Log($"[SEManager] AudioClip: {seSource.clip}");
        Debug.Log($"[SEManager] Volume: {seSource.volume}");
        Debug.Log($"[SEManager] AudioSource ���L����: {seSource.enabled}");
    }

    /// <summary>
    /// SE���Đ�����AudioSource���擾�i�Ȃ���ΐV�������j
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
                Debug.Log($"[SEManager] ������ AudioSource ���ė��p");
                return source;
            }
        }

        if (seSourcePrefab == null)
        {
            Debug.LogError("[SEManager] seSourcePrefab ���ݒ肳��Ă��܂���I");
            return null;
        }

        AudioSource newSource = Instantiate(seSourcePrefab, transform);
        newSource.enabled = true;
        newSource.gameObject.SetActive(true);
        newSource.playOnAwake = false;
        seSources.Add(newSource);

        Debug.Log($"[SEManager] �V���� AudioSource ���쐬");
        return newSource;
    }
}
