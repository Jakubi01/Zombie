using Characters.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller.PlayerController
{
    public class PlayerController : Controller
    {
        private PlayerCharacter _playerCharacter;
        public Vector2 Input { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            
            _playerCharacter = GetComponent<PlayerCharacter>();
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
            
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            
        }
    }
}