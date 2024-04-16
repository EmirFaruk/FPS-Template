using UnityEngine;


namespace RunnnerGame.PlayerControl
{
    public class PlayerControl : MonoBehaviour
    {
        private Rigidbody _playerRigidbody;
        private InputManager _inputManager;
        private Animator _animator;

        private bool _grounded;
        private bool _hasAnimator;

        [SerializeField] private float AnimBlendSpeed = 8.9f;
        [SerializeField] private Transform CameraRoot;
        [SerializeField] private Transform Camera;
        [SerializeField] private float UpperLimit = -40f;
        [SerializeField] private float BottomLimit = 70f;
        [SerializeField] private float MouseSensitivity = 21.9f;
        [SerializeField] private float JumpFactor = 260f;
        [SerializeField] private float Dis2Ground = .8f;
        [SerializeField] private LayerMask GroundCheck;
        [SerializeField] private float AirResistance = .8f;

        private int _xVelHash;
        private int _yVelHash;
        private int _jumpHash;
        private int _groundHash;
        private int _fallingHash;

        private const float _walkSpeed = 2;
        private const float _runSpeed = 6;
        private Vector2 _currentVelocity;

        private int _zVelHash;
        private float _xRotation;

        private void Start()
        {
            _hasAnimator = TryGetComponent<Animator>(out _animator);
            _inputManager = GetComponent<InputManager>();
            _playerRigidbody = GetComponent<Rigidbody>();

            _xVelHash = Animator.StringToHash("X_Velocity");
            _yVelHash = Animator.StringToHash("Y_Velocity");
            _zVelHash = Animator.StringToHash("Z_Velocity");
            _jumpHash = Animator.StringToHash("Jump");
            _groundHash = Animator.StringToHash("Grounded");
            _fallingHash = Animator.StringToHash("Falling");
            // _grounded = true;
        }


        private void FixedUpdate()
        {
            SampleGround();
            Move();
            HandleJump();
        }

        private void LateUpdate()
        {
            CamMovements();
        }

        private void Move()
        {
            if (!_hasAnimator) return;

            float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
            if (_inputManager.Move == Vector2.zero) targetSpeed = 0f;

            if (_grounded)
            {
                _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _inputManager.Move.x * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);
                _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _inputManager.Move.y * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);

                var xVelDifference = _currentVelocity.x - _playerRigidbody.velocity.x;
                var zVelDifference = _currentVelocity.y - _playerRigidbody.velocity.z;

                _playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange);
            }
            else
            {
                _playerRigidbody.AddForce(transform.TransformVector(
                    new Vector3(_currentVelocity.x * AirResistance, 0, _currentVelocity.y * AirResistance)),
                    ForceMode.VelocityChange);
            }

            _animator.SetFloat(_xVelHash, _currentVelocity.x);
            _animator.SetFloat(_yVelHash, _currentVelocity.y);
        }


        private void CamMovements()
        {
            if (!_hasAnimator) return;

            var Mouse_X = _inputManager.Look.x;
            var Mouse_Y = _inputManager.Look.y;
            Camera.position = CameraRoot.position;


            _xRotation -= Mouse_Y * MouseSensitivity * Time.smoothDeltaTime;
            _xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit);

            Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            //_playerRigidbody.MoveRotation(_playerRigidbody.rotation * Quaternion.Euler(0, Mouse_X * MouseSensitivity * Time.smoothDeltaTime, 0));
            var rotation = Quaternion.Euler(0, Mouse_X * MouseSensitivity * Time.smoothDeltaTime, 0);
            _playerRigidbody.MoveRotation(_playerRigidbody.rotation * rotation);
        }

        private void HandleJump()
        {
            if (!_hasAnimator) return;
            if (!_inputManager.Jump) return;
            _animator.SetTrigger(_jumpHash);
        }

        public void JumpAddForce()
        {
            _playerRigidbody.AddForce(-_playerRigidbody.velocity.y * Vector3.up, ForceMode.VelocityChange);
            _playerRigidbody.AddForce(Vector3.up * JumpFactor, ForceMode.Impulse);
            _animator.ResetTrigger(_jumpHash);
        }

        private void SampleGround()
        {
            if (!_hasAnimator) return;

            if (Physics.Raycast(_playerRigidbody.worldCenterOfMass, Vector3.down, out var hit, Dis2Ground + .1f, GroundCheck))
            {
                _grounded = true;
                SetAnimationGrounding();
                return;
            }

            _grounded = false;
            _animator.SetFloat(_zVelHash, _playerRigidbody.velocity.y);
            SetAnimationGrounding();
            return;
        }

        private void SetAnimationGrounding()
        {
            _animator.SetBool(_fallingHash, !_grounded);
            _animator.SetBool(_groundHash, _grounded);
        }
    }
}


