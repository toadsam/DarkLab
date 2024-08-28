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
    public bool playerEventOff = true;

    private Rigidbody _rigidbody;
    private AudioSource _audioSource;

    public AudioClip footstepClip;
    public float footstepInterval = 0.5f;
    private float footstepTimer;
    

    public static PlayerController instance;
    private void Awake()
    {
        instance = this;
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        footstepTimer = footstepInterval;
    }

    private void FixedUpdate()
    {
        if(playerEventOff && WakeUp.isWakeUp)
            Move();
      
    }

    private void LateUpdate()
    {   
        //&& WakeUp.isWakeUp
        if (canLook && !GrandMaMove.isGrandEvent && WakeUp.isWakeUp && playerEventOff)
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

        HandleFootsteps();
    }

    void HandleFootsteps()
    {
        if (curMovementInput != Vector2.zero)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0)
            {
                _audioSource.PlayOneShot(footstepClip);
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0;
        }
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
        if (context.phase == InputActionPhase.Started && SeeDetector.isInteraction)
        {
           
            if (MainManager.Instance != null && MainManager.Instance.objectEventHandler != null)
            {
                var target = MainManager.Instance.objectEventHandler.targrt;
                if (target != null)
                {
                    Debug.Log("여기는 들어오니");
                    MainManager.Instance.objectEventHandler.Match(target);
                }
                else
                {
                    Debug.LogWarning("Target is null in OnInteraction.");
                }
            }
            else
            {
                Debug.LogWarning("MainManager or objectEventHandler is null in OnInteraction.");
            }
        }
    }

    public void OnSensitivityIncrease(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            lookSensitivity += 0.05f;
            Debug.Log("������ ����" + lookSensitivity);
        }
        

    }

    public void OnSensitivityDecrease(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            lookSensitivity -= 0.05f;
            Debug.Log("������ ����" + lookSensitivity);
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

        // �� ���� ���� ���� ��ġ
        Vector3[] rayOrigins = new Vector3[4]
        {
        transform.position + (transform.forward * 0.2f) + (Vector3.up * 0.01f),
        transform.position + (-transform.forward * 0.2f) + (Vector3.up * 0.01f),
        transform.position + (transform.right * 0.2f) + (Vector3.up * 0.01f),
        transform.position + (-transform.right * 0.2f) + (Vector3.up * 0.01f)
        };

        // ���� �׸���
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