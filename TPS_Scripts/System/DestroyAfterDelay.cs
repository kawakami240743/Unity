using UnityEngine;
using RayFire;

public class DestroyAfterDelay : MonoBehaviour
{ 
    public void Destroyed()
    {
        // 5秒後にこのオブジェクト（親ごと子も）削除
        Destroy(gameObject);
    }
}
