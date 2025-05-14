using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MyGame.Items;
using UnityEditor.Experimental.GraphView;

public class ItemSelector : MonoBehaviour
{
    public GameObject itemWheelUI;
    public List<ItemSlotUI> itemSlots;
    public Image selectedItemIcon; // 🔹 **左下のアイテムスロットのアイコン**
    public Text lowerLeftSlotQuantityText; // 🔹 **左下スロットの個数表示**

    private string selectedItemName;
    private bool isSelecting = false;
    private float keyPressTime = 0f;
    [SerializeField] private ItemHolder itemHolder;

    [SerializeField] private Text small;
    [SerializeField] private Text medium;
    [SerializeField] private Text large;

    private int highlightedIndex = -1; // 現在ハイライトされているスロット

    void Start()
    {
        Dictionary<string, Item> items = itemHolder.GetItems();

        // 🔹 **スロットにアイテムを割り当てる**
        int i = 0;
        foreach (var item in items)
        {
            if (i < itemSlots.Count)
            {
                itemSlots[i].AssignItem(item.Value);
            }
            i++;
        }

        // 🔹 **持っているアイテムの中で最初に `small` を選ぶ**
        if (items.ContainsKey("small") && items["small"].Quantity > 0)
        {
            selectedItemName = "small";
            Debug.Log($"✅ 初期アイテムとして {selectedItemName} を選択！");
        }
        else
        {
            Debug.LogWarning("⚠️ `small` のアイテムがないため、選択できません！");
            selectedItemName = null;
        }

        UpdateItemCountText();
    }


    void Update()
    {
        HandleItemSelection();
        UpdateItemCountText();

        if (!string.IsNullOrEmpty(selectedItemName))
        {
            SetCurrentSelectedItem(selectedItemName); // 🔹 **常に最新の状態を反映**
        }
    }


    void UpdateItemCountText()
    {
        Dictionary<string, Item> items = itemHolder.GetItems();

        small.text = items.ContainsKey("small") ? $"{items["small"].Quantity}" : "0"; 
        medium.text = items.ContainsKey("medium") ? $"{items["medium"].Quantity}" : "0";
        large.text = items.ContainsKey("large") ? $"{items["large"].Quantity}" : "0";
    }

