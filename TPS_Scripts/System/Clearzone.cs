using UnityEngine;

public class Clearzone : MonoBehaviour
{
    private Collider clearCollider;

    public void Clear()
    {
        clearCollider = GetComponent<Collider>();
        clearCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("あたったよ");
            GameManager gamemanager = GetComponent<GameManager>();
            gamemanager.GameClear();
        }
    }
}
