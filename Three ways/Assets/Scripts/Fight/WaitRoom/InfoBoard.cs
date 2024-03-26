using System;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Fight.WaitRoom
{
    public class InfoBoard : MonoBehaviour
    {
        public GameObject waiting;
        public GameObject data;
        public Text nickNameText;
        public Text avatar;
        public Text points;
        public GameInfo info;
        public bool isSet;
        public Text roomCodeText;

        public void SetRoom(int roomCode)
        {   
            roomCodeText.text = "Room: " + roomCode;
        }

        private void Start()
        {
            info = new GameInfo();
            isSet = false;
        }

        private string GetAvatarName(int index)
        {
            return index switch
            {
                0 => "Neanderthal",
                1 => "Knight",
                2 => "Musketeer",
                _ => "null"
            };
        }
        public void SetData(string nickName, GameInfo info)
        {   
            nickNameText.text = nickName;
            points.text = info.points.ToString();
            avatar.text = GetAvatarName(info.indexOfAvatar);
            if(isSet) return;
            isSet = true;
            this.info = info;
            waiting.SetActive(false);
            data.SetActive(true);
        }
        public void PlayerLeave()
        {
            isSet = false;
            data.SetActive(false);
            waiting.SetActive(true);
        }
        public void Ready()
        {
            info.isReady = !info.isReady;
        }

        private void Update()
        {
            GetComponent<Image>().color = info.isReady ? new Color(0,255,0,255) :
                new Color(255,0,0,255);
        }
    }
    
    [Serializable]
    public class GameInfo
    {
        public int indexOfAvatar;
        public int points;
        public bool isReady;
        public int code;
        public int maxHP;
        public bool isHost;

        public GameInfo(int indexOfAvatar = 0, int points = 0, int code = 1000, int maxHP = 20, bool isHost = false)
        {
            this.indexOfAvatar = indexOfAvatar;
            this.points = points;
            this.code = code;
            this.maxHP = maxHP;
            this.isHost = isHost;
            isReady = false;
        }
    }
}