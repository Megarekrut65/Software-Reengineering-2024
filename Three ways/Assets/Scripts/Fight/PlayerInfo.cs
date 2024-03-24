﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class PlayerInfo
{
    public string nickName;
    private string password;
    public string eMail;
    public int coins;
    public int points;
    public int currentIndexOfAvatar;
    private List<Weapons> dataOfPurchasedAvatars;
    public bool correctRead;

    public Weapons GetCurrentWeapon()
    {
        return GetWeapon(currentIndexOfAvatar);
    }
    public Weapons GetWeapon(int indexOfAvatar)
    {
        foreach (Weapons weapon in dataOfPurchasedAvatars)
        {
            if(weapon.indexOfAvatar == indexOfAvatar)
                return weapon;
        }

        return new Weapons(indexOfAvatar);
    }
    public void AddResult(GameResult result)
    {
        points += result.newPoints;
        coins += result.coins;
    }
    public bool BuyWeapon(int price, int indexOfAvatar, int indexOfSteel)
    {
        if(price < coins)
        {
            coins -= price;
            foreach (Weapons weapon in dataOfPurchasedAvatars)
            {
                if(weapon.indexOfAvatar == indexOfAvatar)
                    weapon.AddLvl(indexOfSteel);
            }
            
            return true;
        }
        return false;
    }
    public bool BuyAvatar(int price, int index)
    {
        if(price < coins)
        {
            coins -= price;
            dataOfPurchasedAvatars.Add(new Weapons(index, 1, 1));
            return true;
        }
        return false;
    }
    public PlayerInfo()
    {
        nickName = "Player1";
        password = "";
        eMail = "";
        coins = 0;
        points = 0;
        currentIndexOfAvatar = 0;
        dataOfPurchasedAvatars = new List<Weapons>();
        correctRead = false;
    }
    public PlayerInfo(string nickName, string password, string eMail)
    {
        this.nickName = nickName;
        this.password = password;
        this.eMail = eMail;
        coins = 55000;
        points = 100;
        currentIndexOfAvatar = 0;
        dataOfPurchasedAvatars = new List<Weapons>();
        dataOfPurchasedAvatars.Add(new Weapons(currentIndexOfAvatar, 1, 1));
        correctRead = false;
    }
    public PlayerInfo(string nickName, string password, string eMail,
    string coins, string points, string avatar, string purchasedAvatars)
    {
        this.nickName = nickName.Substring(9);
        this.password = password.Substring(9);
        this.eMail = eMail.Substring(6);
        this.points = Convert.ToInt32(points.Substring(7));
        this.coins = Convert.ToInt32(coins.Substring(6));
        this.currentIndexOfAvatar = Convert.ToInt32(avatar.Substring(7));
        StringToList(purchasedAvatars);
    }
    public PlayerInfo(string infoPath)
    {
        ReadPlayerFromFile(infoPath);
    }
    public bool WasBought(int index)
    {
        foreach (Weapons weapon in dataOfPurchasedAvatars)
        {
            if(weapon.indexOfAvatar == index) return true;
        }

        return false;
    }
    string ListToString()
    {
        string result = "PurchasedAvatars=";
        foreach (Weapons weapon in dataOfPurchasedAvatars)
        {
            result += weapon.CreateString() + ",";
        }
        return result;
    }
    bool StringToList(string line)
    {
        dataOfPurchasedAvatars = new List<Weapons>();
        string[] parts = line.Split("=".ToCharArray());
        if(parts.Length != 2) return false;
        string[] avatars = parts[1].Split(",".ToCharArray());
        if(avatars.Length <= 1) return false;
        for(int i = 0; i < avatars.Length - 1; i++)
        {
            if(avatars[i].Length > 10) 
                    dataOfPurchasedAvatars.Add(new Weapons(avatars[i]));
            else return false;
        }
        return true;
    }
    void WritePlayerToFile(ref StreamWriter writer)
    {
        writer.WriteLine("NickName=" + nickName);
        writer.WriteLine("Password=" + password);
        writer.WriteLine("EMail=" + eMail);
        writer.WriteLine("Coins=" + coins.ToString());
        writer.WriteLine("Points=" + points.ToString());
        writer.WriteLine("Avatar=" + currentIndexOfAvatar.ToString());
        writer.WriteLine(ListToString());
    }
    public void CreateInfoFile(string path)
    {
        RefreshFile(path);
        FileStream file = new FileStream(path, FileMode.OpenOrCreate);
        StreamWriter writer = new StreamWriter(file);
        WritePlayerToFile(ref writer);
        writer.Close();
    }
    public void AppendToPlayersFile(string path)
    {
        FileStream file = new FileStream(path, FileMode.Append);
        StreamWriter writer = new StreamWriter(file);
        WritePlayerToFile(ref writer);
        writer.Close();
    }
    public void RefreshFile(string path)
    {
        File.Delete(path);
    }
    void ReadPlayer(ref StreamReader reader)
    {
        if (reader.EndOfStream) return;
        nickName = reader.ReadLine().Substring(9);
        if (reader.EndOfStream) return;
        password = reader.ReadLine().Substring(9);
        if (reader.EndOfStream) return;
        eMail = reader.ReadLine().Substring(6);
        if (reader.EndOfStream) return;
        coins = Convert.ToInt32(reader.ReadLine().Substring(6));
        if (reader.EndOfStream) return;
        points = Convert.ToInt32(reader.ReadLine().Substring(7));
        if (reader.EndOfStream) return;
        currentIndexOfAvatar = Convert.ToInt32(reader.ReadLine().Substring(7));
        if (reader.EndOfStream) return;
        if(!StringToList(reader.ReadLine())) return;
        correctRead = true;
    }
    void ReadPlayerFromFile(string path)
    {
        correctRead = false;
        FileStream file = new FileStream(path, FileMode.OpenOrCreate);
        StreamReader reader = new StreamReader(file);
        ReadPlayer(ref reader);
        reader.Close();
    }
    public void EditPlayerInPlayersFile(string path, string newPath, string infoPath)
    {
        CreateInfoFile(infoPath);
        FileStream newFile = new FileStream(newPath, FileMode.OpenOrCreate);
        newFile.Close();
        FileStream file = new FileStream(path, FileMode.OpenOrCreate);
        StreamReader reader = new StreamReader(file);
        while (!reader.EndOfStream)
        {
            string fileNickName = reader.ReadLine();
            if ((fileNickName.Length <= 9) 
            || (fileNickName.Substring(0, 9) != "NickName="))
            {
                continue;
            }
            if (fileNickName.Substring(9) == nickName)
            {
                AppendToPlayersFile(newPath);
            }
            else
            {
                PlayerInfo newPlayer = new PlayerInfo(
                    fileNickName, 
                    reader.ReadLine(), 
                    reader.ReadLine(), 
                    reader.ReadLine(), 
                    reader.ReadLine(), 
                    reader.ReadLine(), 
                    reader.ReadLine());
                newPlayer.AppendToPlayersFile(newPath);
            }
        }
        reader.Close();
        File.Delete(path);
        File.Move(newPath, path);
    }
    public void DeletePlayer(string path, string newPath, string infoPath)
    {
        File.Delete(infoPath);
        FileStream newFile = new FileStream(newPath, FileMode.OpenOrCreate);
        newFile.Close();
        FileStream file = new FileStream(path, FileMode.OpenOrCreate);
        StreamReader reader = new StreamReader(file);
        while (!reader.EndOfStream)
        {
            string fileNickName = reader.ReadLine();
            if ((fileNickName.Length <= 9) || (fileNickName.Substring(0, 9) != "NickName="))
            {
                continue;
            }
            if (fileNickName.Substring(9) == nickName)
            {
                continue;
            }
            else
            {
                PlayerInfo newPlayer = new PlayerInfo(
                    fileNickName, 
                    reader.ReadLine(), 
                    reader.ReadLine(), 
                    reader.ReadLine(), 
                    reader.ReadLine(), 
                    reader.ReadLine(), 
                    reader.ReadLine());
                newPlayer.AppendToPlayersFile(newPath);
            }
        }
        reader.Close();
        File.Delete(path);
        File.Move(newPath, path);
    }
    public bool CorrectPassword(string password)
    {
        return (this.password == password);
    }
    public bool EditPassword(string oldPassword, string newPassword)
    {
        if(CorrectPassword(oldPassword)) 
        {
            password = newPassword;
            return true;
        }
        
        return false;
    }
}
