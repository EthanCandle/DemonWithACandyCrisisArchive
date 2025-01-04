using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

//namespace StarterAssets

public class PlayerMovementScript : MonoBehaviour
{
    public GameObject playerObj;
    public ThirdPersonController pc;
    public PlayerInput pi;
    public bool isPushed, shouldBePushed;
    public GameObject enemyCurrentTouched;
    public Vector3 enemyPosition = new Vector3(0,0,0);
    public AnimationClip stunClip;
    public Animator pAnimator;
public float stunTime = 0.5f;

    public void Start()
    {
        //print(stunClip.apparentSpeed);
        stunTime = stunClip.length / pAnimator.GetFloat("AnimationSpeed");
        //print((stunClip.length + " " + pAnimator.GetFloat("AnimationSpeed")));
    }
    public void Update()
    {
        // when the player is pushed, move them 
        if (isPushed == true)
        {
            PlayerPush(enemyPosition);
        }
    }

    public void PlayerJump()
    {

    }

    // 
    public void PushedFromEnemyMethod(GameObject enemy)
    {
        // if they are already being pushed, don't push again
        if (isPushed)
        {
            return;
        }
        // changes the players speed to 0
        pc.SetSpeedToZero();
        // sets isPushed to true because they are pushed
        isPushed = true;
        // adjusts the stun animation time
        stunTime = stunClip.length / pAnimator.GetFloat("AnimationSpeed");
        // sets teh animation trigger for being pushed
        pc._animator.SetBool("Pushed", true);
        // starts the timer to stop being stunned (not being able to move)
        StartCoroutine(Stunned());
        // starts the timer to stop being pushed in a direction
        StartCoroutine(BeingPushed());
        // gets the enemy that touched the player
        enemyPosition = enemy.transform.position;
        // stop the players speed again
        pc.SetSpeedToZero();
    }

    // prevent moving
    public IEnumerator Stunned()
    {
        // resets the players y fall speed to 0
        pc._verticalVelocity = 0;
        yield return new WaitForSeconds(stunTime);
        // sets isPushed to false because they aren't being pushed or stunned anymore
        isPushed = false;
        // so player can move and jump now since they aren't stunned
        pc.canMove = true;
        pc.canJump = true;
        // resets pushed bool to false
        pc._animator.SetBool("Pushed", false);
    }

    // move the player
    public IEnumerator BeingPushed()
    {
        // 
        shouldBePushed = true;
        // half the time of the stun time because of the get up animation
        yield return new WaitForSeconds(stunTime / 2);
        shouldBePushed = false;
    }

    public void PlayerPush(Vector3 enemyPosition)
    {
        // stops player from moving up in the y axis
        if(pc._verticalVelocity > 0)
        {
            pc._verticalVelocity = 0;
        }
        // manuelly controls their y falling speed exponentially
        pc._verticalVelocity -= Mathf.Pow(20, Time.deltaTime+1) * Time.deltaTime;

        // if player should be pushed at this moment
        if (shouldBePushed)
        {
            // pushes player from enemy object position
            pc.PushForce(default, enemyPosition);
        }
        else
        {
            pc._controller.Move(new Vector3(0, pc._verticalVelocity * Time.deltaTime,0));

        }
        pc.StopJumpInputs();
        pc.canMove = false;
        pc.canJump = false;
        pc.coyoteJumpTime = 0;
        pc.jumpTimeHold = 0;
    }


