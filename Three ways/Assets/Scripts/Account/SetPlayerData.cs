using Fight.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Account
{
    public class SetPlayerData : MonoBehaviour
    {
        public Text nickName;
        public Text points;
        public Text coins;
        public PlayerController Player;
        public GameObject deleting;
        public GameObject editing;
        public GameObject account;
        
        private void Start()
        {
            PlayerData data = PlayerStorage.GetCurrentPlayer();
            if (data == null)
            {
                SceneManager.LoadScene("LogIn", LoadSceneMode.Single);
                return;
            }
            Player = new PlayerController(data);
            SetData();
        }
        private void SetData()
        {
            nickName.text = Player.Data.nickname;
            points.text = Player.Data.points.ToString();
            coins.text = "$" + Player.Data.coins;
        }
        public void BackButton()
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
        public void EditPlayer()
        {
            PlayerStorage.UpdateCurrentPlayer(Player.Data);
            account.SetActive(true);
        }
        public void DeleteAccount()
        {
            PlayerStorage.DeletePlayer(Player.Data);
            SceneManager.LoadScene("LogIn", LoadSceneMode.Single);
        }
        public void DeleteButton()
        {
            deleting.SetActive(true);
            account.SetActive(false);
        }
        public void EditButton()
        {
            editing.SetActive(true);
            account.SetActive(false);
        }
    }
}
