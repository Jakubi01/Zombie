using Characters.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller.PlayerController
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : Controller
    {
        private PlayerCharacter _playerCharacter;
        public CharacterController CharacterController { get; private set; }
        public Vector2 Input { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            
            _playerCharacter = GetComponent<PlayerCharacter>();
            CharacterController = GetComponent<CharacterController>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Input = context.ReadValue<Vector2>();
        }
    }
}