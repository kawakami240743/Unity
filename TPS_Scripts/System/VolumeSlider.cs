using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("AudioManager ��������܂���I");
            return;
        }

        // PlayerPrefs ���特�ʂ����[�h
        float savedVolume = PlayerPrefs.GetFloat("BGMVolume", 0.3f);
        volumeSlider.value = savedVolume;

        // AudioManager �ɕۑ�����Ă��鉹�ʂ�K�p
        AudioManager.Instance.SetVolume(savedVolume);

        // �X���C�_�[�̕ύX���� BGM �̉��ʂ𑦎����f
        volumeSlider.onValueChanged.AddListener((value) =>
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetVolume(value);
            }
        });
    }
}
