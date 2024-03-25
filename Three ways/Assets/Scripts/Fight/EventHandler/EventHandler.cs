using System.Collections;
using Fight.GameManager;
using Fight.Person;
using Fight.Player;
using Select;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fight.EventHandler
{
    public class EventHandler : MonoBehaviour
    {
        public GameObject attackController;
        public GameObject protectController;
        public GameObject gameCanvas;
        private const float WaitForNext = 2.8f;
        public int maxHp = 5;
        private bool _needWait;
        private bool _wasFight;
        private bool _isSet;
        private PlayerController _playerInfo;
        public GameObject gameManager;

        public PlayerUI left;
        public PlayerUI right;

        private IEnumerator ShowControllers()
        {
            CheckHealth();
            left.gameEvent.IsSelected = false;
            yield return new WaitForSeconds(WaitForNext); 
            left.gameEvent.IsAttackChance = false;
            left.gameEvent.IsProtectChance = false;
            yield return new WaitForSeconds(0.2f); 
            gameCanvas.GetComponent<Canvas>().sortingOrder = 20;
            attackController.SetActive(true);
            attackController.GetComponent<SelectedWay>().Refresh();
            _needWait = false;
            StopCoroutine("ShowControllers");
        }

        private void SetObjects()
        {
            left.gameEvent = new GameEvent(maxHp);
            right.gameEvent = new GameEvent(maxHp);
        }

        private void Start()
        {
            PlayerData data = PlayerStorage.GetCurrentPlayer();
            _playerInfo = new PlayerController(data);
            Weapons weapons = _playerInfo.GetCurrentWeapon();
            attackController.GetComponent<SelectedWay>().chance = weapons.CountChance(Steel.Sword);
            protectController.GetComponent<SelectedWay>().chance = weapons.CountChance(Steel.Shield);
            _isSet = false;
        }
        public void Begin()
        {
            SetObjects();
            _needWait = false;
            _isSet = true;
            StartCoroutine("ShowControllers");
        }
        public void NextPerson()
        {
            if(_wasFight)
            {
                StartCoroutine("ShowControllers");
            } 
            else
            {
                _wasFight = true;
                right.person.GetComponent<EntityController>().Hitting();
            }
        }

        private void CheckFight()
        {
            if(!_needWait&&left.gameEvent.IsSelected && right.gameEvent.IsSelected)
            {
                _wasFight = false;
                _needWait = true; 
                left.person.GetComponent<EntityController>().Hitting();
            }
        }

        private void SetLeft()
        {
            left.gameEvent.AttackIndex = attackController.GetComponent<SelectedWay>().index;
            left.gameEvent.ProtectIndex = protectController.GetComponent<SelectedWay>().index;
            left.gameEvent.IsAttackChance = attackController.GetComponent<SelectedWay>().isChance;
            left.gameEvent.IsProtectChance = protectController.GetComponent<SelectedWay>().isChance;
            left.gameEvent.IsSelected = true;
        }

        private void SettingFight()
        {
            attackController.GetComponent<SelectedWay>().isSelected = false;
            protectController.GetComponent<SelectedWay>().isSelected = false;
            SetLeft();
        }

        private void CheckChoice()
        {
            if(attackController.GetComponent<SelectedWay>().isSelected &&
               protectController.GetComponent<SelectedWay>().isSelected)
            {
                SettingFight();
            }
        }

        private void Win()
        {
            Weapons weapons = _playerInfo.GetCurrentWeapon();
            int lvl = (weapons.GetLvl(0) + weapons.GetLvl(Steel.Shield))/2;
            GameResult result = GameResult.CountWin(right.points, left.points, lvl);
            result.WriteResult();
        }

        private void Lose()
        {
            Weapons weapons = _playerInfo.GetCurrentWeapon();
            int lvl = (weapons.GetLvl(0) + weapons.GetLvl(Steel.Shield))/2;
            GameResult result = GameResult.CountLose(right.points, left.points, lvl);
            result.WriteResult();
        }

        private void CheckWinner()
        {
            switch (left.hp.value)
            {
                case <= 0 when right.hp.value <= 0:
                    Win();
                    return;
                case <= 0:
                    Lose();
                    break;
            }

            if(right.hp.value <= 0) Win();
        }
        public void ForcedExit(bool mine)
        {
            if(mine) Lose();
            else Win();
            if(mine) SceneManager.LoadScene("EndFight", LoadSceneMode.Single);
            else gameManager.GetComponent<LeaveManager>().EndFight();
        }

        private IEnumerator FightEnd()
        {
            StopCoroutine("ShowControllers");
            yield return new WaitForSeconds(3f);
            CheckWinner();
            gameManager.GetComponent<LeaveManager>().EndFight();
            StopCoroutine("FightEnd");
        }

        private void CheckHealth()
        {
            left.hp.value = left.gameEvent.Hp;
            right.hp.value = right.gameEvent.Hp;
            left.hpText.text = left.gameEvent.Hp.ToString();
            right.hpText.text = right.gameEvent.Hp.ToString();
            if(left.hp.value <= 0) left.person.GetComponent<EntityController>().DieAvatar();
            if(right.hp.value <= 0) right.person.GetComponent<EntityController>().DieAvatar();
            if(left.hp.value <= 0 || right.hp.value <= 0)
            {
                StartCoroutine(FightEnd());
            }
        }

        private void Update()
        {
            if(!_isSet) return;
            CheckChoice();
            CheckFight();
        }
    }
}
