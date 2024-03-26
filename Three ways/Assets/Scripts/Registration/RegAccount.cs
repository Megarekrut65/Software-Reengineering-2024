using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using Fight.Player;
using Select;
using UnityEngine.SceneManagement;

public class RegAccount : MonoBehaviour
{
    public InputField inputNickName;
    public InputField inputEMail;
    public InputField inputPassword;
    public InputField inputPasswordAgain;
    public GameObject errors;
    public GameObject wait;

    private bool CheckAlreadyCreatedAccount(string nickname)
    {
        if (!PlayerStorage.ExistsPlayer(nickname)) return false;
        
        IncorrectData("Player","with entered nickname already exists!");
        return true;
    }
    
    private void IncorrectData(string obj, string message)
    {
        errors.SetActive(true);
        errors.GetComponent<RegErrors>().SetError(obj + " " + message);
    }

    private bool EmptyInput(string obj, ref InputField input)
    {
        if (input.text.Length > 3) return false;
        IncorrectData(obj, "The number of characters entered must be greater than three.");
        return true;
    }

    private bool EmptyFields()
    {
        return EmptyInput("Nickname:", ref inputNickName)||
        EmptyInput("Email:",ref inputEMail)|| 
        EmptyInput("Password:",ref inputPassword);
    }
    
    public void Create()
    {      
        if(EmptyFields()) return;  
        string nickname = inputNickName.text;
        string eMail = inputEMail.text;
        string password = inputPassword.text;         
        string passwordAgain = inputPasswordAgain.text;         
        if (password != passwordAgain)
        {
            IncorrectData("Passwords", "do not match!");
            return;
        }
        if(CheckAlreadyCreatedAccount(nickname)) return;

        PlayerData playerData = new PlayerData
        {
            nickname = nickname, eMail = eMail, coins = 10000, points = 100,
            id = Guid.NewGuid().ToString(), password = password, 
            weapons = new WeaponsData[] { new WeaponsData{indexOfAvatar = 0, lvlOfShield = 0, lvlOfSword = 0}}
        };
        PlayerStorage.SaveCurrentPlayer(playerData);
        SceneManager.LoadScene("Main", LoadSceneMode.Single);        
    }
}
