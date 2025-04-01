using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.ProBuilder.MeshOperations;

public class FreeCam : MonoBehaviour
{
    public Study_NavAgent Kiwi;
    public Transform GoalIndicator;
    public List<Vector3> WayPoints;
    
    public float Speed = 5.0f;
    public float MouseSensitivity = 10.0f;
    
    private float angleX;
    private float angleY;
    
    // Start is called before the first frame update
    private void Start()
    {
        angleX = transform.eulerAngles.x;
        angleY = transform.eulerAngles.y;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            UpdateMovement();
            UpdateRotation();
        }
        
        if (Cursor.visible == true)
        {
            //Mouse 0 = Left Click
            //Mouse 1 = Right Click
            //Mouse 2 = Wheel Click
            if (Input.GetMouseButtonDown(1))
            {
                // 이동 목표지점 설정
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit,100f, LayerMask.GetMask("Ground")))
                {
                    GoalIndicator.position = hit.point;
                    if(Input.GetKey(KeyCode.LeftShift))
                    {
                        WayPoints.Add(hit.point);
                    }
                    else
                    {
                        Kiwi.SetGoal(hit.point);
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Kiwi.SetDestinations(WayPoints.ToArray());
            WayPoints.Clear();
        }
    }

    private void ToggleCursorLock()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void UpdateMovement()
    {
        Vector3 inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        inputVector = inputVector.normalized;
        
        Vector3 worldDirection = transform.TransformDirection(inputVector);
        transform.position += worldDirection * (Speed * Time.deltaTime);
    }

    private void UpdateRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity;

        angleX -= mouseY;
        angleX = Mathf.Clamp(angleX, -90f, 90f);
        
        angleY += mouseX;
        
        Quaternion xQuaternion = 
            Quaternion.AngleAxis(angleX, Vector3.right);
        Quaternion yQuaternion = 
            Quaternion.AngleAxis(angleY, Vector3.up);
        
        //서순 중요
        transform.rotation = yQuaternion * xQuaternion;
    }
}