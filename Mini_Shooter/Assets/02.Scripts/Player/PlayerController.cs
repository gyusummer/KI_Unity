using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private static readonly int FIRE = Animator.StringToHash("Fire");
    private static readonly int SWAP = Animator.StringToHash("Swap");
    private static readonly int RELOAD = Animator.StringToHash("Reload");

    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;
    
    [Header("Camera Settings")]
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f;
    
    // 바닥 체크 관련
    [Header("Ground Check")]
    public float groundCheckDistance = 0.1f;
    public LayerMask groundMask = ~0; // 기본은 모든 레이어
    
    [Header("Animator")]
    public Animator animator;
    
    private CharacterController controller;
    private Vector3 velocity;
    private float verticalLookRotation = 0f;
    private Quaternion initialCameraRotation;

    [SerializeField] private AnimStateEventListener characterAnimatorListener;
     
    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        initialCameraRotation = cameraTransform.localRotation;
    }

    private void OnEnable()
    {
        Player.LocalPlayer.Events.OnStartChangedWeapon += PlayWeaponChangeAnimation;
        Player.LocalPlayer.Events.OnStartedReload += PlayReloadAnimation;
    }

    private void OnDisable()
    {
        Player.LocalPlayer.Events.OnStartChangedWeapon -= PlayWeaponChangeAnimation;
        Player.LocalPlayer.Events.OnEndedReload -= PlayReloadAnimation;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateMovement();
        UpdateCameraRotation();
        UpdateInput();
    }

    private void UpdateMovement()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * (speed * Time.deltaTime));

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        bool isGrounded = controller.isGrounded;
        //if (isGrounded == false) isGrounded = IsGroundedBySpherecast();
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void UpdateCameraRotation()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        Quaternion yawRotation = Quaternion.AngleAxis(mouseX, Vector3.up);
        transform.rotation *= yawRotation;

        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -maxLookAngle, maxLookAngle);

        Quaternion pitchRotation = Quaternion.AngleAxis(verticalLookRotation, Vector3.right);
        cameraTransform.localRotation = initialCameraRotation * pitchRotation;
    }

    private void UpdateInput()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            bool successFire = Player.LocalPlayer.CurrentWeapon.Fire();

            if (successFire)
            {
                //AnyState를 이용할 생각임.
                animator.ResetTrigger(FIRE);
                animator.SetTrigger(FIRE);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger(RELOAD);
        }
    }

    private bool IsGroundedBySpherecast()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f; // 약간 위에서 시작
        float rayLength = (controller.height / 2) + groundCheckDistance;

        return Physics.SphereCast(origin, controller.radius * 0.9f, Vector3.down, out _, rayLength, groundMask);
    }
    
    private void OnDrawGizmos()
    {
        if (controller == null) return;

        Gizmos.color = IsGroundedBySpherecast() ? Color.green : Color.red;

        Vector3 origin = transform.position + Vector3.up * 0.1f;
        float rayLength = (controller.height / 2) + groundCheckDistance;
        Gizmos.DrawWireSphere(origin + Vector3.down * rayLength, controller.radius * 0.9f);
    }

    private void PlayWeaponChangeAnimation()
    {
        animator.ResetTrigger(SWAP);
        animator.SetTrigger(SWAP);
    }
    
    private void PlayReloadAnimation()
    {
        animator.ResetTrigger(RELOAD);
        animator.SetTrigger(RELOAD);
    }
    
}
