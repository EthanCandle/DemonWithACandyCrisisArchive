using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public bool hasControl = true;

    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 7.0f;

    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 20.0f;

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
    public float JumpHeight = 2.5f;
    public float jumpTimeHold = 0, jumpTimeHoldReset = 0.35f;

    public float coyoteJumpTime = 0, coyoteJumpTimeReset = 0.15f;
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -35.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.0f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;
    [Tooltip("The character uses its own gravity value for slopes. The engine default is -9.81f")]
    public float GravityOfSloper = -45.0f;
    public bool groundedOnceCheck = false;


    public bool isGoingToBeOffGround = false; // used to make falling off ledges more smooth without causing a dip

    public float timeToHoldJumpBufferCurrent = 0;
    public float timeToHoldJumpBufferMax = 0.25f;
    public bool jumpBufferIsOn = false;

    public float slideFactor = 0.5f; // Sliding intensity
    public float slideThreshold = 0.7f; // Controls when to start sliding, 1 means directly into slope

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
    public float speedCurrent;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    public float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    // timeout deltatime
    public float _jumpTimeoutDelta; /// i think this is input delay on jumping after touchign the ground
    private float _fallTimeoutDelta;

    // animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;
    public float pos1; // this is for the roof check
    public bool canMove = true, canJump = true;

    public PlayerInput _playerInput;

    public Animator _animator;
    public CharacterController _controller;
    public InputManager _input;
    public GameObject _mainCamera;

    public const float _threshold = 0.01f;

    public bool _hasAnimator;

    public Sound landedSFX, jumpSFX;
    public bool isRoofed = false;
    public float roofOffsetSphere = 2.0f;

    public float dashSpeed = 15.0f, dashDuration = 1.0f;
    public bool isDashing = false;
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
        _input = GetComponent<InputManager>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

        AssignAnimationIDs();

        // reset our timeouts on start
        _jumpTimeoutDelta = JumpTimeout;
        // _fallTimeoutDelta = FallTimeout;


    }

    public void GainPlayerControl()
    {
        SetPlayerControl(true);
        _playerInput.actions["Move"].Enable();
        _playerInput.actions["Jump"].Enable();
        _playerInput.actions["JumpHold"].Enable();
    }

    public void LosePlayerControl()
    {
        SetPlayerControl(false);
        SetSpeedToZero();
        _playerInput.actions["Move"].Disable();
        _playerInput.actions["Jump"].Disable();
        _playerInput.actions["JumpHold"].Disable();
        _animator.SetBool(_animIDJump, false);
        _animator.SetBool(_animIDFreeFall, false);
    }

    public void SetPlayerControl(bool playerControlState = true)
    {
        hasControl = playerControlState;
      //  _playerInput.enabled = playerControlState;
    }
    private void FixedUpdate()
    {
        GroundedCheck();
        CheckIfRoofed();
    }

    private void Update()
    {
        //print(_animator.GetFloat(_animIDSpeed));
        ///_hasAnimator = TryGetComponent(out _animator); // prolly not needed? since its already at start

        if (hasControl)
        {

            JumpAndGravity(); // should be after ground check because it still thinks its on ground when we jump because we havn't moved yet causing a bug
            Move();

            BufferJumpFunction();
            DashMechanic();
            //print($"y: {transform.position.y}");
        }
        else
        {
            AddGravity();
        }

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
        // print("Set bool" + Grounded);
        bool lastCheckGrounded = Grounded;
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

     //   OnDrawGizmos();
        // update animator if using character
        if (_hasAnimator)
        {
            _animator.SetBool(_animIDGrounded, Grounded);
          //  print("Set bool2" + Grounded);
        }

        if (Grounded && !lastCheckGrounded)
        {
            //print($"Ground: {Grounded}, LastGround: {lastCheckGrounded}");
             FindObjectOfType<AudioManager>().PlaySoundInstantiate(landedSFX);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z), GroundedRadius);

        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + roofOffsetSphere,
            transform.position.z), GroundedRadius);
    }

    public void BufferJumpFunction()
    {
        if (_input.jump)
        {
            timeToHoldJumpBufferCurrent = timeToHoldJumpBufferMax;
            _input.jump = false;
        }

        if (timeToHoldJumpBufferCurrent > 0)
        {
            timeToHoldJumpBufferCurrent -= 1 * Time.deltaTime;
            jumpBufferIsOn = true;
            if (timeToHoldJumpBufferCurrent <= 0){
                jumpBufferIsOn = false;
            }
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
        if (!canMove)
        {
            _animator.SetFloat(_animIDSpeed, 0);
            return;
        }
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed;
        if (_input.sprint)
        {
            targetSpeed = SprintSpeed;
        }
        else
        {
            targetSpeed = MoveSpeed;
        }
        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        // float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
        float inputMagnitude;
        if (_input.analogMovement)
        {
            inputMagnitude = _input.move.magnitude;
        }
        else
        {
            inputMagnitude = 1.0f;
        }


        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            speedCurrent = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            speedCurrent = Mathf.Round(speedCurrent * 1000f) / 1000f;
        }
        else
        {
            speedCurrent = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        // normalise input direction
        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (_input.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        if (canMove)
        {
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (speedCurrent * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }
        // update animator if using character
        if (_hasAnimator)
        {
            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }
    }

    private void JumpAndGravity()
    {

        // this prevents the player from hovering after leaving the ground assuming no slopes were detected
        if (Grounded == false && groundedOnceCheck && _verticalVelocity <= 0) // prevent stop jumping
        {
            print("In grounded once check guard");
            groundedOnceCheck = false;
            _verticalVelocity = -2.5f;
        }

        // jump timeout (prevents player from jumping 
        if (_jumpTimeoutDelta >= 0.0f)
        {
            _jumpTimeoutDelta -= Time.deltaTime;
        }

        if (Grounded && _jumpTimeoutDelta <= 0.0f)
        {
          //  print($"On ground right now _input.jump = ({_input.jump} _jumpTimeoutDelta = ({_jumpTimeoutDelta}");
            groundedOnceCheck = true;
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
                // Get the bounds of the player's collider
                Bounds bounds = _controller.bounds;

                // Calculate the front-most point of the collider (front of the player)
                Vector3 frontEdge = transform.position + transform.forward * bounds.extents.z;
                // Adjust the raycast position to be at the bottom front edge of the collider
                Vector3 rayOrigin = new Vector3(frontEdge.x, bounds.min.y, frontEdge.z);

                // Downward direction for the raycast
                Vector3 downwardDirection = Vector3.down;

                // Debug Ray (for visualization)
                Debug.DrawRay(rayOrigin, downwardDirection * 1, Color.blue);



                // Cast the ray
                RaycastHit hit;

                // guard to prevent cylinder collider messhaps when falling off edges
                // shoots in a downwards direction 1 space in front of the plauyer, means we are on ground/slope
                if (Physics.Raycast(rayOrigin, downwardDirection, out hit, 1.0f, GroundLayers))
                {
                    _verticalVelocity = GravityOfSloper;
                    // Debug.Log("Ray hit: " + hit.collider.name);

                    // You can add logic here for when the ray hits something
                }
                else
                {
                    // print("No floor ahead, velocity = 0");
                    _verticalVelocity = 0;
                }

            }

            // Jump
            if (jumpBufferIsOn)
            {
                FindObjectOfType<AudioManager>().PlaySoundInstantiate(jumpSFX);
              //  print($"Jumping: {jumpTimeHold}");
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
              //  Debug.Log("Jump Velocity: " + _verticalVelocity);  // Should be ~7.67 m/s
                coyoteJumpTime = 0;
                //_input.jump = false;
                // update animator if using character
                if (_hasAnimator)
                {

                    _animator.SetBool(_animIDJump, true);
                }
                _jumpTimeoutDelta = JumpTimeout;
                // stops the falling check at the ttop if you jump 
                groundedOnceCheck = false;
                _input.jump = false;

            }
            else
            {
                coyoteJumpTime = coyoteJumpTimeReset; // reset coyote timer
            }



        }
        else if (coyoteJumpTime > 0)
        {

            // _input.jump = false;
            coyoteJumpTime -= 1 * Time.deltaTime; // lower coyote time;
                                                  // Jump

            // Jump
            if (jumpBufferIsOn)
            {
                FindObjectOfType<AudioManager>().PlaySoundInstantiate(jumpSFX);
               // print("Coyote Jumping");
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                //  Debug.Log("Jump Velocity: " + _verticalVelocity);  // Should be ~7.67 m/s
                coyoteJumpTime = 0;
                //_input.jump = false;
                // update animator if using character
                if (_hasAnimator)
                {

                    _animator.SetBool(_animIDJump, true);
                }
                _jumpTimeoutDelta = JumpTimeout;
                // stops the falling check at the ttop if you jump 
                groundedOnceCheck = false;
                _input.jump = false;
            }
            else if (_input.jumpHold && jumpTimeHold > 0) // holding jump to still jump higher
            {
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                jumpTimeHold -= 1 * Time.deltaTime;
                //print("JumpHOldBugger");
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
          //  print($"Jumping: {jumpTimeHold}, inputHold: {_input.jumpHold}, jumpHoldTime:{jumpTimeHold}");
            if (_input.jumpHold && jumpTimeHold > 0) // holding jump to still jump higher
            {
               // print("in jump hold");
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                jumpTimeHold -= 1 * Time.deltaTime;
                //  print("JumpHOldBugger");
                //print(jumpTimeHold);
                //  print(Grounded);

                return;
            }
            else
            {
               // _input.jumpHold = false;
                jumpTimeHold = 0;

            }
            // reset the jump timeout timer
            ///

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

         //   _input.jump = false;
        }
        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;


        }
        ///AddGravity();
    }

    public void AddGravity()
    {
        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
            _controller.Move(new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        }
    }

    public void CheckIfRoofed()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + roofOffsetSphere,
            transform.position.z);
        isRoofed = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        if (isRoofed)
        {
            print("roofed");
            jumpTimeHold = 0;
            coyoteJumpTime = 0;
            _verticalVelocity = -2.0f;
            StopJumpInputs();
        }
        //if (pos1 == gameObject.transform.position.y && !Grounded)
        //{
        //    print("roofed");
        //    jumpTimeHold = 0;
        //    coyoteJumpTime = 0;
        //    _verticalVelocity = -2.0f;
        //    StopJumpInputs();
        //}
        //pos1 = gameObject.transform.position.y;
    }

    // moves the player
    public void JumpForce(float multiplier)
    {
        _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity * multiplier);
    }

    public void SetSpeedToZero()
    {
        speedCurrent = 0;
        _animator.SetFloat(_animIDSpeed, 0);
        print(_animator.GetFloat(_animIDSpeed));
    }

    // stops the jump and jump hold intputs from the input manager
    public void StopJumpInputs()
    {
        _input.jump = false;
        _input.jumpHold = false;
        canJump = false;
    }


    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }


    public void DashMechanic()
    {
        // if we are suppose to be in dashing, then add force
        if (isDashing)
        {
            DashinSpeedBoost();
            return;
        }

        // if hte player isn't presing the itneract button then ignore
        if (!_input.interact)
        {
           // print($"{_input.interact} {isDashing}");
            return;
        }


        _input.interact = false;
        // only happens the first time the player presses interact
        if (!isDashing)
        {
            StartCoroutine(DashWait());
        }

        isDashing = true;


    }

    public void DashinSpeedBoost()
    {
        print("Dashing");
        // normalise input direction
        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
        _playerInput.actions["Move"].Disable();
        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (_input.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
        else
        {
            // if no input i guess (should make player dash forwards from camera
            _targetRotation = _mainCamera.transform.eulerAngles.y;
        }

        _verticalVelocity = 0;

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        canMove = false;
        // move the player
        _controller.Move(targetDirection.normalized * (dashSpeed * Time.deltaTime));
        

    }

    public IEnumerator DashWait()
    {
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        canMove = true; 
        _playerInput.actions["Move"].Enable();
        print("Stop dashing");
        // end the dash and regive player movement
    }

}