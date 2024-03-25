using System;
using Fight.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Fight.EventHandler
{
    public enum Direction: int
    {
        Top, Center, Bottom
    }
    public struct GameEvent
    {
        public bool IsSelected;
        public int Hp;
        public Direction AttackIndex;//1-top, 2-centre, 3-botton
        public Direction ProtectIndex;
        public bool IsAttackChance;
        public bool IsProtectChance;

        public GameEvent(int hp = 5)
        {
            IsSelected = false;
            this.Hp = hp;
            AttackIndex = Direction.Top;
            ProtectIndex = Direction.Top;
            IsAttackChance = false;
            IsProtectChance = false;
        }
    }
    
    [Serializable]
    public class PlayerUI
    {
        public GameEvent gameEvent;
        public GameObject person;
        public Slider hp;
        public Text hpText;
        public int points = 0;
    }
}