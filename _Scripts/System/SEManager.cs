using UnityEngine;

public class SEManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] se;

    public static SEManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int i)
    {
        if (i >= 0 && i < se.Length)
        {
            audioSource.PlayOneShot(se[i]);
        }
    }
}
