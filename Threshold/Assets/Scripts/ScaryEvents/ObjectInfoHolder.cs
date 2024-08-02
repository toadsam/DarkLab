using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ScaryEvents
{
    public class ObjectInfoHolder : MonoBehaviour
    {
        public List<Light> lightTargets = new List<Light>();
        public List<Transform> transformTargets = new List<Transform>();
        public List<Renderer> rendererTargets = new List<Renderer>(); 
        
        [Space(10)]
        public scaryEventTier objectTier;
        
        // 실행 시점에 따른 이벤트 실행을 위한 Object 종속 이벤트. 외부 접근 방지를 위해 private + SerializeField로 선언
        public bool isObjectDependentEvent = false;
        public UnityEvent objectDependentEvents = new UnityEvent();
        // 실행 시점 순회 방지를 위한 Dictionary (+ Dict 는 Inspector에 노출되지 않아, HideInInspector 필요 없음.)
        
        private void Awake()
        {
            gameObject.layer = LayerMask.NameToLayer("NotCollideToOther");
            if (gameObject.CompareTag("Proximity") && GetComponent<Collider>() != null)
            {
                GetComponent<Collider>().isTrigger = true;
            }
        }
        
        public void DisableObjectDependentEvents()
        {
            if (GetComponent<Collider>() != null)
            {
                GetComponent<Collider>().enabled = false;
               MainManager.Instance.randomObjectSelector.CheckAndExecuteEvent();
            }
        }
        
        public void EnableObjectDependentEvents()
        {
            if (GetComponent<Collider>() != null)
            {
                GetComponent<Collider>().enabled = true;
            }
        }
    }
}
