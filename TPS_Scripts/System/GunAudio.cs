using UnityEngine;

public class GunAudio : MonoBehaviour
{
    public void ShotAudio()
    {
        SEManager.Instance.PlaySE("Gun");
    }
}
