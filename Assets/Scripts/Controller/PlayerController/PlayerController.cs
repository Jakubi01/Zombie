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
            Input = context.ReadValue<Vector2>();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (_playerShooter)
            {
                _playerShooter.Fire();
            }
        }

        public void OnReload(InputAction.CallbackContext context)
        {
            if (_playerShooter)
            {
                _playerShooter.Reload();
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            
        }
    }
}