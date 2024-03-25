using System;
using System.Collections;
using Fight.EventHandler;
using Photon.Pun;
using UnityEngine;

namespace Fight.Person
{
    public class EntityController : MonoBehaviour
    {
        [SerializeField]
        private Entity entity;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private Vector2 startPosition;
        [SerializeField]
        private Vector2 endPosition;
        [SerializeField]
    
        public int index = 0;
        public bool isStunned = false;
    
        private PhotonView _photonView;
        private EventSync _sync;
    
        private float _progress;
        private bool _isRun;
        private bool _wasHit;

        private void SetUI()
        {
            transform.position = entity.minePosition;
            entity.hpSlider.maxValue = entity.handler.maxHp;
            entity.hpText.text = entity.hpSlider.maxValue.ToString();
            entity.nickNameText.text = _photonView.Owner.NickName;   
        }
        private void SetPlayer()
        {  
            if (_photonView.IsMine) entity.SetMinePlayer(gameObject);
            else entity.SetOtherPlayer(gameObject);
            SetUI(); 
        }
        public void Hitting()
        {
            startPosition = entity.minePosition;
            endPosition = entity.enemyPosition;
            _progress = 0;
            _isRun = true;
            animator.SetBool("run", _isRun );
            _wasHit = false;
        }
        public void SetIdle()
        {
            animator.SetBool("block", false );
            animator.SetBool("block-skill", false );
            animator.SetBool("damage", false );
        }

        private int AttackSpecialSkill(int indexOfEnemy)
        {
            switch (indexOfEnemy)
            {
                case 0: return -1;
                case 1: return -2;
                case 2: return -3;
                default: return -1;
            }
        }
        public void EditMineHp(int value)
        {
            entity.handler.left.gameEvent.Hp += value; 
            entity.hpSlider.value = entity.handler.left.gameEvent.Hp;
            entity.hpText.text = entity.handler.left.hp.ToString();
        }
        public void EditHp(int value)
        {
            entity.hpSlider.value = _sync.GameEvent.Hp + value;
            entity.hpText.text = (_sync.GameEvent.Hp + value).ToString();
        }
        public void AttackSound()
        {
            entity.sword.GetComponent<AudioSource>().Play();
        }
        public void ProtectSound()
        {
            entity.shield.GetComponent<AudioSource>().Play();
        }

        private void Attack(bool isChance, int indexOfEnemy)
        {
            int damage = -1;
            if(isChance) damage = AttackSpecialSkill(indexOfEnemy);
            animator.SetBool("damage", true );
            if(_photonView.IsMine) EditMineHp(damage);
            else EditHp(damage);
        
        }
        public void DieAvatar()
        {
            animator.SetBool("die", true );
        }
        public void SetStun(bool stun)
        {
            isStunned = stun;
        }
        private void ProtectSpecialSkill()
        {
            if(isStunned) 
            {
                SetStun(false);
                return;
            }

            EntityController entityController = (_photonView.IsMine 
                    ? entity.handler.right.person : entity.handler.left.person)
                .GetComponent<EntityController>();
        
            switch (index)
            {
                case 0: entityController.GetHit(Direction.None, false, index);break;
                case 1: entityController.SetStun(true); break;
                case 2:
                {
                    if(_photonView.IsMine) EditMineHp(1);
                    else EditHp(1);
                }
                    break;
            }
        }
        private void Protect(bool isChance, int indexOfEnemy)
        {
            animator.SetBool("block", true );
            if(isChance && indexOfEnemy == 0) Attack(false, 0);
            else if(_sync.GameEvent.IsProtectChance) ProtectSpecialSkill();
        }
        private void GetHit(Direction enemyAttack, bool isChance, int indexOfEnemy)
        {
            if(enemyAttack != _sync.GameEvent.ProtectIndex) Attack(isChance, indexOfEnemy);  
            else Protect(isChance, indexOfEnemy);
        }
        public void Fight()
        {
            AttackSound();
            bool chance = _sync.GameEvent.IsAttackChance;
            if(isStunned && _sync.GameEvent.IsAttackChance)
            {
                isStunned = false;
                chance  = false;
            }
            EntityController entityController = (_photonView.IsMine 
                    ? entity.handler.right.person : entity.handler.left.person)
                .GetComponent<EntityController>();
        
            entityController.GetHit(_sync.GameEvent.AttackIndex, chance, index);
        }
        public void StopHit()
        {
            animator.SetBool("run", false );
            animator.SetInteger("hit", 0);
            _wasHit = true;
            startPosition = entity.enemyPosition;
            endPosition = entity.minePosition;
            _progress = 0;
            _isRun = true;
        }
        private void Start()
        {
            _sync = new EventSync();
            _isRun = false;
            animator = GetComponent<Animator>();
            _photonView = GetComponent<PhotonView>();
            SetPlayer();
            StartCoroutine(EventExchange());
        }
        private void FixedUpdate()
        {
            if(!_isRun) return;
            transform.position = Vector2.Lerp(startPosition, endPosition, _progress);
            _progress += entity.step;
            if (!(Math.Abs(transform.position.x - endPosition.x) < 0.01)) return;
        
            _isRun = false;
            if(!_wasHit)
            {
                animator.SetInteger("hit", (int)_sync.GameEvent.AttackIndex);
            } 
            else entity.handler.NextPerson();
        }
        private void SetSword()
        {
            entity.sword.GetComponent<SpriteRenderer>().color = _sync.GameEvent.IsAttackChance ? Color.red : Color.white;
        }

        private void SetShield()
        {
            entity.shield.GetComponent<SpriteRenderer>().color = _sync.GameEvent.IsProtectChance ? Color.green : Color.white;
        }

        private IEnumerator EventExchange()
        {
            while(true)
            {
                if(_photonView.IsMine) _sync.GameEvent = entity.handler.left.gameEvent;
                else entity.handler.right.gameEvent = _sync.GameEvent;
            
                yield return new WaitForSeconds(0.01f);
                SetSword();
                SetShield();
            }       
        }
    }
}