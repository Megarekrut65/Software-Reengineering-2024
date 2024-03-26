using System;
using UnityEngine;
using UnityEngine.UI;

namespace Fight.Person
{
    [Serializable]
    public class Entity
    {
        public EventHandler.EventHandler handler;
        public Text nickNameText;
        public Slider hpSlider;
        public Text hpText;
        public Vector2 minePosition;
        public Vector2 enemyPosition;
        public float step;
        public GameObject sword;
        public GameObject shield;

        public void SetMinePlayer(GameObject gameObject)
        {
            handler = GameObject.Find("MainCamera").GetComponent<EventHandler.EventHandler>();
            minePosition = new Vector3(-5.5f, -5f, 0f);
            enemyPosition = new Vector3(4f, -5f, 0f); 
            gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            nickNameText = GameObject.Find("LeftNickName").GetComponent<Text>();
            hpSlider = GameObject.Find("LeftHP").GetComponent<Slider>();
            hpText = GameObject.Find("LeftHPText").GetComponent<Text>();  
            handler.left.person = gameObject;
        }
        public void SetOtherPlayer(GameObject gameObject)
        {
            handler = GameObject.Find("MainCamera").GetComponent<EventHandler.EventHandler>();
            minePosition = new Vector3(5.5f, -5f, 0f); 
            enemyPosition = new Vector3(-4f, -5f, 0f);
            gameObject.transform.localScale = new Vector3(-0.6f, 0.6f, 0.6f);
            nickNameText = GameObject.Find("RightNickName").GetComponent<Text>();
            hpSlider = GameObject.Find("RightHP").GetComponent<Slider>();
            hpText = GameObject.Find("RightHPText").GetComponent<Text>();
            handler.right.person = gameObject;
        }
    }
}