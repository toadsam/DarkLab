using System;
using System.Collections;
using System.Collections.Generic;
using ScaryEvents;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class ExtensionMethods
{
	public static void AddListener (this EventTrigger trigger, EventTriggerType eventType, System.Action<PointerEventData> listener)
	{
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = eventType;
		entry.callback.AddListener(data => listener.Invoke((PointerEventData)data));
		trigger.triggers.Add(entry);
	}
}

public class PlayerInteraction : MonoBehaviour
{
    public PlayerInputActions InputActions { get; private set; }
    [SerializeField] private GameObject interactionText;
    [SerializeField] private GameObject objectCamera;
    [SerializeField] private GameObject cameraUI;
    private Transform cameraPosition;
    private Camera cameraObj;

    //focus 상호작용 관련 변수
    private bool focusInteraction;
    private Vector3 minBound;
    private Vector3 maxBound;
    private Vector3 originalCameraPosition;
    private RenderTexture renderTexture;
   // private ObjectInfoHolder targrt;
    
    public static bool isDetect;

    private void Awake()
    {
       // MainManager.Instance.objectEventHandler.targrt = null;     
        InputActions = new PlayerInputActions();
        InputActions.Player.Exit.performed += ctx => OnEscapePressed();

     //   cameraObj = objectCamera.GetComponent<Camera>();  일단 여기 부분은 주석처리한다.
      //  renderTexture = objectCamera.GetComponent<Camera>().targetTexture;

       // interactionText.GetComponent<Button>().onClick.AddListener(OnInteraction);
        //cameraUI.GetComponent<EventTrigger>().AddListener(EventTriggerType.PointerClick, OnClick);

        isDetect = false;
    }
    
    void Update()
    {
       // if(cameraObj.orthographicSize == 0.5f)
        ///    MovingCamera(); //일단 움직이는 것만 구현하기 위해서
    }

    private void OnEnable()
    {
        // Input 시스템 활성화
        InputActions.Enable();
    }
    
    private void OnDisable()
    {
        // Input 시스템 비활성화
        InputActions.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 상호작용 가능한 물체의 태그를 확인
        if (other.CompareTag("Interaction"))
        {
            if (other.transform.childCount > 0)
            {
                
                cameraPosition = other.transform.GetChild(0);
                // 가져온 첫 번째 자식에 대한 작업을 수행
            }
            else
            {
                Debug.Log("없습니다");
                return;
            }
            MainManager.Instance.objectEventHandler.targrt = other.GetComponent<ObjectInfoHolder>();
            MainManager.Instance.objectEventHandler.Match(MainManager.Instance.objectEventHandler.targrt, scaryEventWhen.OnProximity);
            // UI 텍스트를 활성화하여 상호작용 가능 문구를 표시
            interactionText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 물체와의 충돌이 종료될 때 UI 텍스트 비활성화
       // MainManager.Instance.objectEventHandler.targrt = null; 일단은 여기 부분 주석처리! 방생성 때 오류를 방지하기 위해서!
       // interactionText.gameObject.SetActive(false);
    }

    
    public void OnInteraction()
    {
        Debug.Log("나 누르는 중");
        interactionText.gameObject.SetActive(false);
        StartInteraction();
    }

    private void StartInteraction()
    {
        if(cameraPosition != null)
        {
            MainManager.Instance.objectEventHandler.Match(MainManager.Instance.objectEventHandler.targrt, scaryEventWhen.OnViewInteractionStart);
            objectCamera.transform.position = cameraPosition.position;
            objectCamera.transform.rotation = cameraPosition.rotation;
            originalCameraPosition = objectCamera.transform.position;
            cameraUI.SetActive(true);
            Player.isMoving = false;

            float halfOrthographicSize = cameraObj.orthographicSize / 2f;
            minBound = new Vector3(originalCameraPosition.x - halfOrthographicSize, originalCameraPosition.y - halfOrthographicSize, originalCameraPosition.z - halfOrthographicSize);
            maxBound = new Vector3(originalCameraPosition.x + halfOrthographicSize, originalCameraPosition.y + halfOrthographicSize, originalCameraPosition.z + halfOrthographicSize);
        }
        

        //여기 부분에 위치를 카메라의 위치를 받고 카메라를 옮긴다.

    }

    private void MovingCamera()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement;

        if (Quaternion.Euler(90, 0, 0).Equals(objectCamera.transform.rotation))
            movement = new Vector3(horizontalInput, 0f, verticalInput) * 2f * Time.deltaTime;
        else
            movement = new Vector3(horizontalInput, verticalInput, 0f) * 2f * Time.deltaTime;

        // 월드 좌표 기준으로 이동
        movement = objectCamera.transform.TransformDirection(movement);

        // 새로운 위치 계산
        Vector3 newPosition = objectCamera.transform.position + movement;

        // 이동한 위치를 특정 구역 내에 제한
        newPosition.x = Mathf.Clamp(newPosition.x, minBound.x, maxBound.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBound.y, maxBound.y);
        newPosition.z = Mathf.Clamp(newPosition.z, minBound.z, maxBound.z);

        // 실제로 이동
        objectCamera.transform.position = newPosition;
    }

    //Focus 상호작용 함수
    private void OnClick(PointerEventData eventData)
    {
        if(!focusInteraction)
        {
            isDetect = true;
            MainManager.Instance.objectEventHandler.Match(MainManager.Instance.objectEventHandler.targrt, scaryEventWhen.OnFocusInteractionStart);
            Camera.main.GetComponent<Camera>().GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>().renderPostProcessing = true;
            
            Vector2 screenPoint = eventData.position;

            Vector2 normalizedPoint = new Vector2(screenPoint.x / Screen.width, screenPoint.y / Screen.height);
            Vector2 renderTextureSize = new Vector2(renderTexture.width, renderTexture.height);
            Vector2 renderTexturePoint = Vector2.Scale(normalizedPoint, renderTextureSize);

            Ray ray = cameraObj.ScreenPointToRay(renderTexturePoint);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                cameraObj.transform.position = hit.point;

                cameraObj.orthographicSize = 0.5f;
                focusInteraction = true;
            }
            else
            {
                Debug.Log("레이캐스트에 의한 충돌이 없습니다.");
            }
        }
    }

    private void OnEscapePressed()
    {
        Debug.Log("ESC 키가 눌렸습니다.");
        if(cameraPosition != null)
        {
            cameraUI.SetActive(false);
            cameraObj.orthographicSize = 1f;
            focusInteraction = false;
            Player.isMoving = true;
            isDetect = false;
            Camera.main.GetComponent<Camera>().GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>().renderPostProcessing = false;
            cameraObj.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>().renderPostProcessing = false;
        }
        // ESC 키가 눌렸을 때 수행되어야 하는 동작을 여기에 추가합니다.
    }
}
