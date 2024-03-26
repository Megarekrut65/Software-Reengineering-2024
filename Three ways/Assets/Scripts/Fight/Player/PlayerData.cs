using System;
using Select;
using UnityEngine.Serialization;

namespace Fight.Player
{
    [Serializable]
    public class PlayerData
    {
        public string id;
        public string nickname;
        public string password;
        public string eMail;
        public int coins;
        public int points;
        public WeaponsData[] weapons;
    }

    [Serializable]
    public class Wrapper
    {
        public PlayerData[] players;
    }
}