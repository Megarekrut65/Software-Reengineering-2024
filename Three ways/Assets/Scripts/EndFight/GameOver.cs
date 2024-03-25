using Fight.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EndFight
{
    public class GameOver : MonoBehaviour
    {
        public Text theText;
        private GameResult _result;
        private PlayerController _player;

        void Start()
        {
       
            _result = new GameResult();
            _result.ReadResult();
        
            theText.text = _result.GetString();
            _player = new PlayerController(PlayerStorage.GetCurrentPlayer());
            _player.AddResult(_result);
            PlayerStorage.UpdateCurrentPlayer(_player.Data);
        }
        public void ClickNext()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
