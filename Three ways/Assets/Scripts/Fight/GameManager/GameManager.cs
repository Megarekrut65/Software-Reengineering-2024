using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

namespace Fight.GameManager
{
    public class GameManager: MonoBehaviourPunCallbacks
    {
        public GameObject[] playerPrefabs;
        private GameObject _mainCamera;
        public GameObject leftBoard;
        public GameObject rightBoard;
        public GameObject waitRoom;
        public GameObject startingGame;
        public GameObject gameRoom;
        public GameObject playerInfo;
        private readonly Vector3 _pos = Vector3.zero;  
        private bool _isStarted;

        private void SetPlayer()
        {
            int index = leftBoard.GetComponent<InfoBoard>().info.indexOfAvatar;
            string playerName = playerPrefabs[index].name;
            PhotonNetwork.Instantiate(playerName, _pos, Quaternion.identity);
        }

        private void RegisterMyTypes()
        {
            PhotonPeer.RegisterType(typeof(GameEvent), 100, GameSerializer.SerializeGameEvent, 
                GameSerializer.DeserializeGameEvent);
            PhotonPeer.RegisterType(typeof(GameInfo), 101, GameSerializer.SerializeGameInfo, 
                GameSerializer.DeserializeGameInfo);
        }

        private void Start()
        {
            _isStarted = false;
            RegisterMyTypes();
            PhotonNetwork.Instantiate(playerInfo.name, _pos, Quaternion.identity);
            _mainCamera = GameObject.Find("Main Camera");
        }

        private void Update()
        {
            if(!_isStarted&&leftBoard.GetComponent<InfoBoard>().info.isReady &&
               rightBoard.GetComponent<InfoBoard>().info.isReady)
            {
                _isStarted = true;
                startingGame.SetActive(true);
                waitRoom.SetActive(false);
                startingGame.GetComponent<StartingGame>().Game();
            }
        }
        public void StartGame()
        {
            gameRoom.GetComponent<Animator>().SetBool("move", true);
            SetPlayer();
            _mainCamera.GetComponent<EventHandler>().Begin();
        }
        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            Debug.LogFormat("Player {0} entered room", newPlayer.NickName);
        }
    }
}