using Characters.Player;
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

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (!enabled) return;

            if (context.started)
            {
                _playerCharacter.SpeedMultiplier = 1.2f;
            }
            else if (context.canceled)
            {
                _playerCharacter.SpeedMultiplier = 1f;
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!enabled) return;
            
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

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!enabled) return;
        }
    }
}