using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject CubePrefab;
    
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
    
    private CharacterController controller;
    private Vector3 velocity;
    private float verticalLookRotation = 0f;
    private Quaternion initialCameraRotation;

    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        
        
        initialCameraRotation = cameraTransform.localRotation;
    }

    // Update is called once per frame
    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, groundCheckDistance, groundMask))
            {
                Vector3 randEuler = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                Instantiate(CubePrefab, hit.point, Quaternion.Euler(randEuler));
            }
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
}
