using Photon.Pun;
using UnityEngine;

namespace Fight.WaitRoom
{
    public class SyncPlayersInfo : MonoBehaviour, IPunObservable
    {
        [SerializeField]
        private GameInfo _gameInfo;
        private GameObject _board;
        private GameObject _mainCamera;
        private PhotonView _photonView;

        private void SetMine()
        {
            _board = GameObject.Find("LeftBoard");
            string json = PlayerPrefs.GetString("game-info", "{}");
            _gameInfo = JsonUtility.FromJson<GameInfo>(json);
        }

        private void SetOther()
        {
            _board = GameObject.Find("RightBoard");
        }

        private void Start()
        {
            _mainCamera = GameObject.Find("Main Camera");
            _photonView = GetComponent<PhotonView>();
            _photonView.Synchronization = ViewSynchronization.ReliableDeltaCompressed;
            if(_photonView.IsMine) SetMine();
            else SetOther();
        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(stream.IsWriting)
            {
                stream.SendNext(_gameInfo);
            }
            else
            {
                _gameInfo = (GameInfo)stream.ReceiveNext();
                Debug.Log("Receive");
                Debug.Log(_gameInfo.isReady);
            }
        }

        private void Update()
        {
            if(_gameInfo.isHost) _mainCamera.GetComponent<EventHandler.EventHandler>().maxHp = _gameInfo.maxHP; 
            if(_gameInfo.isHost) _board.GetComponent<InfoBoard>().SetRoom(_gameInfo.code);
            _board.GetComponent<InfoBoard>().SetData(_photonView.Owner.NickName, _gameInfo);
            if(_photonView.IsMine)
            {
                _gameInfo.isReady = _board.GetComponent<InfoBoard>().info.isReady;
                _mainCamera.GetComponent<EventHandler.EventHandler>().right.points = _gameInfo.points;
            }
            else
            {
                _board.GetComponent<InfoBoard>().info.isReady = _gameInfo.isReady;
                _mainCamera.GetComponent<EventHandler.EventHandler>().left.points = _gameInfo.points;
            }
            
        }
    }
}
