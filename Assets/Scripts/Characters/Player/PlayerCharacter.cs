using Controller.PlayerController;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerCharacter : CharacterBase
    {
        private PlayerController _playerController;
        private CharacterController _characterController;
        private const float Speed = 10f;
        private Vector3 _moveDirection;

        private protected void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            _characterController = _playerController.CharacterController;
        }

        private void FixedUpdate()
        {
            ProcessTranslate();
        }

        private void ProcessTranslate()
        {
            if (!_playerController) return;
            var inputValue = _playerController.Input;
            
            // animation
            var worldMove = new Vector3(inputValue.x, 0f, inputValue.y);
            var localMove = transform.InverseTransformDirection(worldMove);

            localMove = Vector3.ClampMagnitude(localMove, 1f);
            
            // TODO : set animation value here.

            _moveDirection = worldMove;
            _characterController.Move((_moveDirection * Speed + new Vector3(0, Physics.gravity.y, 0)) * Time.fixedDeltaTime);
        }
    }
}