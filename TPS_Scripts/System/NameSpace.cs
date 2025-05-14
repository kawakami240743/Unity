using UnityEngine;

namespace Game
{
    // ステータスを管理するクラス
    public class PlayerStatus
    {
        // 基本ステータス
        public float hp { get; set; }
        public int maxhp { get; set; }
        public int atk { get; set; }

        // コンストラクタ
        public PlayerStatus(float health,int max, int attack)
        {
            hp = health;
            maxhp = max;
            atk = attack;
        }
    }

    [System.Serializable]
    public class EnemyStatus
    {
        // 基本ステータス
        public int hp { get; set; }
        public int atk { get; set; }
        public int move { get; set; }
        public int rtn { get; set; }

        // コンストラクタ
        public EnemyStatus(int health, int attack, int moveSpeed, int rotationSpeed)
        {
            hp = health;
            atk = attack;
            move = moveSpeed;
            rtn = rotationSpeed;
        }
    }
}
