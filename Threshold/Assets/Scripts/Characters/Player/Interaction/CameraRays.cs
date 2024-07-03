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

            ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); 
            ratHits = Physics.RaycastAll(ray, MAX_RAY_DISTANCE); 
            

            foreach (RaycastHit hit in ratHits)
            {
                if (hit.collider.CompareTag("FocusObject") && (!hit.collider.gameObject.GetComponent<InteractionObject>().data.isComplete)) 
                {
                  
                    Debug.DrawLine(ray.origin, hit.point, Color.green); 
                    
                    
                   
                    
                        greenRayDuration += Time.deltaTime; 
                        if (greenRayDuration >= GREEN_RAY_THRESHOLD)
                        {
                    
                            ExecuteFunction(ratHits[0].point,hit.collider.gameObject); 
                            greenRayDuration = 0.0f; 
                          ;
                        
                        }
                    
                   
                      
                    

                    break;
                }
                else
                {
                    Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
                }


            }


            void ExecuteFunction(Vector3 hitPoint, GameObject gameObject)
            {
                  
                MainManager.Instance.objectEventHandler.Match(MainManager.Instance.objectEventHandler.targrt, scaryEventWhen.OnSustainedFocusInteraction);

                if (gameObject.GetComponent<InteractionObject>() != null)
                {
                    if (gameObject.GetComponent<InteractionObject>().data.isChage == true)
                    {
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
