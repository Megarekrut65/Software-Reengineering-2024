using Photon.Pun;
using UnityEngine;

namespace Fight.WaitRoom
{
    public class SyncPlayersInfo : MonoBehaviour, IPunObservable
    {
        private GameInfo gameInfo;
        private GameObject board;
        private GameObject mainCamera;
        private PhotonView photonView;

        private void SetMine()
        {
            board = GameObject.Find("LeftBoard");
            string json = PlayerPrefs.GetString("game-info", "{}");
            gameInfo = JsonUtility.FromJson<GameInfo>(json);
        }

        private void SetOther()
        {
            board = GameObject.Find("RightBoard");
        }

        private void Start()
        {
            mainCamera = GameObject.Find("Main Camera");
            photonView = GetComponent<PhotonView>();
            if(photonView.IsMine) SetMine();
            else SetOther();
        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(stream.IsWriting)
            {
                stream.SendNext(gameInfo);
            }
            else
            {
                gameInfo = (GameInfo)stream.ReceiveNext();
            }
        }

        private void Update()
        {
            if(gameInfo.isHost) mainCamera.GetComponent<EventHandler.EventHandler>().maxHp = gameInfo.maxHP; 
            if(gameInfo.isHost) board.GetComponent<InfoBoard>().SetRoom(gameInfo.code);
            board.GetComponent<InfoBoard>().SetData(photonView.Owner.NickName, gameInfo);
            if(photonView.IsMine)
            {
                gameInfo.isReady = board.GetComponent<InfoBoard>().info.isReady;
                mainCamera.GetComponent<EventHandler.EventHandler>().right.points = gameInfo.points;
            }
            else
            {
                board.GetComponent<InfoBoard>().info.isReady = gameInfo.isReady;
                mainCamera.GetComponent<EventHandler.EventHandler>().left.points = gameInfo.points;
            }
        }
    }
}
