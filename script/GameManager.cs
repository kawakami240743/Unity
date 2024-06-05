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
        public int spd;
    }

    public class Enemy
    {
        public int name;
        public int hp;
        public int atk;
        public int spd;
    }

    public class Items
    {
        public int HelpItems;
        public int AttackItems;
    }


    public static class HelpItems
    {

    }

    public static class AttackItems
    {

    }
}

public class GameManager : MonoBehaviour
{

}
