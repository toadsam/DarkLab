using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public class UnityEventFunctionWrapper : MonoBehaviour
    {
        [Header("UnityEvents")]
        public UnityEvent awakeEvent;
        public UnityEvent startEvent;
        public UnityEvent onEnableEvent;
        public UnityEvent onDisableEvent;
        public UnityEvent onDestroyEvent;
        
        [Header("UnityEvents with Delay")]
        public UnityEvent awakeEventWithDelay;
        public float awakeEventDelay = 5f;
    
        private void Awake()
        {
            awakeEvent.Invoke();
            StartCoroutine(StartWithDelay());
        }
        
        private IEnumerator StartWithDelay()
        {
            yield return new WaitForSeconds(awakeEventDelay);
            awakeEventWithDelay.Invoke();
        }
        
        private void Start()
        {
            startEvent.Invoke();
        }
    
        private void OnEnable()
        {
            onEnableEvent.Invoke();
        }
        
        private void OnDisable()
        {
            onDisableEvent.Invoke();
        }
        
        private void OnDestroy()
        {
            onDestroyEvent.Invoke();
        }
    }
}
