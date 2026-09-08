using Animation;
using Controller.PlayerController;
using Interface;
using Managers;
using UnityEngine;
using UnityEngine.UI;

// UI 관련 코드

// 플레이어 캐릭터의 생명체로서의 동작을 담당
namespace Entity
{
    public class PlayerHealth : LivingEntity
    {
        public Slider healthSlider; // 체력을 표시할 UI 슬라이더

        public AudioClip deathClip; // 사망 소리
        public AudioClip hitClip; // 피격 소리
        public AudioClip itemPickupClip; // 아이템 습득 소리

        private AudioSource _playerAudioPlayer; // 플레이어 소리 재생기
        private Animator _playerAnimator; // 플레이어의 애니메이터

        private PlayerController _playerController; // 플레이어 움직임 컴포넌트
        private PlayerShooter _playerShooter; // 플레이어 슈터 컴포넌트

        private void Awake()
        {
            _playerAnimator = GetComponent<Animator>();
            _playerAudioPlayer = GetComponent<AudioSource>();
            
            _playerController = GetComponent<PlayerController>();
            _playerShooter = GetComponent<PlayerShooter>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            healthSlider.gameObject.SetActive(true);
            healthSlider.maxValue = startingHealth;
            healthSlider.value = Health;

            _playerController.enabled = true;
            _playerShooter.enabled = true;
        }

        private void Start()
        {
            if (GameManager.Instance)
            {
                OnDeath += GameManager.Instance.EndGame;
            }
        }

        // 체력 회복
        public override void RestoreHealth(float newHealth)
        {
            base.RestoreHealth(newHealth);

            healthSlider.value = Health;
        }

        // 데미지 처리
        public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
        {
            if (!Dead)
            {
                _playerAudioPlayer.PlayOneShot(deathClip);
            }
            
            base.OnDamage(damage, hitPoint, hitDirection);
            healthSlider.value = Health;
        }

        // 사망 처리
        public override void Die()
        {
            base.Die();
            
            healthSlider.gameObject.SetActive(false);
            
            _playerAudioPlayer.PlayOneShot(deathClip);
            _playerAnimator.SetTrigger(AnimationHashToParam.Die);
            
            _playerController.enabled = false;
            _playerShooter.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Dead) return;
            
            IItem item = other.GetComponent<IItem>();
            if (item == null) return;
            
            item.Use(gameObject);
            _playerAudioPlayer.PlayOneShot(itemPickupClip);
        }
    }
}