using Characters.Player;
using Controller.PlayerController;
using Entity;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] private Button ammo;
        [SerializeField] private Button health;
        [SerializeField] private Button speed;

        private GameObject _player;
        private PlayerShooter _playerShooter;
        private PlayerCharacter _playerCharacter;
        private LivingEntity _livingEntity;
        
        private const float RestoreHealthValue = 30f;
        private const float SpeedValue = 1.1f;
        
        private void Start()
        {
            SetPlayerReferences();
        }
        
        private void OnEnable()
        {
            if (!_player || !_playerCharacter || !_livingEntity)
                SetPlayerReferences();
            
            ammo.onClick.AddListener(OnAmmoClick);
            health.onClick.AddListener(OnHealthClick);
            speed.onClick.AddListener(OnSpeedClick);
        }

        private void OnDisable()
        {
            ammo.onClick.RemoveAllListeners();
            health.onClick.RemoveAllListeners();
            speed.onClick.RemoveAllListeners(); 
        }

        private void SetPlayerReferences()
        {
            var pc = FindFirstObjectByType<PlayerController>();
            _player = pc.gameObject;
            _playerShooter = _player.GetComponent<PlayerShooter>();
            _playerCharacter = _player.GetComponent<PlayerCharacter>();
            _livingEntity = _player.GetComponent<LivingEntity>();
        }

        private void OnAmmoClick()
        {
            _playerShooter.gun.ammoRemain += 30;

            var uiManager = UIManager.Instance;
            uiManager.UpdateAmmoText(_playerShooter.gun.magAmmo, _playerShooter.gun.ammoRemain);
            uiManager.ToggleShop(false);
        }

        private void OnHealthClick()
        {
            _livingEntity.RestoreHealth(RestoreHealthValue);
            
            UIManager.Instance.ToggleShop(false);
        }

        private void OnSpeedClick()
        {
            _playerCharacter.IncreaseMoveSpeed(SpeedValue);
            
            UIManager.Instance.ToggleShop(false);
        }
    }
}