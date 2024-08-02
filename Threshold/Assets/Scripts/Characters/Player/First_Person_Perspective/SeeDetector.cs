using ScaryEvents;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro ���ӽ����̽� �߰�
public class SeeDetector : MonoBehaviour
{
    public float rayDistance = 1f;  // ������ �Ÿ�
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
}
