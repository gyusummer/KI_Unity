using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Study_CharacterController : MonoBehaviour
{
    [Header("이동 설정")]
    public float walkSpeed = 2.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.2f;
    
    [Header("카메라 설정")]
    public Transform cameraTransform;
    public float mouseSensitivity = 2.0f;
    public float maxLookAngle = 60.0f;
    
    private CharacterController characterController;
    private float verticalLookRotation = 0.0f;
    private Vector3 velocity = Vector3.zero;
    private Quaternion initialCameraRotation = Quaternion.identity;
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        initialCameraRotation = cameraTransform.localRotation;
    }

    private void Update()
    {
        UpdateMovement();
        UpdateCamera();
    }

    private void UpdateMovement()
    {
        bool isGrounded = characterController.isGrounded;
        float speed = walkSpeed; //요기에 달리기가 들어가면 될겁니다
        
        Vector2 inputAxis = 
            new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        Vector3 move = transform.right * inputAxis.x + transform.forward * inputAxis.y;
        characterController.Move(move * (speed * Time.deltaTime));

        {
            //여러분이 바꿔야할 코드들
            
            if (velocity.y < 0)
            {
                velocity.y = -2f;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Jump!");
                velocity.y = jumpHeight;
            }
        }
        
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void UpdateCamera()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        
        Quaternion yawRotation = Quaternion.AngleAxis(mouseX, Vector3.up);
        transform.rotation *= yawRotation;

        verticalLookRotation += mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -maxLookAngle, maxLookAngle);
        
        Quaternion pitchRotation = Quaternion.AngleAxis(verticalLookRotation, Vector3.left);
        cameraTransform.localRotation = initialCameraRotation * pitchRotation;
    }
    
}
