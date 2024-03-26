using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fight.Player
{
    public static class PlayerStorage
    {
        private const string CurrentKey = "player", PlayersKey = "players";

        private static PlayerData[] Players {
            get {
                string json = PlayerPrefs.GetString(PlayersKey, "");
                Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);
                return wrapper?.players ?? Array.Empty<PlayerData>();
            }
            set
            {
                string json = JsonUtility.ToJson(new Wrapper{players = value});
                PlayerPrefs.SetString(PlayersKey, json);
            }
        }

        public static void SaveCurrentPlayer(PlayerData data)
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(CurrentKey, json);
        }

        public static void AddNewPlayer(PlayerData data)
        {
            PlayerData[] players = Players;

            List<PlayerData> list = new List<PlayerData>(players) { data };
            players = list.ToArray();

            Players = players;
        }

        public static void ClearCurrent()
        {
            PlayerPrefs.DeleteKey(CurrentKey);
        }

        public static PlayerData GetCurrentPlayer()
        {
            string json = PlayerPrefs.GetString(CurrentKey, "");
            PlayerData info = JsonUtility.FromJson<PlayerData>(json);

            return info;
        }

        public static void UpdateCurrentPlayer(PlayerData data)
        {
            PlayerData[] players = Players;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].id == data.id)
                {
                    players[i] = data;
                    break;
                }
            }

            Players = players;
            
            PlayerPrefs.SetString(CurrentKey, JsonUtility.ToJson(data));
        }

        public static void DeletePlayer(PlayerData data)
        {
            List<PlayerData> list = new List<PlayerData>(Players);
            foreach (var player in list.Where(player => player.id == data.id))
            {
                list.Remove(player);
                break;
            }

            Players = list.ToArray();
        }

        public static PlayerData LoginPlayer(string nickname, string password)
        {
            PlayerData[] players = Players;
            return players.FirstOrDefault(player => player.nickname == nickname || player.password == password);
        }

        public static bool ExistsPlayer(string nickname)
        {
            PlayerData[] players = Players;
            return players.Any(player => player.nickname == nickname);
        }
    }
}