using System;
using Fight;
using Fight.Player;

namespace Select
{
    [Serializable]
    public class WeaponsData
    {
        public int indexOfAvatar = 0;
        public int lvlOfSword = 1;
        public int lvlOfShield = 1;
    }

    public class Weapons
    {
        private const int MaxLvl = 15;
        public WeaponsData Data { get; private set; }
        public Weapons(WeaponsData data)
        {
            Data = data;
        }
       private int CountShieldChance()
        {
            int add = Data.indexOfAvatar switch
            {
                0 => 12 - 5 / Data.lvlOfShield,
                1 => 4,
                2 => 8 - 4 / Data.lvlOfShield,
                _ => 0
            };

            return Data.lvlOfShield * 2 + add;
        }
        private int CountSwordChance()
        {
            int add = Data.indexOfAvatar switch
            {
                0 => 16 - 10 / Data.lvlOfSword,
                1 => 9 - 5 / Data.lvlOfSword,
                2 => 0,
                _ => 0
            };
            return Data.lvlOfSword + add + 5;
        }
        public int CountChance(Steel steel)
        {
            return steel switch
            {
                Steel.Sword => CountSwordChance(),
                Steel.Shield => CountShieldChance(),
                _ => 0
            };
        }
        public int CountPrice(Steel steel)
        {
            int startPrice = 100;
            startPrice = steel switch
            {
                Steel.Sword => Data.lvlOfSword * startPrice + Data.indexOfAvatar * Data.lvlOfSword * 10,
                Steel.Shield => Data.lvlOfShield * startPrice + Data.indexOfAvatar * Data.lvlOfShield * 10,
                _ => startPrice
            };
            return startPrice + MaxLvl;
        }
        public bool IsMaxLvl(Steel steel)
        {
            switch(steel)
            {
                case Steel.Sword: if(Data.lvlOfSword == MaxLvl) return true;
                    break;
                case Steel.Shield: if(Data.lvlOfShield == MaxLvl) return true;
                    break;
            }
            return false;
        }
        public void AddLvl(Steel steel)
        {
            switch(steel)
            {
                case Steel.Sword: if(Data.lvlOfSword < MaxLvl) Data.lvlOfSword++;
                    break;
                case Steel.Shield: if(Data.lvlOfShield < MaxLvl) Data.lvlOfShield++;
                    break;
            }
        }
        public int GetLvl(Steel steel)
        {
            return steel switch
            {
                Steel.Sword => Data.lvlOfSword,
                Steel.Shield => Data.lvlOfShield,
                _ => 0
            };
        }
    }
}