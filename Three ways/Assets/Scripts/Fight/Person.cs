﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Person : MonoBehaviour, IPunObservable
{
    private PhotonView photonView;
    private GameEvent gameEvent;
    private Text nickNameText;
    private Slider hpSlider;
    private Text hpText;
    private GameObject mainCamera;
    private Animator animator;
    public Vector2 startPostion;
    public Vector2 endPosition;
    public Vector2 minePostion;
    public Vector2 enemyPosition;
    public float step;
    private float progress;
    private bool isRun;
    private bool wasHit;
    public GameObject sword;
    public GameObject shield;
    public int index = 0;
    public bool isStuned = false;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(gameEvent);
        }
        else
        {
            gameEvent = (GameEvent)stream.ReceiveNext();
        }
    }
    void SetMinePlayer()
    {
        minePostion = new Vector3(-5.5f, -5f, 0f);
        enemyPosition = new Vector3(4f, -5f, 0f); 
        transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        nickNameText = GameObject.Find("LeftNickName").GetComponent<Text>();
        hpSlider = GameObject.Find("LeftHP").GetComponent<Slider>();
        hpText = GameObject.Find("LeftHPText").GetComponent<Text>();  
        mainCamera.GetComponent<EventHandler>().leftPerson = gameObject;
    }
    void SetOtherPlayer()
    {
        minePostion = new Vector3(5.5f, -5f, 0f); 
        enemyPosition = new Vector3(-4f, -5f, 0f);
        transform.localScale = new Vector3(-0.6f, 0.6f, 0.6f);
        nickNameText = GameObject.Find("RightNickName").GetComponent<Text>();
        hpSlider = GameObject.Find("RightHP").GetComponent<Slider>();
        hpText = GameObject.Find("RightHPText").GetComponent<Text>();
        mainCamera.GetComponent<EventHandler>().rightPerson = gameObject;
    }
    void SetSame()
    {
        transform.position = minePostion;
        hpSlider.maxValue = mainCamera.GetComponent<EventHandler>().maxHP;
        hpText.text = hpSlider.maxValue.ToString();
        nickNameText.text = photonView.Owner.NickName;   
    }
    void SetPlayer()
    {  
        if (photonView.IsMine) SetMinePlayer();
        else SetOtherPlayer();
        SetSame(); 
    }
    public void Hitting()
    {
        startPostion = minePostion;
        endPosition = enemyPosition;
        progress = 0;
        isRun = true;
        animator.SetBool("run", isRun );
        wasHit = false;
    }
    public void SetIdle()
    {
        animator.SetBool("block", false );
        animator.SetBool("block-skill", false );
        animator.SetBool("damage", false );
    }
    int AttackSpecialSkill(int indexOfEnemy)
    {
        switch (indexOfEnemy)
        {
            case 0: return -1;
            case 1: return -2;
            case 2: return -3;
            default: return -1;
        }
    }
    public void EditMineHP(int value)
    {
        mainCamera.GetComponent<EventHandler>().left.hp += value; 
        hpSlider.value = mainCamera.GetComponent<EventHandler>().left.hp;
        hpText.text = mainCamera.GetComponent<EventHandler>().left.hp.ToString();
    }
    public void EditHP(int value)
    {
        hpSlider.value = gameEvent.hp + value;
        hpText.text = (gameEvent.hp + value).ToString();
    }
    public void AttackSound()
    {
        sword.GetComponent<AudioSource>().Play();
    }
    public void ProtectSound()
    {
        shield.GetComponent<AudioSource>().Play();
    }
    void Attack(bool isChance, int indexOfEnemy)
    {
        int damage = -1;
        if(isChance) damage = AttackSpecialSkill(indexOfEnemy);
        animator.SetBool("damage", true );
        if(photonView.IsMine) EditMineHP(damage);
        else EditHP(damage);
        
    }
    public void DieAvatar()
    {
        animator.SetBool("die", true );
    }
    public void SetStun(bool stun)
    {
        isStuned = stun;
    }
    void ProtectSpecialSkill()
    {
        if(isStuned) 
        {
            SetStun(false);
            return;
        }
        switch (index)
        {
            case 0:
            {
                if (photonView.IsMine) 
                mainCamera.GetComponent<EventHandler>().rightPerson.GetComponent<Person>().GetHit(4, false, index);
                else  mainCamera.GetComponent<EventHandler>().leftPerson.GetComponent<Person>().GetHit(4, false, index);
            }
                break;
            case 1:
            {
                if (photonView.IsMine) 
                mainCamera.GetComponent<EventHandler>().rightPerson.GetComponent<Person>().SetStun(true);
                else  mainCamera.GetComponent<EventHandler>().leftPerson.GetComponent<Person>().SetStun(true);
            }
                break;
            case 2:
            {
                if(photonView.IsMine) EditMineHP(1);
                else EditHP(1);
            }
                break;
            default:
                break;
        }
    }
    void Protect(bool isChance, int indexOfEnemy)
    {
        animator.SetBool("block", true );
        if(isChance && indexOfEnemy == 0) Attack(false, 0);
        else if(gameEvent.isProtectChance) ProtectSpecialSkill();
    }
    public void GetHit(int enemyAttack, bool isChance, int indexOfEnemy)
    {
        if(enemyAttack != gameEvent.protectIndex) Attack(isChance, indexOfEnemy);  
        else Protect(isChance, indexOfEnemy);
    }
    public void Fight()
    {
        AttackSound();
        bool chance = gameEvent.isAttackChance;
        if(isStuned&&gameEvent.isAttackChance)
        {
            isStuned = false;
            chance  = false;
        }
        if(photonView.IsMine) 
        mainCamera.GetComponent<EventHandler>().rightPerson.GetComponent<Person>().GetHit(gameEvent.attackIndex, chance, index);
        else  mainCamera.GetComponent<EventHandler>().leftPerson.GetComponent<Person>().GetHit(gameEvent.attackIndex, chance, index);
    }
    public void StopHit()
    {
        animator.SetBool("run", false );
        animator.SetInteger("hit", 0);
        wasHit = true;
        startPostion = enemyPosition;
        endPosition = minePostion;
        progress = 0;
        isRun = true;
    }
    void Start()
    {
        gameEvent = new GameEvent();
        isRun = false;
        animator = GetComponent<Animator>();
        photonView = GetComponent<PhotonView>();
        mainCamera = GameObject.Find("Main Camera");
        SetPlayer();
        StartCoroutine("EventExchange");
    }
    void FixedUpdate()
    {
        if(!isRun) return;
        transform.position = Vector2.Lerp(startPostion, endPosition, progress);
        progress += step;
        if(transform.position.x == endPosition.x)
        {
            isRun = false;
            if(!wasHit)
            {
                animator.SetInteger("hit", gameEvent.attackIndex);
            } 
            else mainCamera.GetComponent<EventHandler>().NextPerson();
        }
    }
    void SetSword()
    {
        if(gameEvent.isAttackChance) sword.GetComponent<SpriteRenderer>().color = Color.red;
        else sword.GetComponent<SpriteRenderer>().color = Color.white;
    }
    void SetShield()
    {
        if(gameEvent.isProtectChance) shield.GetComponent<SpriteRenderer>().color = Color.green;
        else shield.GetComponent<SpriteRenderer>().color = Color.white;
    }
    IEnumerator EventExchange()
    {
        while(true)
        {
            if(photonView.IsMine)
            {
                gameEvent = mainCamera.GetComponent<EventHandler>().left;
            }
            else
            {
                mainCamera.GetComponent<EventHandler>().right = gameEvent;
            }
            yield return new WaitForSeconds(0.01f);
            SetSword();
            SetShield();
        }       
    }
}