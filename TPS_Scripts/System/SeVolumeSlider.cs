using UnityEngine;
using UnityEngine.UI;

public class SeVolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider seVolumeSlider;

    private void Start()
    {
        if (SEManager.Instance == null)
        {
            Debug.LogWarning("SEManager ��������܂���I");
            return;
        }

        // PlayerPrefs ���� SE �̉��ʂ����[�h
        float savedSEVolume = PlayerPrefs.GetFloat("SEVolume", 0.5f);
        seVolumeSlider.value = savedSEVolume;

        // SEManager �ɕۑ�����Ă��鉹�ʂ�K�p
        SEManager.Instance.SetSEVolume(savedSEVolume);

        // �X���C�_�[�̕ύX���� SE �̉��ʂ𑦎����f
        seVolumeSlider.onValueChanged.AddListener((value) =>
        {
            SEManager.Instance.SetSEVolume(value);
        });
    }
}
