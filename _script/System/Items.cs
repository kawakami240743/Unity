using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public GameManager gameManager;
    public int itemId;
    public AudioClip SetItem;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.SetItemSound();
            Destroy(gameObject);
        }
    }
}
