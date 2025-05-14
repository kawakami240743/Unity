using UnityEngine;
using MyGame.Grenades;
using NUnit.Framework.Internal;

public class GrenadePickup : MonoBehaviour
{
    private bool isPickedUp = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (isPickedUp) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            GrenadeHolder grenadeHolder = collision.gameObject.GetComponentInParent<GrenadeHolder>();

            if (grenadeHolder != null)
            {
                string randomGrenade = GetRandomGrenade();

                int beforeQuantity = grenadeHolder.GetGrenades()[randomGrenade].Quantity; // **取得前の個数を記録**
                grenadeHolder.AddGrenade(randomGrenade);
                int afterQuantity = grenadeHolder.GetGrenades()[randomGrenade].Quantity; // **取得後の個数を記録**

                if (beforeQuantity == afterQuantity)
                {
                    Debug.Log($"⚠️ {collision.gameObject.name} は {randomGrenade} をこれ以上持てません！");
                    return; // **🔹 取得できなかったら削除しない**
                }

                Debug.Log($"{collision.gameObject.name} がグレネードを獲得: {randomGrenade}");

                isPickedUp = true;
                Destroy(gameObject);
            }
        }
    }
    private string GetRandomGrenade()
    {
        return (Random.Range(0, 2) == 0) ? "stone" : "grenade";
    }
}
