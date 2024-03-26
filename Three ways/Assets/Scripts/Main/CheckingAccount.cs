using Fight.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main
{
    public class CheckingAccount : MonoBehaviour
    {
        void Start()
        {
            PlayerData player = PlayerStorage.GetCurrentPlayer();
            if(player == null) SceneManager.LoadScene("LogIn", LoadSceneMode.Single);
        }
    }
}
