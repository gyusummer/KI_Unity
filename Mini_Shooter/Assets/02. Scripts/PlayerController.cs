using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly int FIRE = Animator.StringToHash("Fire");
    private static readonly int SWAP = Animator.StringToHash("Swap");

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

    [Header("Gun")]
    public Weapon[] weapons = new Weapon[4];
    public Weapon CurrentWeapon { get; private set; }
    
    private CharacterController controller;
    private Vector3 velocity;
    private float verticalLookRotation = 0f;
    private Quaternion initialCameraRotation;

     
    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        initialCameraRotation = cameraTransform.localRotation;

        for (int i = 0; i < weapons.Length; i++)
        {
            if(weapons[i] is not null)
            {
                weapons[i].gameObject.SetActive(false);
            }
        }
        
        animator.ResetTrigger(SWAP);
        animator.SetTrigger(SWAP);

        CurrentWeapon = weapons[0];
        CurrentWeapon.gameObject.SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateMovement();
        UpdateCameraRotation();
        UpdateInput();
    }

    private void UpdateInput()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.shortNameHash == SWAP || animator.IsInTransition(0)) return;
            bool successFire = CurrentWeapon.Fire();
            
            if (successFire)
            {
                animator.ResetTrigger(FIRE);
                animator.SetTrigger(FIRE);
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwapWeapon(0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwapWeapon(1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwapWeapon(2);
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwapWeapon(3);
        }
    }

    private void SwapWeapon(int weaponIndex)
    {
        if (weapons[weaponIndex] is null || CurrentWeapon == weapons[weaponIndex]) return;
        CurrentWeapon.gameObject.SetActive(false);
        animator.ResetTrigger(SWAP);
        animator.SetTrigger(SWAP);
        CurrentWeapon = weapons[weaponIndex];
        CurrentWeapon.gameObject.SetActive(true);
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
        if (isGrounded == false) isGrounded = IsGroundedBySpherecast();
        
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
    
}
