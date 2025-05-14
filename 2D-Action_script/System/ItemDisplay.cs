using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    public GameObject itemPanel; // アイテムを表示するパネル
    public Image itemImage; // アイテム表示を変えるやつ

    [SerializeField] private Sprite[] itemSprites; // image

    private void Start()
    {
        itemPanel.SetActive(false); //??????????????
    }

    public void ItemSet(int itemId)
    {
        if (itemId >= 0 && itemId < itemSprites.Length)
        {
            itemImage.sprite = itemSprites[itemId];
            itemPanel.SetActive(true); // ????????????????????????
        }
        else
        {
            Debug.LogError("Invalid item ID: " + itemId);
        }
    }

    public void ItemDestroy()
    {
        itemPanel.SetActive(false); // ?????????????????
    }
}
