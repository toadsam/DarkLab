using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ScaryEvents
{
    [Serializable]
    public class ObjectDependentEventWithWhen
    {
        public UnityEvent eventToTrigger;
        public scaryEventWhen whenToTrigger;
    }
    
    public class ObjectInfoHolder : MonoBehaviour
    {
        public List<Light> lightTargets = new List<Light>();
        public List<Transform> transformTargets = new List<Transform>();
        public List<AudioSource> audioTargets = new List<AudioSource>();
        public scaryEventTier objectTier;
        
        // 실행 시점에 따른 이벤트 실행을 위한 Object 종속 이벤트. 외부 접근 방지를 위해 private + SerializeField로 선언
        [SerializeField] private List<ObjectDependentEventWithWhen> objectDependentEvents = new List<ObjectDependentEventWithWhen>();
        // 실행 시점 순회 방지를 위한 Dictionary (+ Dict 는 Inspector에 노출되지 않아, HideInInspector 필요 없음.)
        public Dictionary<scaryEventWhen, UnityEvent> eventDictionary = new Dictionary<scaryEventWhen, UnityEvent>();
        
        private void Awake()
        {
            foreach (var objectDependentEvent in objectDependentEvents)
            {
                if (eventDictionary.ContainsKey(objectDependentEvent.whenToTrigger))
                {
                    UnityEvent unityEvent = eventDictionary[objectDependentEvent.whenToTrigger];
                    eventDictionary[objectDependentEvent.whenToTrigger] = unityEvent;
                }
                else
                {
                    eventDictionary[objectDependentEvent.whenToTrigger].AddListener(objectDependentEvent.eventToTrigger.Invoke);
                }
            }
        }
    }
}
