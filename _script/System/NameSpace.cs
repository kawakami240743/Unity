using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    public class Player
    {
        public string name;
        public int hp;
        public int atk;
        public float spd;
    }

    public class Enemies_01
    {
        public string name;
        public int hp;
        public int atk;
        public int spd;
    }

    public class Enemies_02
    {
        public string name;
        public int hp;
        public int atk;
        public int spd;
    }

    public class BossEnemyInfo
    {
        public string name;
        public int hp;
        public int atk;
        public int spd;
    }

    public class ItemTypes
    {
        public int HelpItem;
        public int AttackItem;
    }

    public static class HelpItems
    {
        public const int Heal = 1;
        public const int Invincible = 2;
        public const int Auto_shot = 3;
    }

    public static class AttackItems
    {
        public const int Bomb = 4;
        public const int Tornado = 5;
        public const int Thunder = 6;
    }
}
