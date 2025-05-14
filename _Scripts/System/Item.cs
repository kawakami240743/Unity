using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] public int itemPower;
    [SerializeField] private Text itemText;
    void Start()
    {
        SetItemPower();
        UpdateUI();
    }

    private void SetItemPower()
    {
        itemPower = Random.Range(2, 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    private void UpdateUI()
    {
        itemText.text = "×" + itemPower.ToString();
    }
}
