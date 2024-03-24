﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EventHandler : MonoBehaviour
{
    public GameObject attackControler;
    public GameObject protectControler;
    public GameObject gameCanvas;
    public GameEvent left;
    public GameEvent right;
    private float waitForNext = 2.8f;
    public int maxHP = 5;
    private bool needWait;
    private bool wasFight;
    private bool isSeted;
    public GameObject leftPerson;
    public GameObject rightPerson;
    private Slider leftHP;
    private Slider rightHP;
    private Text leftHpText;
    private Text rightHpText;
    private string resultPath = "result-info.txt";
    private string infoPath = "player-info.txt";
    private PlayerInfo playerInfo;
    public int minePoints = 0;
    public int otherPoints = 0;
    public GameObject gameManager;

    IEnumerator ShowControlers()
    {
        CheckHealth();
        left.isSelected = false;
        yield return new WaitForSeconds(waitForNext); 
        left.isAttackChance = false;
        left.isProtectChance = false;
        yield return new WaitForSeconds(0.2f); 
        gameCanvas.GetComponent<Canvas>().sortingOrder = 20;
        attackControler.SetActive(true);
        attackControler.GetComponent<SelectedWay>().Refresh();
        needWait = false;
        StopCoroutine("ShowControlers");
    }
    void SetObjects()
    {
        left = new GameEvent(maxHP);
        right = new GameEvent(maxHP);  
        leftHP = GameObject.Find("LeftHP").GetComponent<Slider>();
        rightHP = GameObject.Find("RightHP").GetComponent<Slider>();
        leftHpText = GameObject.Find("LeftHPText").GetComponent<Text>();
        rightHpText = GameObject.Find("RightHPText").GetComponent<Text>();
    }
    void Start()
    {
        CorrectPathes.MakeCorrect(ref resultPath, ref infoPath);
        playerInfo = new PlayerInfo(infoPath);
        Weapons weapons = playerInfo.GetCurrentWeapon();
        attackControler.GetComponent<SelectedWay>().chance = weapons.CountChance(0);
        protectControler.GetComponent<SelectedWay>().chance = weapons.CountChance(1);
        isSeted = false;
    }
    public void Begin()
    {
        SetObjects();
        needWait = false;
        isSeted = true;
        StartCoroutine("ShowControlers");
    }
    public void NextPerson()
    {
        if(wasFight)
        {
            StartCoroutine("ShowControlers");
        } 
        else
        {
            wasFight = true;
            rightPerson.GetComponent<Person>().Hitting();
        }
    }
    void CheckFight()
    {
        if(!needWait&&left.isSelected && right.isSelected)
        {
            wasFight = false;
            needWait = true; 
            leftPerson.GetComponent<Person>().Hitting();
        }
    }
    void SetLeft()
    {
        left.attackIndex = attackControler.GetComponent<SelectedWay>().index;
        left.protectIndex = protectControler.GetComponent<SelectedWay>().index;
        left.isAttackChance = attackControler.GetComponent<SelectedWay>().isChance;
        left.isProtectChance = protectControler.GetComponent<SelectedWay>().isChance;
        left.isSelected = true;
    }
    void SettingFight()
    {
        attackControler.GetComponent<SelectedWay>().isSelected = false;
        protectControler.GetComponent<SelectedWay>().isSelected = false;
        SetLeft();
    }
    void CheckSelectings()
    {
        if(attackControler.GetComponent<SelectedWay>().isSelected &&
        protectControler.GetComponent<SelectedWay>().isSelected)
        {
            SettingFight();
        }
    }
    void Win()
    {
        Weapons weapons = playerInfo.GetCurrentWeapon();
        int lvl = (weapons.GetLvl(0) + weapons.GetLvl(1))/2;
        GameResult result = GameResult.CountWin(minePoints, otherPoints, lvl);
        result.WriteResult(resultPath);
    }
    void Lose()
    {
        Weapons weapons = playerInfo.GetCurrentWeapon();
        int lvl = (weapons.GetLvl(0) + weapons.GetLvl(1))/2;
        GameResult result = GameResult.CountLose(minePoints, otherPoints, lvl);
        result.WriteResult(resultPath);
    }
    void CheckWiner()
    {
        if(leftHP.value <= 0 && rightHP.value <= 0) 
        {
            Win();
            return;
        }
        if(leftHP.value <= 0) Lose();
        if(rightHP.value <= 0) Win();
    }
    public void ForcedExit(bool mine)
    {
        if(mine) Lose();
        else Win();
        if(mine) SceneManager.LoadScene("EndFight", LoadSceneMode.Single);
        else gameManager.GetComponent<GameManager>().EndFight();
    }
    IEnumerator FightEnd()
    {
        StopCoroutine("ShowControlers");
        yield return new WaitForSeconds(3f);
        CheckWiner();
        gameManager.GetComponent<GameManager>().EndFight();
        StopCoroutine("FightEnd");
    }
    void CheckHealth()
    {
        leftHP.value = left.hp;
        rightHP.value = right.hp;
        leftHpText.text = left.hp.ToString();
        rightHpText.text = right.hp.ToString();
        if(leftHP.value <= 0) leftPerson.GetComponent<Person>().DieAvatar();
        if(rightHP.value <= 0) rightPerson.GetComponent<Person>().DieAvatar();
        if(leftHP.value <= 0 || rightHP.value <= 0)
        {
            StartCoroutine("FightEnd");
        }
    }
    void Update()
    {
        if(!isSeted) return;
        CheckSelectings();
        CheckFight();
    }
}
public struct GameEvent
{
    public bool isSelected;
    public int hp;
    public int attackIndex;//1-top, 2-centre, 3-botton
    public int protectIndex;
    public bool isAttackChance;
    public bool isProtectChance;

    public GameEvent(int hp = 5)
    {
        isSelected = false;
        this.hp = hp;
        attackIndex = 0;
        protectIndex = 0;
        isAttackChance = false;
        isProtectChance = false;
    }
}