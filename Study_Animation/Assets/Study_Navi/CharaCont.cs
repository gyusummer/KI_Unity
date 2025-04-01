using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaCont : MonoBehaviour
{
    [Header("Move Settings")]
    public float walkSpeed;
    public float gravitationalAcceleration = -9.81f;

    [Header("Camera Settings")]
    public Transform cameraTransform;
    public float mouseSensitivity;
    public float maxCameraAngle;
    public float minCameraAngle;
    
    private CharacterController characterController;
    private float verticalLookRotation = 0.0f;
    private Vector3 gravity = Vector3.zero;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        UpdateMovement();
        UpdateCamera();
    }

    private void UpdateMovement()
    {
        float speed = walkSpeed;
        Vector2 inputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 movement = inputAxis.x * transform.right + inputAxis.y * transform.forward;
        characterController.Move(movement * (speed * Time.deltaTime));

        gravity.y = gravitationalAcceleration;
        characterController.Move(gravity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Jump()
    {
        
    }

    private void UpdateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        // Debug.Log($"Get X {Input.GetAxis("Mouse X")}/ Raw X {Input.GetAxisRaw("Mouse X")}");
        // Debug.Log($"Get Y {Input.GetAxis("Mouse Y")}/ Raw Y {Input.GetAxisRaw("Mouse X")}");
        
        Quaternion yawRotation = Quaternion.AngleAxis(mouseX, Vector3.up);
        transform.rotation *= yawRotation;
        
        Quaternion pitchRotation = Quaternion.AngleAxis(mouseY, Vector3.left);
        cameraTransform.localRotation *= pitchRotation;
    }
}