    /* Note: animations are called via the controller for both the character and capsule using animator null checks







        //[RequireComponent(typeof(PlayerInput))]


            [Header("Player")]
            [Tooltip("Move speed of the character in m/s")]
            public float MoveSpeed = 2.0f;

            [Tooltip("Sprint speed of the character in m/s")]
            public float SprintSpeed = 5.335f;

            [Tooltip("How fast the character turns to face movement direction")]
            [Range(0.0f, 0.3f)]
            public float RotationSmoothTime = 0.12f;

            [Tooltip("Acceleration and deceleration")]
            public float SpeedChangeRate = 10.0f;

            public AudioClip LandingAudioClip;
            public AudioClip[] FootstepAudioClips;
            [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

            [Space(10)]
            [Tooltip("The height the player can jump")]
            public float JumpHeight = 1.2f;
            public float jumpTimeHold = 0, jumpTimeHoldReset = 0.5f;

            public float coyoteJumpTime = 0, coyoteJumpTimeReset = 0.25f;
            [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
            public float Gravity = -15.0f;

            [Space(10)]
            [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
            public float JumpTimeout = 0.50f;

            [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
            public float FallTimeout = 0.15f;

            [Header("Player Grounded")]
            [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
            public bool Grounded = true;

            [Tooltip("Useful for rough ground")]
            public float GroundedOffset = -0.14f;

            [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
            public float GroundedRadius = 0.28f;

            [Tooltip("What layers the character uses as ground")]
            public LayerMask GroundLayers;

            [Header("Cinemachine")]
            [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
            public GameObject CinemachineCameraTarget;

            [Tooltip("How far in degrees can you move the camera up")]
            public float TopClamp = 70.0f;

            [Tooltip("How far in degrees can you move the camera down")]
            public float BottomClamp = -30.0f;

            [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
            public float CameraAngleOverride = 0.0f;

            [Tooltip("For locking the camera position on all axis")]
            public bool LockCameraPosition = false;

            // cinemachine
            private float _cinemachineTargetYaw;
            private float _cinemachineTargetPitch;

            // player
            private float _speed;
            private float _animationBlend;
            private float _targetRotation = 0.0f;
            private float _rotationVelocity;
            private float _verticalVelocity;
            private float _terminalVelocity = 53.0f;

            // timeout deltatime
            private float _jumpTimeoutDelta;
            private float _fallTimeoutDelta;

            // animation IDs
            private int _animIDSpeed;
            private int _animIDGrounded;
            private int _animIDJump;
            private int _animIDFreeFall;
            private int _animIDMotionSpeed;
            public float pos1; // this is for the roof check


    #if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            private PlayerInput _playerInput;
    #endif
            private Animator _animator;
            private CharacterController _controller;
            public StarterAssetsInputs _input;
            private GameObject _mainCamera;

            private const float _threshold = 0.01f;

            private bool _hasAnimator;

            private bool IsCurrentDeviceMouse
            {
                get
                {
    #if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                    return _playerInput.currentControlScheme == "KeyboardMouse";
    #else
                    return false;
    #endif
                }
            }


            private void Awake()
            {
                // get a reference to our main camera
                if (_mainCamera == null)
                {
                    _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                }
            }

            private void Start()
            {
                _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

                _hasAnimator = TryGetComponent(out _animator);
                _controller = GetComponent<CharacterController>();
                _input = GetComponent<StarterAssetsInputs>();
    #if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                _playerInput = GetComponent<PlayerInput>();
    #else
                Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
    #endif

                AssignAnimationIDs();

                // reset our timeouts on start
                _jumpTimeoutDelta = JumpTimeout;
                _fallTimeoutDelta = FallTimeout;
            }

            private void Update()
            {
                _hasAnimator = TryGetComponent(out _animator);
                if (_input.jumpHold)
                {
                    // print("JumpHold In Update");

                }
                JumpAndGravity();
                GroundedCheck();
                Move();
                CheckIfRoofed();
            }

            private void LateUpdate()
            {
                CameraRotation();
            }

            private void AssignAnimationIDs()
            {
                _animIDSpeed = Animator.StringToHash("Speed");
                _animIDGrounded = Animator.StringToHash("Grounded");
                _animIDJump = Animator.StringToHash("Jump");
                _animIDFreeFall = Animator.StringToHash("FreeFall");
                _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            }

            private void GroundedCheck()
            {
                // set sphere position, with offset
                Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                    transform.position.z);
                Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                    QueryTriggerInteraction.Ignore);

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDGrounded, Grounded);
                }
            }

            private void CameraRotation()
            {
                // if there is an input and camera position is not fixed
                if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
                {
                    //Don't multiply mouse input by Time.deltaTime;
                    float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                    _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                    _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
                }

                // clamp our rotations so our values are limited 360 degrees
                _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
                _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

                // Cinemachine will follow this target
                CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                    _cinemachineTargetYaw, 0.0f);
            }

            private void Move()
            {
                // set target speed based on move speed, sprint speed and if sprint is pressed
                float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

                // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

                // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
                // if there is no input, set the target speed to 0
                if (_input.move == Vector2.zero) targetSpeed = 0.0f;

                // a reference to the players current horizontal velocity
                float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

                float speedOffset = 0.1f;
                float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

                // accelerate or decelerate to target speed
                if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                    currentHorizontalSpeed > targetSpeed + speedOffset)
                {
                    // creates curved result rather than a linear one giving a more organic speed change
                    // note T in Lerp is clamped, so we don't need to clamp our speed
                    _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                        Time.deltaTime * SpeedChangeRate);

                    // round speed to 3 decimal places
                    _speed = Mathf.Round(_speed * 1000f) / 1000f;
                }
                else
                {
                    _speed = targetSpeed;
                }

                _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
                if (_animationBlend < 0.01f) _animationBlend = 0f;

                // normalise input direction
                Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

                // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
                // if there is a move input rotate player when the player is moving
                if (_input.move != Vector2.zero)
                {
                    _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                        _mainCamera.transform.eulerAngles.y;
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                        RotationSmoothTime);

                    // rotate to face input direction relative to camera position
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }


                Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

                // move the player
                _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                                    new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetFloat(_animIDSpeed, _animationBlend);
                    _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
                }
            }

            private void JumpAndGravity()
            {
                if (Grounded)
                {

                    if (_input.jumpHold && jumpTimeHold > 0) // edds
                    {
                        jumpTimeHold = 0;
                    }
                    // reset the fall timeout timer
                    _fallTimeoutDelta = FallTimeout;

                    jumpTimeHold = jumpTimeHoldReset; // resets jump holding

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, false);
                        _animator.SetBool(_animIDFreeFall, false);
                    }

                    // stop our velocity dropping infinitely when grounded
                    if (_verticalVelocity < 0.0f)
                    {
                        _verticalVelocity = -2f;
                    }

                    // Jump
                    if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                    {
                        // the square root of H * -2 * G = how much velocity needed to reach desired height
                        _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                        // update animator if using character
                        if (_hasAnimator)
                        {
                            _animator.SetBool(_animIDJump, true);
                        }
                    }
                    else
                    {
                        coyoteJumpTime = coyoteJumpTimeReset; // reset coyote timer
                    }

                    // jump timeout
                    if (_jumpTimeoutDelta >= 0.0f)
                    {
                        _jumpTimeoutDelta -= Time.deltaTime;
                    }

                }
                else if (coyoteJumpTime > 0)
                {

                    // _input.jump = false;
                    coyoteJumpTime -= 1 * Time.deltaTime; // lower coyote time;
                                                            // Jump

                    if (_input.jumpHold && jumpTimeHold > 0) // holding jump to still jump higher
                    {
                        _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                        jumpTimeHold -= 1 * Time.deltaTime;
                        print("JumpHOldBugger");
                        //print(jumpTimeHold);
                        //  print(Grounded);
                        if (_hasAnimator)
                        {
                            _animator.SetBool(_animIDJump, true);
                        }
                        return;
                    }


                }
                else
                {


                    if (_input.jumpHold && jumpTimeHold > 0) // holding jump to still jump higher
                    {
                        _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                        jumpTimeHold -= 1 * Time.deltaTime;
                        print("JumpHOldBugger");
                        //print(jumpTimeHold);
                        //  print(Grounded);

                        return;
                    }
                    else
                    {
                        _input.jumpHold = false;
                        jumpTimeHold = 0;

                    }
                    // reset the jump timeout timer
                    _jumpTimeoutDelta = JumpTimeout;

                    // fall timeout
                    if (_fallTimeoutDelta >= 0.0f)
                    {
                        _fallTimeoutDelta -= Time.deltaTime;
                    }
                    else
                    {
                        // update animator if using character
                        if (_hasAnimator)
                        {
                            _animator.SetBool(_animIDFreeFall, true);
                        }
                    }

                    // if we are not grounded, do not jump
                    _input.jump = false;
                }

                // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
                if (_verticalVelocity < _terminalVelocity)
                {
                    _verticalVelocity += Gravity * Time.deltaTime;
                }
            }


            public void CheckIfRoofed()
            {

                if (pos1 == gameObject.transform.position.y && !Grounded)
                {
                    print("roofed");
                    jumpTimeHold = 0;
                    coyoteJumpTime = 0;
                    _verticalVelocity = -2.0f;
                }
                pos1 = gameObject.transform.position.y;
            }

            // moves the player
            public void JumpForce(float multiplier)
            {
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity * multiplier);
            }
            private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
            {
                if (lfAngle < -360f) lfAngle += 360f;
                if (lfAngle > 360f) lfAngle -= 360f;
                return Mathf.Clamp(lfAngle, lfMin, lfMax);
            }

            private void OnDrawGizmosSelected()
            {
                Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
                Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

                if (Grounded) Gizmos.color = transparentGreen;
                else Gizmos.color = transparentRed;

                // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
                Gizmos.DrawSphere(
                    new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                    GroundedRadius);
            }

            private void OnFootstep(AnimationEvent animationEvent)
            {
                if (animationEvent.animatorClipInfo.weight > 0.5f)
                {
                    if (FootstepAudioClips.Length > 0)
                    {
                        var index = Random.Range(0, FootstepAudioClips.Length);
                        AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                    }
                }
            }

            private void OnLand(AnimationEvent animationEvent)
            {
                if (animationEvent.animatorClipInfo.weight > 0.5f)
                {
                    AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }
        */
}

