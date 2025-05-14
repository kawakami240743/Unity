using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBossSE : MonoBehaviour
{
    AudioClip Start_Boss_SE;
    AudioSource audioSource;

    public void BossSE()
    {
        audioSource.PlayOneShot(Start_Boss_SE);
    }
}
