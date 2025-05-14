using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("AudioManager が見つかりません！");
            return;
        }

        // PlayerPrefs から音量をロード
        float savedVolume = PlayerPrefs.GetFloat("BGMVolume", 0.3f);
        volumeSlider.value = savedVolume;

        // AudioManager に保存されている音量を適用
        AudioManager.Instance.SetVolume(savedVolume);

        // スライダーの変更時に BGM の音量を即時反映
        volumeSlider.onValueChanged.AddListener((value) =>
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetVolume(value);
            }
        });
    }
}
