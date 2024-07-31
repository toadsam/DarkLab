using ScaryEvents;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 네임스페이스 추가
public class SeeDetector : MonoBehaviour
{
    public float rayDistance = 1f;  // 레이의 거리
    public LayerMask layerMask;     // 레이캐스트에 영향을 받는 레이어
    public float detectionTime = 2f; // 감지 시간
    public LayerMask ignoreLayerMask; // 무시할 레이어

    private float detectionTimer = 0f;
    private bool isSeeing = false;

    public static bool isInteraction;

    public GameObject interactionUI; // 상호작용 UI
    public Transform CameraTransform;

    public TextMeshProUGUI infoText; // 오브젝트 정보를 표시할 UI 텍스트

    private void Start()
    {
        isInteraction = false;

        if (infoText != null)
        {
            infoText.text = ""; // 초기 텍스트를 빈 문자열로 설정
        }
    }

    void Update()
    {
        // 카메라의 앞 방향으로 레이캐스트를 쏨
        Ray ray = new Ray(CameraTransform.position, CameraTransform.forward);
        RaycastHit[] hits;
        Color rayColor = Color.green; // 기본 레이 색상은 초록색

        // 모든 충돌 감지
        hits = Physics.RaycastAll(ray, rayDistance, layerMask);

        bool interactionDetected = false;
        bool seeDetected = false;

        foreach (RaycastHit hit in hits)
        {
            // "See" 태그 감지
            if (hit.collider.CompareTag("See"))
            {
                if (!isSeeing)
                {
                    isSeeing = true;
                    detectionTimer = 0f;
                    MainManager.Instance.objectEventHandler.targrt = null;
                }
                else
                {
                    detectionTimer += Time.deltaTime;

                    if (detectionTimer >= detectionTime)
                    {
                        Debug.Log(hit.transform.gameObject.name);
                        MainManager.Instance.objectEventHandler.targrt = hit.transform.gameObject.GetComponent<ObjectInfoHolder>();
                        TriggerSeeEvent();
                        detectionTimer = 0f;
                    }
                }

                rayColor = Color.red;
                seeDetected = true;
            }

            // "Interaction" 태그 감지
            if (hit.collider.CompareTag("Interaction"))
            {
                interactionUI.SetActive(true);
                isInteraction = true;
                MainManager.Instance.objectEventHandler.targrt = hit.transform.gameObject.GetComponent<ObjectInfoHolder>();
                interactionDetected = true;
                DisplayObjectInfo(hit.transform.gameObject);
            }
        }

        // 감지되지 않으면 초기화
        if (!seeDetected)
        {
            isSeeing = false;
            detectionTimer = 0f;
           // MainManager.Instance.objectEventHandler.targrt = null;
        }

        if (!interactionDetected)
        {
            interactionUI.SetActive(false);
            isInteraction = false;
           // MainManager.Instance.objectEventHandler.targrt = null;
        }

        // 디버그 라인 그리기 (초록색 또는 빨간색)
        Debug.DrawRay(CameraTransform.position, CameraTransform.forward * rayDistance, rayColor);
    }

    void TriggerSeeEvent()
    {
        MainManager.Instance.objectEventHandler.Match(MainManager.Instance.objectEventHandler.targrt, scaryEventWhen.OnFocusInteractionStart);
        Debug.Log("See 태그의 오브젝트를 2초 동안 감지했습니다!");
        MainManager.Instance.objectEventHandler.targrt = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Proximity"))
        {
            MainManager.Instance.objectEventHandler.targrt = other.GetComponent<ObjectInfoHolder>();
            MainManager.Instance.objectEventHandler.Match(MainManager.Instance.objectEventHandler.targrt, scaryEventWhen.OnFocusInteractionStart);
            Debug.Log("Proximity 오브젝트에 가까이 감지됨: " + other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Proximity"))
        {
            Debug.Log("Proximity 오브젝트에서 멀어짐: " + other.gameObject.name);
        }
    }

    void DisplayObjectInfo(GameObject gameObject)
    {
       
            infoText.text = $"Object Name: + {gameObject.name}";
    }
}
