using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumpForce;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;

    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true;

    private Rigidbody _rigidbody;

    public static PlayerController instance;
    private void Awake()
    {
        instance = this;
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
        Debug.Log(IsGrounded());
    }

    private void LateUpdate()
    {
        if (canLook && WakeUp.isWakeUp)
        {
            CameraLook();
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (IsGrounded())
                _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);

        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started) 
        {
            Debug.Log("아이템이 있다면 상호작용을 시작합니다");
        }
    }

    public void OnSensitivityIncrease(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            lookSensitivity += 0.05f;
            Debug.Log("현재의 감도" + lookSensitivity);
        }
        

    }

    public void OnSensitivityDecrease(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            lookSensitivity -= 0.05f;
            Debug.Log("현재의 감도" + lookSensitivity);
        }

    }

    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (Vector3.up * 0.01f) , Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f)+ (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + (transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (transform.right * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.right * 0.2f), Vector3.down);
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // 네 개의 레이 시작 위치
        Vector3[] rayOrigins = new Vector3[4]
        {
        transform.position + (transform.forward * 0.2f) + (Vector3.up * 0.01f),
        transform.position + (-transform.forward * 0.2f) + (Vector3.up * 0.01f),
        transform.position + (transform.right * 0.2f) + (Vector3.up * 0.01f),
        transform.position + (-transform.right * 0.2f) + (Vector3.up * 0.01f)
        };

        // 레이 그리기
        for (int i = 0; i < rayOrigins.Length; i++)
        {
            Gizmos.DrawRay(rayOrigins[i], Vector3.down * 0.1f);
        }
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}