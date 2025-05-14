using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Slider bgmSlider;
    private float bgmVolume;
    public Slider seSlider;
    private float seVolume;

    private BGMManager bgmManager;
    private AudioSource bgmAudio;

    private SEManager seManager;
    private AudioSource seAudio;

    void Start()
    {
        bgmSlider = GameObject.Find("BGM_Slider").GetComponent<Slider>();

        if (bgmSlider != null)
        {
            bgmManager = FindFirstObjectByType<BGMManager>();

            if (bgmManager != null)
            {
                bgmAudio = bgmManager.GetComponent<AudioSource>();
                bgmVolume = bgmAudio.volume;
                bgmSlider.value = bgmVolume;
            }

            else if (bgmManager == null)
            {
                Debug.LogError("BGMManagerが存在していません");
            }
        }

        else
        {
            Debug.LogError("BGMSliderが存在していません");
        }



        seSlider = GameObject.Find("SE_Slider").GetComponent<Slider>();

        if (seSlider != null)
        {
            seManager = FindFirstObjectByType<SEManager>();

            if (seManager != null)
            {
                seAudio = seManager.GetComponent<AudioSource>();
                seVolume = seAudio.volume;
                seSlider.value = seVolume;
            }

            else if (seManager == null)
            {
                Debug.LogError("SEManagerが存在していません");
            }
        }

        else
        {
            Debug.LogError("SESliderが存在していません");
        }
    }

    private void Update()
    {
        bgmAudio.volume = bgmSlider.value;
        seAudio.volume = seSlider.value;
    }
}
