using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fight.GameManager
{
    public class LeaveManager: MonoBehaviourPunCallbacks
    {
        private GameObject _mainCamera;
        public GameObject rightBoard;
        public GameObject question;
        private bool _isStarted;
        private bool _endFight = false;
        public GameObject gameCanvas;
        
        public void AnswerYes()
        {
            PhotonNetwork.LeaveRoom();
        }
        public void AnswerNo()
        {
            question.SetActive(false);
            gameCanvas.GetComponent<Canvas>().sortingOrder = 0;
        }
    
        public void Leave()
        {
            if(_isStarted) 
            {
                question.SetActive(true);
                gameCanvas.GetComponent<Canvas>().sortingOrder = 30;
            }
            else 
            {
                PhotonNetwork.LeaveRoom();
                SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
            }
        }
        public override void OnLeftRoom()
        {
            if(_endFight) 
            {
                SceneManager.LoadScene("EndFight", LoadSceneMode.Single);
                return;
            }
            if(_isStarted) _mainCamera.GetComponent<EventHandler>().ForcedExit(true);
        }

        private void EditBoard()
        {
            rightBoard.GetComponent<InfoBoard>().PlayerLeave();
        }
        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            if(_endFight) return;
            if(_isStarted) _mainCamera.GetComponent<EventHandler>().ForcedExit(false);
            else EditBoard();
            Debug.LogFormat("Player {0} left room", otherPlayer.NickName);
        }
        public void EndFight()
        {
            _endFight = true;
            PhotonNetwork.LeaveRoom();
        }
    }
}