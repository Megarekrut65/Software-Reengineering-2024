using System;
using Fight;
using Fight.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Select
{
    public class DescriptionBoard : MonoBehaviour, 
        IPointerDownHandler, IPointerUpHandler
    {
        public Text title;
        public Text lvl;
        public Text price;
        public Text properties;
        public GameObject mainCamera;
        public GameObject theCanvas;
        public GameObject updateButton;
        public GameObject updating;
        private Weapons _currentWeapon;
        private Steel _currentSteel;
        private int _currentAvatar;

        public void OnPointerDown(PointerEventData eventData)
        {   
            theCanvas.GetComponent<Canvas>().sortingOrder = 0;
            gameObject.SetActive(false);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            //it must be here with OnPointerDown
        }
        public void UpdateWeapon()
        {
            if(mainCamera.GetComponent<ReadAvatars>().Player.UpgradeWeapon(_currentWeapon.CountPrice(_currentSteel), _currentAvatar, _currentSteel))
            {
                SetDescriptions();
                SetNumbers();
                mainCamera.GetComponent<ReadAvatars>().SaveAvatar();
            }
            else
            {
                updating.GetComponent<Animation>().Play("updating-have-not-money");
                updateButton.GetComponent<Animation>().Play("update-have-not-money");
            }
        }

        private void SetDescriptions()
        {
            switch(_currentSteel)
            {
                case Steel.Sword:
                {
                    SetTitleSword(_currentAvatar);
                    SetPropertiesSword(_currentAvatar);
                }
                    break;
                case Steel.Shield:
                {
                    SetTitleShield(_currentAvatar);
                    SetPropertiesShield(_currentAvatar);
                }
                    break;
            }
        }

        private void SetNumbers()
        {
            if(!_currentWeapon.IsMaxLvl(_currentSteel))
            {
                lvl.text = "Level: " + _currentWeapon.GetLvl(_currentSteel);
                price.text = "$" + _currentWeapon.CountPrice(_currentSteel);
            } 
            else 
            {
                lvl.text = "Level: " + _currentWeapon.GetLvl(_currentSteel) + "MAX";
                price.text = "";
                updateButton.SetActive(false);
            }
        
        }
        public void SetData(Steel indexOfSteel)
        {
            updating.GetComponent<Animation>().Play("updating-start");
            _currentSteel = indexOfSteel;
            _currentAvatar = mainCamera.GetComponent<ReadAvatars>().currentIndex;
            _currentWeapon = mainCamera.GetComponent<ReadAvatars>().Player.GetWeapon(_currentAvatar);
            theCanvas.GetComponent<Canvas>().sortingOrder = 20;
            updateButton.SetActive(mainCamera.GetComponent<ReadAvatars>().Player.WasBought(_currentAvatar));
            SetDescriptions();
            SetNumbers();
        }

        private void SetTitleSword(int index)
        {
            switch(index)
            {
                case 0: title.text = "Mace";
                    break;
                case 1: title.text = "Gladius";
                    break;
                case 2: title.text = "Rapier";
                    break;
            }
        }

        private void SetPropertiesSword(int index)
        {
            int chance = Convert.ToInt32(_currentWeapon.CountChance(0));
            string line = "";
            string propertieLine = "With a " + chance.ToString() + "% chance,";
            switch(index)
            {
                case 0: 
                {
                    line = "Great and powerful weapon of that time.\n";
                    propertieLine += " it pierces the shield.";
                }
                    break;
                case 1:
                {
                    line = "This weapon was a favorite among the gladiators who fought in arenas for entertainment.\n";
                    propertieLine += " it deals twice as much damage.";
                }
                    break;
                case 2: 
                {
                    line = "A fast and sharp sword that pierces the enemy with quick blows.\n";
                    propertieLine += " it deals triple as much damage.";
                }
                    break;
                default:
                    break;
            }
            properties.text = line + propertieLine;
        }

        private void SetTitleShield(int index)
        {
            switch(index)
            {
                case 0: title.text = "Round shield";
                    break;
                case 1: title.text = "Wankel shield";
                    break;
                case 2: title.text = "Buckler shield";
                    break;
                default:
                    break;
            }
        }

        private void SetPropertiesShield(int index)
        {
            int chance = Convert.ToInt32(_currentWeapon.CountChance(Steel.Shield));
            string line = "";
            string propertiesLine = "With a " + chance.ToString() + "% chance,";
            switch(index)
            {
                case 0: 
                {
                    line = "Not very strong shield, but able to withstand several blows.\n";
                    propertiesLine += " it strikes at the enemy.";
                }
                    break;
                case 1:
                {
                    line = "Large and heavy shield.\n";
                    propertiesLine += " cancel one skill of the enemy.";
                }
                    break;
                case 2: 
                {
                    line = "Small but effective shield.\n";
                    propertiesLine += " it restores health equal to the damage.";
                }
                    break;
            }
            properties.text = line + propertiesLine;
        }
    }
}
