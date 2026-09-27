using Characters.Player;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller.PlayerController
{
    public class PlayerController : Controller
    {
        private PlayerCharacter _playerCharacter;
        private PlayerShooter _playerShooter;
        public Vector2 Input { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            
            _playerCharacter = GetComponent<PlayerCharacter>();
            _playerShooter = GetComponent<PlayerShooter>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (!enabled)
            {
                Input = Vector2.zero;
                return;
            }
            
            Input = context.ReadValue<Vector2>();
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (!enabled || GameManager.Instance.IsStandBy) return;

            if (context.started)
            {
                _playerCharacter.Dash();
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!enabled || GameManager.Instance.IsStandBy) return;
            
            if (_playerShooter)
            {
                _playerShooter.Fire();
            }
        }

        public void OnReload(InputAction.CallbackContext context)
        {
            if (!enabled) return;
            
            if (_playerShooter)
            {
                _playerShooter.Reload();
            }
        }
    }
}