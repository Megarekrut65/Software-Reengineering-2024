using System;
using System.Collections;
using Fight.Player;
using Fight.WaitRoom;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Lobby
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        public Text roomCodeText;
        public InputField maxHp;
        public InputField roomCode;
        private PlayerController _player;
        private int _numberOfRoom;
        private bool _isConnect;
        private bool _needConnect;
        public GameObject waiting;
        public Text waitingText;
        public GameObject errorsBoard;
        public GameObject creating;
        public GameObject joining;
        public GameObject lobbyMenu;
        public Toggle isPrivate;
        private bool _findRandom;

        private void Start()
        {       
            SetDisconnect();
            PlayerSetting();
            SettingPhoton();
            _findRandom = false;
            StartCoroutine(FindRoom());
        }

        private void PlayerSetting()
        {
            _player = new PlayerController(PlayerStorage.GetCurrentPlayer());
        }
        public void SetDisconnect()
        {
            PhotonNetwork.Disconnect();
            _isConnect = false;
            _needConnect = true;
        }

        private void SettingPhoton()
        {
            PhotonNetwork.NickName = _player.Data.nickname;
            //PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = "2";
        }
        public override void OnLeftRoom()
        {
            Debug.Log("Left Room");
        }
        public override void OnConnectedToMaster()
        {
            _isConnect = true;
            _numberOfRoom = UnityEngine.Random.Range(1000,9999);
            roomCodeText.text = "Room code: " + _numberOfRoom.ToString();
            SetWaiting("", false);
            Debug.Log("Connected to Master");   
        }

        private bool CreateGameInfo(int code, bool isHost)
        {
            int maxHpNumber = Convert.ToInt32(maxHp.text);
            if(maxHpNumber < 1)
            {
                errorsBoard.SetActive(true);
                errorsBoard.GetComponent<Errors>().SetError("Error" + ": Max hp can't be less than one");
                return false;
            }
            GameInfo gameInfo = new GameInfo(_player.CurrentIndexOfAvatar, _player.Data.points, code, maxHpNumber, isHost);
            PlayerPrefs.SetString("game-info", JsonUtility.ToJson(gameInfo));

            return true;
        }
        public void CreateRoom()
        {
            if(!_isConnect) return;
            SetWaiting("Creating...", true);
            if(!CreateGameInfo(_numberOfRoom, true)) return;
            Debug.Log("Creating...");  
            PhotonNetwork.CreateRoom(_numberOfRoom.ToString(), new Photon.Realtime.RoomOptions{MaxPlayers = 2, IsVisible = !isPrivate.isOn});
        }

        private IEnumerator CloseWait()
        {
            yield return new WaitForSeconds(2f);
            waiting.SetActive(false);
            StopCoroutine("CloseWait");
        }

        private void SetWaiting(string text, bool active)
        {
            if(active)
            {
                waiting.SetActive(active);
                waitingText.text = text;
            }
            else
            {
                StartCoroutine("CloseWait");
            }
        }

        private IEnumerator FindRoom()
        {
            while(true)
            {
                if(_isConnect&&_findRandom) 
                {
                    PhotonNetwork.JoinRandomRoom();    
                }  
                yield return new WaitForSeconds(10f);       
            }
        }
        public void OnRandomRoom()
        {
            if(!_isConnect) return;
            SetWaiting("Finding random room...", true);
            Debug.Log("Finding...");
            maxHp.text = "20";
            CreateGameInfo(1111, false);
            _findRandom = true;   
        }
        public override void OnCreatedRoom()
        {
            SetWaiting("", false);
            Debug.Log("Created!");
        }
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            errorsBoard.SetActive(true);
            errorsBoard.GetComponent<Errors>().SetError("Error" + ": " + message);
            Debug.Log("Error" + returnCode.ToString() + ": " + message);  
        }
        public void JoinToRoom()
        {
            if(!_isConnect) return;
            SetWaiting("Joining...", true);
            Debug.Log("Joining...");
            if(roomCode.text.Length == 0) OnJoinRoomFailed(32758, " Room code is too short");
            else 
            {
                maxHp.text = "20";
                CreateGameInfo(Convert.ToInt32(roomCode.text), false);
                PhotonNetwork.JoinRoom(roomCode.text);
            }
        }
        public override void OnJoinedRoom()
        {
            _findRandom = false;
            SetWaiting("", false);
            Debug.Log("Joined the room");
            PhotonNetwork.LoadLevel("Fight"); 
        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            errorsBoard.SetActive(true);
            errorsBoard.GetComponent<Errors>().SetError("Error" + ": " + message);
            Debug.Log("Error"+returnCode.ToString() + ": " + message);  
        }
        public override void OnLeftLobby()
        {
            Debug.Log("Lobby");
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            _isConnect = false;
            _needConnect = true;
            Debug.Log("Disconnected from Master: " + cause.ToString());
            if(roomCodeText != null) roomCodeText.text = "Room code: XXXX";
        }

        private void Update()
        {
            if(_needConnect && !_isConnect) 
            {
                SetWaiting("Connecting...", true);
                _needConnect = false;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        public void Creating()
        {
            lobbyMenu.SetActive(false);
            creating.SetActive(true);
            maxHp.text = "20";
        }
        public void Joining()
        {
            lobbyMenu.SetActive(false);
            joining.SetActive(true);
        }
        public void ExitFromLobby()
        {
            PhotonNetwork.Disconnect();
        }
    }
}
