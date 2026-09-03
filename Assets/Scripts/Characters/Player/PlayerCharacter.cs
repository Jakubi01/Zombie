using Controller.PlayerController;
using UnityEngine;

namespace Characters.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(PlayerController))]
    public class PlayerCharacter : CharacterBase
    {
        private PlayerController _playerController;
        private Rigidbody _rigidbody;
        private const float Speed = 10f;
        private Vector3 _moveDirection;

        private protected void Awake()
        {
            _playerController = GetComponent<PlayerController>();

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void FixedUpdate()
        {
            ProcessTranslate();
        }

        private void ProcessTranslate()
        {
            var inputValue = _playerController.Input;
    
            var inputDirection = Vector3.ClampMagnitude(new Vector3(inputValue.x, 0f, inputValue.y), 1f);
            var localMove = transform.InverseTransformDirection(inputDirection);
    
            // TODO : 애니메이터 값 세팅
            
            _moveDirection = inputDirection;
            _rigidbody.linearVelocity = _moveDirection * Speed;
        }
    }
}