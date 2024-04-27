using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraRays : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    [SerializeField] private GameObject FocusCamera;
    private RaycastHit[] ratHits;

    private Ray ray;

    public float MAX_RAY_DISTANCE = 500.0f;
    private float greenRayDuration = 0.0f;
    public float GREEN_RAY_THRESHOLD = 2.0f;

    

    private void LateUpdate()
    {
        if (PlayerInteraction.isDetect)
        {

            ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // ī�޶� ����Ʈ�� �߽ɿ��� ���� ����
            ratHits = Physics.RaycastAll(ray, MAX_RAY_DISTANCE); // ��� �浹 ������ ������
            

            foreach (RaycastHit hit in ratHits)
            {
                if (hit.collider.CompareTag("FocusObject") && (!hit.collider.gameObject.GetComponent<InteractionObject>().data.isComplete)) // �浹�� ������Ʈ�� �±װ� "add"�� ��쿡��
                {
                  
                    Debug.DrawLine(ray.origin, hit.point, Color.green); // ������ �浹 �������� �ʷϻ� ������ �׷��� (����׿�)
                    
                    
                   
                    
                        greenRayDuration += Time.deltaTime; // �ʷϻ� ����ĳ��Ʈ�� ���� �ð� ������Ʈ
                        if (greenRayDuration >= GREEN_RAY_THRESHOLD)
                        {
                    
                            ExecuteFunction(ratHits[0].point,hit.collider.gameObject); // �ʷϻ� ����ĳ��Ʈ�� ���� �ð� �̻� ���ӵǸ� �Լ� ����
                            greenRayDuration = 0.0f; // �ʷϻ� ����ĳ��Ʈ�� �������� �ʾ����Ƿ� ���� �ð� �ʱ�ȭ
                          ;
                        
                        }
                    
                   
                      
                    

                    break; // ù ��°�� �߰��� "add" �±׸� ���� ������Ʈ�� ó���ϰ� ���� ����
                }
                else
                {
                    Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
                }


            }


            void ExecuteFunction(Vector3 hitPoint, GameObject gameObject)
            {
                  Debug.Log("�ʷϻ� ����ĳ��Ʈ�� 2�� �̻� ���ӵǾ����ϴ�!");
                MainManager.Instance.objectEventHandler.Match(MainManager.Instance.objectEventHandler.targrt, scaryEventWhen.OnSustainedFocusInteraction);

                if (gameObject.GetComponent<InteractionObject>() != null)
                {
                    if (gameObject.GetComponent<InteractionObject>().data.isChage == true)
                    {
                        Debug.Log("�ٲ����");
                        MainManager.Instance.GetScore();
                        gameObject.GetComponent<InteractionObject>().data.isComplete = true;
                    }
                    else
                    {
                        Debug.Log("�� �ٲ����");
                    }
                }
                else { Debug.Log("���� �ٸ� ���̴�!!!!");
                }
                
            }
        }
    }
}
