using System.Collections;
using System.Collections.Generic;
using Fight.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public void ExitButton()
    {
        PlayerData data = PlayerStorage.GetCurrentPlayer();
        if (data != null)
        {
            PlayerStorage.DeletePlayer(data);
        }
        
        SceneManager.LoadScene("LogIn", LoadSceneMode.Single);
    }
}
