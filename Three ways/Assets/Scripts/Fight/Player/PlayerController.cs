using System.Collections.Generic;
using Select;
using UnityEngine;

namespace Fight.Player
{
    
    public class PlayerController
    {
        public PlayerData Data { get; }
        private readonly Dictionary<int, Weapons> _weapons;

        public int CurrentIndexOfAvatar { get; }
        
        public PlayerController(PlayerData data)
        {
            CurrentIndexOfAvatar = PlayerPrefs.GetInt("CurrentAvatar", 0);
            Data = data;
            _weapons = new Dictionary<int, Weapons>();
            foreach (WeaponsData weaponsData in Data.weapons)
            {
                _weapons[weaponsData.indexOfAvatar] = new Weapons(weaponsData);
            }
        }

        public Weapons GetCurrentWeapon()
        {
            return GetWeapon(CurrentIndexOfAvatar);
        }
        public Weapons GetWeapon(int indexOfAvatar)
        {
            return _weapons.TryGetValue(indexOfAvatar, out Weapons weapons) 
                ? weapons 
                : new Weapons(new WeaponsData{indexOfAvatar = indexOfAvatar});
        }
        public void AddResult(GameResult result)
        {
            Data.points += result.newPoints;
            Data.coins += result.coins;
        }
        public bool UpgradeWeapon(int price, int indexOfAvatar, Steel steel)
        {
            if (price >= Data.coins) return false;
            Data.coins -= price;

            if (!_weapons.TryGetValue(indexOfAvatar, out Weapons weapons)) return false;
            weapons.AddLvl(steel);
            
            return true;
        }
        public bool BuyAvatar(int price, int index)
        {
            if (price >= Data.coins) return false;
        
            Data.coins -= price;
            _weapons[index] = new Weapons(new WeaponsData{indexOfAvatar = index});
            return true;
        }
        public bool WasBought(int index)
        {
            return _weapons.ContainsKey(index);
        }
        public bool CorrectPassword(string password)
        {
            return Data.password == password;
        }
        public bool EditPassword(string oldPassword, string newPassword)
        {
            if (!CorrectPassword(oldPassword)) return false;
        
            Data.password = newPassword;
            return true;
        }
    }
}
