using Fight.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Select
{
    public class ReadAvatars : MonoBehaviour
    {
        public PlayerController Player;
        public GameObject[] objects;
        public Image[] attacks; 
        public Image[] protects; 
        public string[] avatarNames;
        public Image attack;
        public Image protect;
        public Text avatarName;
        public int currentIndex = 0;
        private const int MinIndex = 0;
        private const int MaxIndex = 2;
        public GameObject nextButton;
        public GameObject buying;
        public Text buyPrice;
        public GameObject coinsText;

        private int GetPrice()
        {
            int price = 0;
            switch (currentIndex)
            {
                case 0: price = 100;
                    break;
                case 1: price = 800;
                    break;
                case 2: price = 1500;
                    break;
            }
            return price;
        }
        public void BuyAvatar()
        {
            if(Player.BuyAvatar(GetPrice(), currentIndex))
            {
                coinsText.GetComponent<Animation>().Play("coins-buy");
                SaveAvatar();
            }
            else
            {
                buying.GetComponent<Animation>().Play("buying-have-not-money");
            }
        }
        public void SaveAvatar()
        {
            PlayerPrefs.SetInt("CurrentAvatar", currentIndex);
            PlayerStorage.SaveCurrentPlayer(Player.Data);
            SetAll();
        }

        private void Start()
        {

            Player = new PlayerController(PlayerStorage.GetCurrentPlayer());
            EditAvatars();
        }

        private void EditAvatars()
        {
            for(int i = 0; i <= MaxIndex; i++)
            {
                objects[i].SetActive(false);
            }
            SetCurrentAvatar();
            SetAll();
        }

        private void SetCurrentAvatar()
        {
            objects[currentIndex].SetActive(true);
            objects[currentIndex].GetComponent<Animator>().SetBool("idle", false);
        }

        private void SetAll()
        {    
            attack.sprite = attacks[currentIndex].sprite;
            protect.sprite = protects[currentIndex].sprite;
            avatarName.text = avatarNames[currentIndex];
            if(Player.WasBought(currentIndex)) 
            {
                nextButton.SetActive(true);
                buying.SetActive(false);
            }
            else
            {
                nextButton.SetActive(false);
                buying.SetActive(true);
                buyPrice.text = "$" + GetPrice();
            }
            coinsText.GetComponent<Text>().text = "$" + Player.Data.coins;
        }
        public void Right()
        {
            if (currentIndex >= MaxIndex) return;
            
            objects[currentIndex].SetActive(false);
            currentIndex++;
            SetCurrentAvatar();
            SetAll();
        }
        public void Left()
        {
            if (currentIndex <= MinIndex) return;
            
            objects[currentIndex].SetActive(false);
            currentIndex--;
            SetCurrentAvatar();
            SetAll();
        }
    }
}
