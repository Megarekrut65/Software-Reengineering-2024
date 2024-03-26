using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using Fight.Player;
using UnityEngine.SceneManagement;

public class LogInAccount : MonoBehaviour
{
    public InputField inputNickName;
    public InputField inputPassword;
    public string nameOfScene = "Main";
    public GameObject errors;
    public GameObject[] avatars;

    public void IncorrectData(string obj, string message)
    {
        inputNickName.text = "";
        inputPassword.text = "";
        foreach (GameObject avatar in avatars)
            avatar.GetComponent<AvatarsAnimations>().Die();

        errors.SetActive(true);
        errors.GetComponent<LogInErrors>().SetError(obj + " " + message);
    }
    public void StopDieAvatars()
    {
        foreach (GameObject avatar in avatars)
            avatar.GetComponent<AvatarsAnimations>().StopDie();
    }

    private void Start()
    {
        StartCoroutine(Hitting());
    }

    private IEnumerator Hitting()
    {
        while(true)
        {
            yield return new WaitForSeconds(6f); 
            int random = UnityEngine.Random.Range(1, 9);
            switch (random)
            {
                case <= 3:
                    avatars[0].GetComponent<AvatarsAnimations>().Hit();
                    break;
                case > 3 and <= 6:
                    avatars[1].GetComponent<AvatarsAnimations>().Hit();
                    break;
                case > 6:
                {
                    foreach (GameObject avatar in avatars)
                        avatar.GetComponent<AvatarsAnimations>().Hit();

                    break;
                }
            }
        }
    }
    
    public void OkButton()
    {
        string nickname = inputNickName.text;
        string password = inputPassword.text;
        PlayerData player = PlayerStorage.LoginPlayer(nickname, password);
        if(player != null)
        {            
            PlayerStorage.SaveCurrentPlayer(player);
            SceneManager.LoadScene(nameOfScene, LoadSceneMode.Single);        
            return;  
        }

        IncorrectData("Nickname or password", "are incorrect!");
    }
}

