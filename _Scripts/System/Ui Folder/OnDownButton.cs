using UnityEngine;

public class OnDownButton : MonoBehaviour
{
    [SerializeField] public int seNum;

    public void PlaySE()
    {
        if (SEManager.instance != null)
        {
            SEManager.instance.PlaySound(seNum);
        }

        else
        {
            Debug.LogError("MusicManagerのインスタンスが見つかりません");
        }
    }
}
