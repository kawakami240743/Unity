using UnityEngine;
using UnityEngine.UI;

public class SwitchWeapon : MonoBehaviour
{
    [SerializeField] private Image weaponImage; // 武器アイコンを表示するUI
    [SerializeField] private Sprite[] weaponSprites; // 武器アイコンのスプライトを配列で管理

    public void UpdateWeaponSprite(bool isAssult, bool isSubmachine, bool isShot, bool isHand)
    {
        if (weaponSprites == null || weaponSprites.Length == 0)
        {
            Debug.LogError("❌ 武器スプライトの配列が設定されていません！");
            return;
        }

        int weaponIndex = -1;
        if (isAssult) weaponIndex = 0;
        else if (isSubmachine) weaponIndex = 1;
        else if (isShot) weaponIndex = 2;
        else if (isHand) weaponIndex = 3;

        if (weaponIndex == -1 || weaponIndex >= weaponSprites.Length)
        {
            Debug.LogError("❌ 有効な武器が選択されていません！");
            return;
        }

        if (weaponImage != null)
        {
            weaponImage.sprite = weaponSprites[weaponIndex]; // スプライトを変更
        }
        else
        {
            Debug.LogError("❌ WeaponImage の参照が設定されていません！");
        }
    }
}
