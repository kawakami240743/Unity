using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MyGame.Grenades;
using System.Linq;

public class GSelector : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject grenadeWheelUI;
    public List<GSlotUI> grenadeSlots;
    public Image selectedGrenadeIcon;
    public Text lowerLeftSlotQuantityText;

    [SerializeField] private GrenadeHolder grenadeHolder;
    public string selectedGrenadeName;
    private bool isSelecting = false;
    private float keyPressTime = 0f;
    private Dictionary<string, Grenade> grenades;  // 🔹 **メンバ変数として宣言**

    [SerializeField] private Text stone;
    [SerializeField] private Text grenade;

    private int highlightedIndex = -1;



    void Start()
    {
        Dictionary<string, Grenade> grenades = grenadeHolder.GetGrenades();
        this.grenades = grenadeHolder.GetGrenades();

        // 🔹 **スロットにグレネードを割り当てる**
        int i = 0;
        foreach (var grenade in grenades)
        {
            if (i < grenadeSlots.Count)
            {
                grenadeSlots[i].AssignGrenade(grenade.Value);
            }
            i++;
        }

        // 🔹 **最初に持っているグレネードを選ぶ**
        if (grenades.ContainsKey("stone") && grenades["stone"].Quantity > 0)
        {
            selectedGrenadeName = "stone";
        }
        else if (grenades.ContainsKey("grenade") && grenades["grenade"].Quantity > 0)
        {
            selectedGrenadeName = "grenade";
        }
        else
        {
            selectedGrenadeName = null;
            Debug.LogWarning("⚠️ どのグレネードも持っていません！");
        }

        UpdateGrenadeCountText();
    }

    void Update()
    {
        HandleGrenadeSelection();
        UpdateGrenadeCountText();

        if (!string.IsNullOrEmpty(selectedGrenadeName))
        {
            SetCurrentSelectedGrenade(selectedGrenadeName);
        }

        if (isSelecting) // **ホイールを開いている間はハイライトを更新**
        {
            HighlightGrenadeByMouseDirection(Input.mousePosition);
        }
    }

    private void HandleGrenadeSelection()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            keyPressTime = Time.time;
            isSelecting = false; // **初期状態リセット**
        }

        if (Input.GetKey(KeyCode.R))
        {
            float heldTime = Time.time - keyPressTime;

            if (!isSelecting && heldTime >= 0.2f) // **0.2秒以上長押ししたらUIを開く**
            {
                OpenGrenadeSelector();
                isSelecting = true; // **選択モードに入る**
            }
        }

        if (isSelecting)
        {
            Vector2 mousePos = Input.mousePosition;
            Debug.Log($"🖱 マウス位置: {mousePos}");
            HighlightGrenadeByMouseDirection(mousePos);
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            if (isSelecting)
            {
                CloseGrenadeSelector(); // **UIを閉じる**
                ConfirmGrenadeSelection(); // **選択確定を追加**
            }
            else
            {
                HandleGrenadeThrow(); // **短押しならグレネードを投げる**
            }
        }

    }

    private void OpenGrenadeSelector()
    {
        grenadeWheelUI.SetActive(true);
        Debug.Log("🛠️ グレネード選択UIを開く");

        // 🔹 **最初のハイライトを設定**
        if (highlightedIndex != -1 && highlightedIndex < grenadeSlots.Count)
        {
            grenadeSlots[highlightedIndex].Highlight();
        }
    }


    private void CloseGrenadeSelector()
    {
        grenadeWheelUI.SetActive(false);
        Debug.Log("✅ グレネード選択UIを閉じる");

        // 🔹 **ハイライトを解除**
        if (highlightedIndex != -1 && highlightedIndex < grenadeSlots.Count)
        {
            grenadeSlots[highlightedIndex].Deselect();
        }
    }


    void UpdateGrenadeCountText()
    {
        Dictionary<string, Grenade> grenades = grenadeHolder.GetGrenades();

        grenade.text = grenades.ContainsKey("grenade") ? $"{grenades["grenade"].Quantity}" : "0";
        stone.text = grenades.ContainsKey("stone") ? $"{grenades["stone"].Quantity}" : "0";
    }

    void HighlightGrenadeByMouseDirection(Vector2 mousePos)
    {
        float bestAngle = 360f;
        int bestIndex = -1;

        for (int i = 0; i < grenadeSlots.Count; i++)
        {
            Vector2 grenadePosition = grenadeSlots[i].transform.position;
            Vector2 direction = grenadePosition - mousePos;
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
                grenadeSlots[highlightedIndex].Deselect(); // 以前のハイライト解除

            highlightedIndex = bestIndex;
            grenadeSlots[highlightedIndex].Highlight(); // 新しいスロットをハイライト
        }
    }

    private void ConfirmGrenadeSelection()
    {
        Dictionary<string, Grenade> grenades = grenadeHolder.GetGrenades();

        if (highlightedIndex < 0 || highlightedIndex >= grenadeSlots.Count)
        {
            Debug.LogWarning("⚠️ グレネードが選択されていません！（highlightedIndex の範囲外）");

            // **持っているグレネードがあるなら最初のものを選択**
            foreach (var grenade in grenades)
            {
                if (grenade.Value.Quantity > 0)
                {
                    selectedGrenadeName = grenade.Key;
                    Debug.Log($"✅ デフォルトで {selectedGrenadeName} を選択！");
                    break;
                }
            }

            if (string.IsNullOrEmpty(selectedGrenadeName))
            {
                Debug.LogError("🚨 グレネードを1つも持っていないため、選択できません！");
                selectedGrenadeName = null;
                return;
            }
        }
        else
        {
            selectedGrenadeName = GetGrenadeNameByIndex(highlightedIndex);
        }

        Debug.Log($"✅ グレネード選択: {selectedGrenadeName}");
        Debug.Log($"🔍 `ConfirmGrenadeSelection()` 実行: highlightedIndex = {highlightedIndex}");

        SetCurrentSelectedGrenade(selectedGrenadeName);
    }


    void SetCurrentSelectedGrenade(string grenadeName)
    {


        Dictionary<string, Grenade> grenades = grenadeHolder.GetGrenades();

        if (!string.IsNullOrEmpty(grenadeName) && grenades.ContainsKey(grenadeName))
        {
            Grenade selectedGrenade = grenades[grenadeName];

            string iconPath = $"Play/{selectedGrenade.Name.ToLower()}";

            // ✅ **アイコンを `Resources.Load<Sprite>()` でロード**
            Sprite grenadeSprite = Resources.Load<Sprite>(iconPath);

            if (grenadeSprite != null)
            {
                selectedGrenadeIcon.sprite = grenadeSprite;
            }
            else
            {
                Debug.LogError($"🚨 `Resources.Load<Sprite>()` 失敗！ ファイルがない: {iconPath}");
            }

            lowerLeftSlotQuantityText.text = $"{selectedGrenade.Quantity}";
        }
        else
        {
            lowerLeftSlotQuantityText.text = "0";
        }
    }




    public bool ThrowGrenadeFromPlayer(Vector3 throwPosition, Vector3 throwDirection)
    {
        PlayerController playerController = FindFirstObjectByType<PlayerController>();

        if (playerController == null || string.IsNullOrEmpty(selectedGrenadeName))
        {
            Debug.LogError("🚨 `ThrowGrenadeFromPlayer()` - グレネードが選択されていません！");
            return false;
        }

        return playerController.ThrowGrenade(throwPosition, throwDirection, selectedGrenadeName);
    }


    void HandleGrenadeThrow()
    {
        if (string.IsNullOrEmpty(selectedGrenadeName))
        {
            Debug.LogError("🚨 `selectedGrenadeName` が `null` または 空文字のため、グレネードを投擲できません！");
            return;
        }

        Debug.Log($"🛠️ グレネード投擲リクエスト: {selectedGrenadeName}");
        PlayerController player = FindFirstObjectByType<PlayerController>();

        bool thrown = player.ThrowGrenade(transform.position, transform.forward, selectedGrenadeName);
        if (thrown)
        {
            grenadeHolder.UseGrenade(selectedGrenadeName); // ✅ **投擲後に所持数を減らす**
            UpdateGrenadeCountText();
            AutoSelectNextGrenade();
            SetCurrentSelectedGrenade(selectedGrenadeName);
        }
    }

    void AutoSelectNextGrenade()
    {
        Dictionary<string, Grenade> grenades = grenadeHolder.GetGrenades();

        foreach (var grenade in grenades)
        {
            if (grenade.Value.Quantity > 0)
            {
                selectedGrenadeName = grenade.Key;
                Debug.Log($"✅ 自動で {selectedGrenadeName} を選択！");
                return;
            }
        }

        Debug.LogWarning("⚠️ すべてのグレネードを使い切ったため、選択を解除します！");
        selectedGrenadeName = null;
    }

    string GetGrenadeNameByIndex(int index)
    {
        switch (index)
        {
            case 0: return "grenade";
            case 1: return "stone";
            default: return "grenade";
        }
    }


    public void UpdateSlotUI()
    {
        if (grenadeHolder == null) return;

        Dictionary<string, Grenade> grenades = grenadeHolder.GetGrenades();
        List<string> grenadeKeys = grenades.Keys.ToList(); // 🔹 **キーをそのままリスト化**

        grenades.Reverse();

        for (int i = 0; i < grenadeSlots.Count; i++)
        {
            if (i < grenades.Count)
            {
                string grenadeName = grenadeKeys[i]; // **リストの順番で取得**
                Debug.Log($"🎯 スロット[{i}] = {grenadeName}");

                grenadeSlots[i].AssignGrenade(grenades[grenadeName]); // 🔹 **所持数も渡す**
            }
            else
            {
                grenadeSlots[i].gameObject.SetActive(false); // **余分なスロットは非表示**
            }


        }
    }

    public GameObject GetSelectedGrenadePrefab()
    {
        if (!string.IsNullOrEmpty(selectedGrenadeName) && grenades.ContainsKey(selectedGrenadeName))
        {
            return grenades[selectedGrenadeName].GrenadePrefab;
        }
        return null;
    }

    public Grenade GetSelectedGrenade()
    {
        if (!string.IsNullOrEmpty(selectedGrenadeName) && grenades.ContainsKey(selectedGrenadeName))
        {
            return grenades[selectedGrenadeName];
        }
        return null;
    }

    public string GetSelectedGrenadeName()
    {
        return selectedGrenadeName;
    }

}
