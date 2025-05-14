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
        int sceneIndex = scene.buildIndex; // ���݂̃V�[���̃C���f�b�N�X���擾
        CurrentSceneName = scene.name; // ���݂̃V�[�������L�^
        Debug.Log($"�V�[���ύX: {scene.name} (Index: {sceneIndex})");

        // �V�[�����Ƃ� BGM ��ݒ�
        switch (sceneIndex)
        {
            case 0: // �X�^�[�g���
                AudioManager.Instance.PlaySceneBGM(0);
                break;

            case 1: // �ݒ���
                AudioManager.Instance.PlaySceneBGM(1);
                break;

            case 2: // �}�b�v�V�[��
                AudioManager.Instance.PlaySceneBGM(2);
                break;

            case 3: // ��Փx�I�����
                AudioManager.Instance.PlaySceneBGM(3);
                break;

            case 4: // �Q�[���v���C
                AudioManager.Instance.PlaySceneBGM(4);
                break;

            case 5: // �Q�[���I�[�o�[
                AudioManager.Instance.PlayGameOverBGM();
                break;

            case 6: // �Q�[���N���A
                AudioManager.Instance.PlayGameClearBGM();
                break;

            default: // ���ݒ�̃V�[��
                Debug.LogWarning($"���ݒ�̃V�[��: {scene.name}");
                break;
        }
    }
}
