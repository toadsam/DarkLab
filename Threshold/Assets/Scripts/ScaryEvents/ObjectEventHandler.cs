using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScaryEvents
{
    public class ObjectEventHandler : MonoBehaviour
    {
        public ScaryEvent[] scaryEvents;
        [SerializeField] private ObjectInfoHolder[] startTargets;
        public ObjectInfoHolder targrt;
        
        private List<ObjectInfoHolder> playedObjectInfoHolders = new List<ObjectInfoHolder>();

        // Setup for scary events
        private void Awake()
        {
            scaryEvents = FindObjectsOfType<ScaryEvent>();
            
        }

        // 방이 바뀌는 경우를 염두. (하나의 씬에서 다른 방으로 바뀔 수 있음 - 구역이 바뀌면, 다른 ObjectInfoHolder[]를 받아와야 함)
        // 현재 구역별로 바뀌므로, 전체 ObjectInfoHolder 는 다른 곳에서 관리해야 할 것으로 보임.
        public void ChangeRoom(ObjectInfoHolder[] inputObjectInfoHolders)
        {
            startTargets = inputObjectInfoHolders;

            foreach (var objectInfoHolder in playedObjectInfoHolders)
            {
                objectInfoHolder.EnableObjectDependentEvents();
            }
        }

        public void AwakeRoom()
        {
            foreach (var objectInfoHolder in playedObjectInfoHolders)
            {
                objectInfoHolder.EnableObjectDependentEvents();
            }
            
            for (int i = 0; i <startTargets.Length; i++)              
                Match(startTargets[i], scaryEventWhen.OnAwake);
        }
   
        public void Match(ObjectInfoHolder objectInfoHolder,scaryEventWhen eventWhen) 
        {

            for (int i = 0; i < scaryEvents.Length; i++)
            {
                if (objectInfoHolder.objectTier.HasFlag(scaryEvents[i].scaryEventTier) && eventWhen  == scaryEvents[i].scaryEventWhen)
                {
                    scaryEvents[i].currentEventTarget = objectInfoHolder;
                    //if (scaryEvents[i].currentEventTarget != null)
                    Debug.Log(scaryEvents[i].currentEventTarget);
                    scaryEvents[i].ResetIndexForTargets();
                    //  Debug.Log(objectInfoHolder.name);
                    scaryEvents[i].StartEvent();
                    
                    // 모든 공포 이벤트 (Awake 포함) 은 Match 를 경유하므로, 공포 이벤트의 실행 시점을 공유하는 오브젝트 종속 공포 이벤트 또한 여기서 실행.
                    objectInfoHolder.eventDictionary[eventWhen].Invoke();
                    
                    // 공포이벤트 비활성화
                    playedObjectInfoHolders.Add(objectInfoHolder);
                    objectInfoHolder.DisableObjectDependentEvents();
                    
                    break;
                }
            }
        }

        #region Test Code

        // 기존 코드와 동일한 동작을 위한 테스트 코드
        private void Start()
        {
            AwakeRoom();
        }

        #endregion
    }
}
