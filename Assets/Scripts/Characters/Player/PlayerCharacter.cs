using System.Collections;
using Animation;
using Controller.PlayerController;
using UnityEngine;
using UnityEngine.InputSystem;

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
        private Camera _mainCamera;

        private const float Speed = 5f;
        private Vector3 _moveDirection;
        private const float TurnSpeed = 10f;
        
        [Header("Dash Settings")]
        [SerializeField] private float dashForce = 20f;

        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float dashCooldown = 1f; 
        private bool _canDash = true;
        private bool _isDashing = false;

        private protected void Awake()
        {
            _playerController = GetComponent<PlayerController>();

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            _rigidbody.angularDamping = 20f;
            _rigidbody.mass = 1f;
            _rigidbody.linearDamping = 1f;

            _capsuleCollider = GetComponent<CapsuleCollider>();
            _capsuleCollider.center = new Vector3(0f, 0.75f, 0f);
            _capsuleCollider.radius = 0.2f;
            _capsuleCollider.height = 1.5f;

            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;

            _animator = GetComponent<Animator>();
            
            _mainCamera = Camera.main;
        }

        private void FixedUpdate()
        {
            if(!_isDashing)
                ProcessTranslate();
            
            ProcessRotate();
        }

        private void ProcessTranslate()
        {
            var currentVelocity = _rigidbody.linearVelocity;

            if (!_playerController || !_playerController.isActiveAndEnabled)
            {
                _moveDirection = Vector3.zero;
                _animator.SetFloat(AnimationHashToParam.Move, 0f);
                _rigidbody.linearVelocity = new Vector3(0f, currentVelocity.y, 0f);
                return;
            }

            var inputValue = _playerController.Input;
            _moveDirection = Vector3.ClampMagnitude(new Vector3(inputValue.x, 0f, inputValue.y), 1f);

            _animator.SetFloat(AnimationHashToParam.Move, _moveDirection.magnitude);
            _rigidbody.linearVelocity = new Vector3(
                _moveDirection.x * Speed, 
                currentVelocity.y,
                _moveDirection.z * Speed);
        }

        private void ProcessRotate()
        {
            if (!_playerController || !_playerController.isActiveAndEnabled || Mouse.current == null) return;
            if (!_mainCamera) return;

            var mouseRay = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            var playerPlane = new Plane(Vector3.up, transform.position);
            if (!playerPlane.Raycast(mouseRay, out var distance)) return;

            var lookDirection = mouseRay.GetPoint(distance) - transform.position;
            lookDirection.y = 0f;
            if (lookDirection.sqrMagnitude <= 0.0001f) return;

            var targetRotation = Quaternion.LookRotation(lookDirection);
            _rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation, targetRotation, TurnSpeed * Time.fixedDeltaTime));
        }

        public void Dash()
        {
            if (!_canDash || _isDashing) return;

            var dashDirection = _moveDirection;
            
            if (dashDirection.sqrMagnitude <= 0.0001f)
            {
                dashDirection = transform.forward;
            }

            StartCoroutine(DashRoutine(dashDirection));
        }
        
        private IEnumerator DashRoutine(Vector3 direction)
        {
            _canDash = false;
            _isDashing = true;

            _rigidbody.linearVelocity = Vector3.zero;
            
            _rigidbody.AddForce(direction.normalized * dashForce, ForceMode.VelocityChange);

            yield return new WaitForSeconds(dashDuration);
            
            _isDashing = false;

            yield return new WaitForSeconds(dashCooldown);
            _canDash = true;
        }
    }
}
