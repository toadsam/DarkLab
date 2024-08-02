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
                Match(startTargets[i]);
        }
   
        public void Match(ObjectInfoHolder objectInfoHolder) 
        {
            if (objectInfoHolder.isObjectDependentEvent)
            {
                objectInfoHolder.objectDependentEvents.Invoke();
                objectInfoHolder.DisableObjectDependentEvents();
                return;
            }

            List<ScaryEvent> matchingEvents = new List<ScaryEvent>();

            for (int i = 0; i < scaryEvents.Length; i++)
            {
                if (objectInfoHolder.objectTier.HasFlag(scaryEvents[i].metaData.tier))
                {
                    matchingEvents.Add(scaryEvents[i]);
                }
            }

            // 일치하는 이벤트가 있는 경우, 무작위로 선택하여 실행
            if (matchingEvents.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, matchingEvents.Count);
                ScaryEvent selectedEvent = matchingEvents[randomIndex];

                selectedEvent.currentEventTarget = objectInfoHolder;
                Debug.Log(selectedEvent.currentEventTarget);
                selectedEvent.ResetIndexForTargets();
                selectedEvent.StartEvent();
                
                // 오브젝트 종속 이벤트 실행
                objectInfoHolder.objectDependentEvents.Invoke();
                
                // 실행된 이벤트를 playedObjectInfoHolders에 추가하고 이벤트 비활성화
                playedObjectInfoHolders.Add(objectInfoHolder);
                objectInfoHolder.DisableObjectDependentEvents();
                
                // 매치된 event 의 tier 만큼의 damage 를 Player 에게 줌 (ProgressChecker.Instance.TakeDamage에서 처리)
                if (selectedEvent.metaData.tier.HasFlag(scaryEventTier.Tier5))
                {
                    ProgressChecker.Instance.TakeDamage(0.5f);
                }
                else if (selectedEvent.metaData.tier.HasFlag(scaryEventTier.Tier5))
                {
                    ProgressChecker.Instance.TakeDamage(0.4f);
                }
                else if (selectedEvent.metaData.tier.HasFlag(scaryEventTier.Tier3))
                {
                    ProgressChecker.Instance.TakeDamage(0.3f);
                }
                else if (selectedEvent.metaData.tier.HasFlag(scaryEventTier.Tier2))
                {
                    ProgressChecker.Instance.TakeDamage(0.2f);
                }
                else if (selectedEvent.metaData.tier.HasFlag(scaryEventTier.Tier1))
                {
                    ProgressChecker.Instance.TakeDamage(0.1f);
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
