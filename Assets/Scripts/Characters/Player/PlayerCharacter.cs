using Animation;
using Controller.PlayerController;
using UnityEngine;

namespace Characters.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(AudioSource))]
    public class PlayerCharacter : CharacterBase
    {
        private PlayerController _playerController;
        private Rigidbody _rigidbody;
        private CapsuleCollider _capsuleCollider;
        private AudioSource _audioSource;
        private Animator _animator;
        
        private const float Speed = 5f;
        private Vector3 _moveDirection;
        private const float TurnSpeed = 10f;

        private protected void Awake()
        {
            _playerController = GetComponent<PlayerController>();

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            _rigidbody.angularDamping = 20f;
            
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _capsuleCollider.center = new Vector3(0f, 0.75f, 0f);
            _capsuleCollider.radius = 0.2f;
            _capsuleCollider.height = 1.5f;
            
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            
            _animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            ProcessTranslate();
            ProcessRotate();
        }

        private void ProcessTranslate()
        {
            var inputValue = _playerController.Input;
    
            _moveDirection = Vector3.ClampMagnitude(new Vector3(inputValue.x, 0f, inputValue.y), 1f);
            
            _animator.SetFloat(AnimationHashToParam.Move, _moveDirection.magnitude);
            _rigidbody.linearVelocity = _moveDirection * Speed;
        }

        private void ProcessRotate()
        {
            if (!(_moveDirection.sqrMagnitude > 0.01f)) return;
            
            var targetRotation = Quaternion.LookRotation(_moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, TurnSpeed * Time.fixedDeltaTime);
        }
    }
}