using UnityEngine;
using UnityEngine.UI;

public class SeVolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider seVolumeSlider;

    private void Start()
    {
        if (SEManager.Instance == null)
        {
            Debug.LogWarning("SEManager が見つかりません！");
            return;
        }

        // PlayerPrefs から SE の音量をロード
        float savedSEVolume = PlayerPrefs.GetFloat("SEVolume", 0.5f);
        seVolumeSlider.value = savedSEVolume;

        // SEManager に保存されている音量を適用
        SEManager.Instance.SetSEVolume(savedSEVolume);

        // スライダーの変更時に SE の音量を即時反映
        seVolumeSlider.onValueChanged.AddListener((value) =>
        {
            SEManager.Instance.SetSEVolume(value);
        });
    }
}
