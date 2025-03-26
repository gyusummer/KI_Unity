using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform TracingTarget;
    public float Distance = 10.0f;
    public float DefaultAngle = 45.0f;
    public float VerticalSensitivity = 180.0f;
    public float HorizontalSensitivity = 180.0f;
    public float YMin = -10.0f;
    public float YMax = 90.0f;
    private float currentY = 0.0f;
    private float currentX = 0.0f;

    private void Start()
    {
        ResetCamera();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        UpdateCamera();
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            ResetCamera();
        }

        if (!Input.GetKey(KeyCode.LeftControl))
        {
            SetTargetForward();
        }
    }

    private void ResetCamera()
    {
        Vector3 direction = TracingTarget.forward * -Distance;
        Quaternion rotation = Quaternion.AngleAxis(DefaultAngle, TracingTarget.right);
        transform.position = TracingTarget.position + new Vector3(0f, 1f, 0f) + rotation * direction;
        transform.rotation = rotation * TracingTarget.rotation;

        currentX = transform.eulerAngles.x;
        currentY = transform.eulerAngles.y;
    }

    private void UpdateCamera()
    {
        currentX -= Input.GetAxis("Mouse Y") * VerticalSensitivity * Time.deltaTime;
        currentY += Input.GetAxis("Mouse X") * HorizontalSensitivity * Time.deltaTime;

        currentX = Mathf.Clamp(currentX, YMin, YMax);

        Vector3 direction = new Vector3(0.0f, 0.0f, -Distance);
        Quaternion rotation = Quaternion.Euler(currentX, currentY, 0);
        transform.position = TracingTarget.position + new Vector3(0f, 1f, 0f) + rotation * direction;

        transform.localRotation = rotation;
    }

    private void SetTargetForward()
    {
        TracingTarget.rotation = Quaternion.Euler(0.0f, currentY, 0.0f);
    }
}