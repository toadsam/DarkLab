/*using ScaryEvents;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro ���ӽ����̽� �߰�
public class SeeDetector : MonoBehaviour
{
    public float rayDistance = 0.3f;  // ������ �Ÿ�
    public LayerMask layerMask;     // ����ĳ��Ʈ�� ������ �޴� ���̾�
    public float detectionTime = 2f; // ���� �ð�
    public LayerMask ignoreLayerMask; // ������ ���̾�

    private float detectionTimer = 0f;
    private bool isSeeing = false;

    public static bool isInteraction;

    public GameObject interactionUI; // ��ȣ�ۿ� UI
    public Transform CameraTransform;

    public TextMeshProUGUI infoText; // ������Ʈ ������ ǥ���� UI �ؽ�Ʈ

    private void Start()
    {
        isInteraction = false;

        if (infoText != null)
        {
            infoText.text = ""; // �ʱ� �ؽ�Ʈ�� �� ���ڿ��� ����
        }
    }

    void Update()
    {
        // ī�޶��� �� �������� ����ĳ��Ʈ�� ��
        Ray ray = new Ray(CameraTransform.position, CameraTransform.forward);
        RaycastHit[] hits;
        Color rayColor = Color.green; // �⺻ ���� ������ �ʷϻ�

        // ��� �浹 ����
        hits = Physics.RaycastAll(ray, rayDistance, layerMask);

        bool interactionDetected = false;
        bool seeDetected = false;

        foreach (RaycastHit hit in hits)
        {
            // "See" �±� ����
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

            // "Interaction" �±� ����
            if (hit.collider.CompareTag("Interaction"))
            {
                interactionUI.SetActive(true);
                isInteraction = true;
                MainManager.Instance.objectEventHandler.targrt = hit.transform.gameObject.GetComponent<ObjectInfoHolder>();
                interactionDetected = true;
                DisplayObjectInfo(hit.transform.gameObject);
            }
        }

        // �������� ������ �ʱ�ȭ
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

        // ����� ���� �׸��� (�ʷϻ� �Ǵ� ������)
        Debug.DrawRay(CameraTransform.position, CameraTransform.forward * rayDistance, rayColor);
    }

    void TriggerSeeEvent()
    {
        MainManager.Instance.objectEventHandler.Match(MainManager.Instance.objectEventHandler.targrt);
        Debug.Log("See : 2초이상 바라보았습니다!");
        MainManager.Instance.objectEventHandler.targrt = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Proximity"))
        {
            MainManager.Instance.objectEventHandler.targrt = other.GetComponent<ObjectInfoHolder>();
            MainManager.Instance.objectEventHandler.Match(MainManager.Instance.objectEventHandler.targrt);
            Debug.Log("Proximity 지나간 물체의 이름: " + other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Proximity"))
        {
            Debug.Log("Proximity 나간 물체의 이름: " + other.gameObject.name);
        }
    }

    void DisplayObjectInfo(GameObject gameObject)
    {
       
            infoText.text = $"Object Name: + {gameObject.name}";
    }
}*/
using ScaryEvents;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeeDetector : MonoBehaviour
{
    public float rayDistance = 0.3f;  // 레이캐스트 거리
    public LayerMask layerMask;       // 탐지할 레이어
    public float detectionTime = 2f;  // 탐지 시간
    public LayerMask ignoreLayerMask; // 무시할 레이어

    public GameObject dotPrefab;      // 점을 나타낼 프리팹
    private GameObject currentDot;    // 현재 표시된 점
    private Renderer dotRenderer;     // 점의 렌더러

    private float detectionTimer = 0f;
    private bool isSeeing = false;

    public static bool isInteraction;

    public GameObject interactionUI;  // 상호작용 UI
    public Transform CameraTransform;

    public TextMeshProUGUI infoText;  // 정보 텍스트 UI

    private void Start()
    {
        isInteraction = false;

        if (infoText != null)
        {
            infoText.text = ""; // 초기 텍스트를 빈 문자열로 설정
        }

        if (dotPrefab != null)
        {
            // 점 프리팹을 인스턴스화하고 활성화
            currentDot = Instantiate(dotPrefab);
            currentDot.SetActive(true);
            dotRenderer = currentDot.GetComponent<Renderer>();
            if (dotRenderer != null)
            {
                dotRenderer.material.color = Color.green; // 초기 색상 설정
            }
        }
    }

    void Update()
    {
        // 카메라의 앞 방향으로 레이캐스트
        Ray ray = new Ray(CameraTransform.position, CameraTransform.forward);
        RaycastHit hit;
        Color rayColor = Color.green; // 기본 색상은 초록색

        // 레이캐스트 탐지
        if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
        {
            // "See" 태그 탐지
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

                // 점 색상을 빨간색으로 변경
                if (dotRenderer != null)
                {
                    dotRenderer.material.color = Color.red;
                }
            }
            else
            {
                // "See" 태그를 탐지하지 않았을 때 색상을 초록색으로 변경
                if (dotRenderer != null)
                {
                    dotRenderer.material.color = Color.green;
                }
                isSeeing = false;
                detectionTimer = 0f;
            }

            // "Interaction" 태그 탐지
            if (hit.collider.CompareTag("Interaction"))
            {
                interactionUI.SetActive(true);
                isInteraction = true;
                MainManager.Instance.objectEventHandler.targrt = hit.transform.gameObject.GetComponent<ObjectInfoHolder>();
                DisplayObjectInfo(hit.transform.gameObject);
            }
            else
            {
                interactionUI.SetActive(false);
                isInteraction = false;
            }

            // 레이캐스트 끝 지점에 점 표시
            if (currentDot != null)
            {
                currentDot.SetActive(true);
                currentDot.transform.position = hit.point;
            }
        }
        else
        {
            // 레이캐스트가 맞지 않았을 때 점을 카메라 앞쪽에 배치하고 색상 초기화
            if (currentDot != null)
            {
                currentDot.transform.position = CameraTransform.position + CameraTransform.forward * rayDistance;
                dotRenderer.material.color = Color.green;
            }
            interactionUI.SetActive(false);
            isInteraction = false;
            isSeeing = false;
            detectionTimer = 0f;
        }

        // 레이캐스트 시각화 (디버그용)
        Debug.DrawRay(CameraTransform.position, CameraTransform.forward * rayDistance, rayColor);
    }

    void TriggerSeeEvent()
    {
        MainManager.Instance.objectEventHandler.Match(MainManager.Instance.objectEventHandler.targrt);
        Debug.Log("See : 2초이상 바라보았습니다!");
        MainManager.Instance.objectEventHandler.targrt = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Proximity"))
        {
            MainManager.Instance.objectEventHandler.targrt = other.GetComponent<ObjectInfoHolder>();
            MainManager.Instance.objectEventHandler.Match(MainManager.Instance.objectEventHandler.targrt);
            Debug.Log("Proximity 지나간 물체의 이름: " + other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Proximity"))
        {
            Debug.Log("Proximity 나간 물체의 이름: " + other.gameObject.name);
        }
    }

    void DisplayObjectInfo(GameObject gameObject)
    {
        if (infoText != null)
        {
            infoText.text = $"Object Name: {gameObject.name}";
        }
    }
}
