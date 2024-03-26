using Fight.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Account
{
    public class Exit : MonoBehaviour
    {
        public void ExitButton()
        {
            PlayerStorage.ClearCurrent();
        
            SceneManager.LoadScene("LogIn", LoadSceneMode.Single);
        }
    }
}
