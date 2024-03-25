using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fight.Player
{
    public static class PlayerStorage
    {
        private const string CurrentKey = "player", PlayersKey = "players";


        public static void SaveCurrentPlayer(PlayerData data)
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(CurrentKey, json);
        }

        public static void AddNewPlayer(PlayerData data)
        {
            string json = PlayerPrefs.GetString(PlayersKey, "[]");
            PlayerData[] players = JsonUtility.FromJson<PlayerData[]>(json);

            List<PlayerData> list = new List<PlayerData>(players) { data };
            players = list.ToArray();

            json = JsonUtility.ToJson(players);
            PlayerPrefs.SetString(PlayersKey, json);
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
            string json = PlayerPrefs.GetString(PlayersKey, "[]");
            PlayerData[] players = JsonUtility.FromJson<PlayerData[]>(json);
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].id == data.id)
                {
                    players[i] = data;
                    break;
                }
            }

            json = JsonUtility.ToJson(players);
            PlayerPrefs.SetString(PlayersKey, json);
            
            PlayerPrefs.SetString(CurrentKey, JsonUtility.ToJson(data));
        }

        public static void DeletePlayer(PlayerData data)
        {
            string json = PlayerPrefs.GetString(PlayersKey, "[]");
            PlayerData[] players = JsonUtility.FromJson<PlayerData[]>(json);
            List<PlayerData> list = new List<PlayerData>(players);
            foreach (var player in list.Where(player => player.id == data.id))
            {
                list.Remove(player);
                break;
            }

            players = list.ToArray();
            json = JsonUtility.ToJson(players);
            PlayerPrefs.SetString(PlayersKey, json);
        }
    }
}