    void HandleItemSelection()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            keyPressTime = Time.time; // ✅ `Time.deltaTime` → `Time.time` に修正！
            isSelecting = false; // ✅ ここで `true` にしない！
        }

        if (Input.GetKey(KeyCode.E))
        {
            float holdTime = Time.time - keyPressTime;

            if (!isSelecting && holdTime >= 0.2f) // **0.2秒以上ホールドでメニューを開く**
            {
                OpenItemSelector();
                isSelecting = true;
            }
        }

        if (isSelecting)
        {
            Vector2 mousePos = Input.mousePosition;
            Debug.Log($"🖱 マウス位置: {mousePos}");
            HighlightItemByMouseDirection(mousePos);
        }


        if (Input.GetKeyUp(KeyCode.E))
        {
            if (isSelecting)
            {
                ConfirmItemSelection(); // **選択確定**
                CloseItemSelector(); // **UIを閉じる**
            }
            else
            {
                HandleItemUse();
            }
        }
    }


    void OpenItemSelector()
    {
        itemWheelUI.SetActive(true);
        isSelecting = true;

        Debug.Log("📌 アイテムセレクターをオープン");
    }

    void CloseItemSelector()
    {
        itemWheelUI.SetActive(false);
        isSelecting = false;

        Debug.Log("📌 アイテムセレクターをクローズ");
    }

    void ConfirmItemSelection()
    {
        Dictionary<string, Item> items = itemHolder.GetItems();

        if (highlightedIndex < 0 || highlightedIndex >= itemSlots.Count)
        {
            Debug.LogWarning("⚠️ アイテムが選択されていません！（highlightedIndex の範囲外）");

            // **持っているアイテムがあるなら最初のものを選択**
            foreach (var item in items)
            {
                if (item.Value.Quantity > 0) // **所持数が1以上のアイテムを選ぶ**
                {
                    selectedItemName = item.Key;
                    Debug.Log($"✅ デフォルトで {selectedItemName} を選択！");
                    break;
                }
            }

            if (string.IsNullOrEmpty(selectedItemName))
            {
                Debug.LogError("🚨 アイテムを1つも持っていないため、選択できません！");
                selectedItemName = null;
                return;
            }
        }
        else
        {
            selectedItemName = GetItemNameByIndex(highlightedIndex);
        }

        Debug.Log($"✅ アイテム選択: {selectedItemName}");
        SetCurrentSelectedItem(selectedItemName); // 🔹 **選択アイテムを強制的に設定**
    }

    void HighlightItemByMouseDirection(Vector2 mousePos)
    {
        float bestAngle = 360f;
        int bestIndex = -1;

        for (int i = 0; i < itemSlots.Count; i++)
        {
            Vector2 itemPosition = itemSlots[i].transform.position;
            Vector2 direction = itemPosition - mousePos;
            float angle = Vector2.Angle(Vector2.up, direction);

            if (angle < bestAngle)
            {
                bestAngle = angle;
                bestIndex = i;
            }
        }

        if (bestIndex != highlightedIndex)
        {
            if (highlightedIndex != -1)
                itemSlots[highlightedIndex].Deselect(); // 以前のスロットのハイライト解除

            highlightedIndex = bestIndex;
            itemSlots[highlightedIndex].Highlight(); // 新しいスロットをハイライト
        }
    }

    void SetCurrentSelectedItem(string itemName)
    {
        Dictionary<string, Item> items = itemHolder.GetItems();

        if (!string.IsNullOrEmpty(itemName) && items.ContainsKey(itemName))
        {
            Item selectedItem = items[itemName];

            string iconPath = $"Icons/{selectedItem.Name.ToLower()}";

            // ✅ **アイコンを `Resources.Load<Sprite>()` でロードし、そのまま適用**
            Sprite itemSprite = Resources.Load<Sprite>($"Play/{selectedItem.Name.ToLower()}");

            if (itemSprite != null)
            {
                selectedItemIcon.sprite = itemSprite;
            }
            else
            {
                Debug.LogWarning($"⚠️ {selectedItem.Name} のスプライトが `Resources/Icons/` に見つかりません！");
            }

            lowerLeftSlotQuantityText.text = $"{selectedItem.Quantity}";
        }
        else
        {
            lowerLeftSlotQuantityText.text = "0";
        }
    }





    void HandleItemUse()
    {
        if (string.IsNullOrEmpty(selectedItemName))
        {
            Debug.LogError("🚨 `selectedItemName` が `null` または 空文字のため、アイテムを使用できません！");
            return;
        }

        Debug.Log($"🛠️ アイテム使用リクエスト: {selectedItemName}");

        bool used = itemHolder.UseItem(selectedItemName, gameObject);
        if (used)
        {
            UpdateItemCountText();
            AutoSelectNextItem();
            SetCurrentSelectedItem(selectedItemName);
        }
    }



    void AutoSelectNextItem()
    {
        Dictionary<string, Item> items = itemHolder.GetItems();

        foreach (var item in items)
        {
            if (item.Value.Quantity > 0)
            {
                selectedItemName = item.Key;
                Debug.Log($"✅ 自動で {selectedItemName} を選択！");
                return;
            }
        }

        Debug.LogWarning("⚠️ すべてのアイテムを使い切ったため、選択を解除します！");
        selectedItemName = null;
    }





    string GetItemNameByIndex(int index)
    {
        switch (index)
        {
            case 0: return "small";
            case 1: return "medium";
            case 2: return "large";
            default: return "small";
        }
    }
}
