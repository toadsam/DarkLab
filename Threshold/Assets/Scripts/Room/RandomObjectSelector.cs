using System;
using UnityEngine;

namespace ScaryEvents
{
    public class RandomObjectSelector : MonoBehaviour
    {
        private ObjectInfoHolder[] objectInfoHolders;
        private ObjectInfoHolder selectedObject;
        private System.Random random = new System.Random(); // 랜덤 인스턴스 생성

        private void Start()
        {
            // 활성화 되어 있는 모든 ObjectInfoHolder 스크립트를 가진 오브젝트들을 배열에 저장
            objectInfoHolders = FindObjectsOfType<ObjectInfoHolder>();

            // 배열이 비어있지 않다면
            if (objectInfoHolders.Length > 0)
            {
                // 랜덤으로 하나 선택
                int randomIndex = random.Next(objectInfoHolders.Length);
                selectedObject = objectInfoHolders[randomIndex];

                Debug.Log($"Selected object: {selectedObject.name}");

              //  PrintAllObjectInfoHolders();
            }
        }

        public void ResetSelectedObject()
        {
            Debug.Log("맵이 활성화 되었습니다.");
            // 활성화 되어 있는 모든 ObjectInfoHolder 스크립트를 가진 오브젝트들을 배열에 저장
            objectInfoHolders = FindObjectsOfType<ObjectInfoHolder>();

            // 배열이 비어있지 않다면
            if (objectInfoHolders.Length > 0)
            {
                // 랜덤으로 하나 선택
                int randomIndex = random.Next(objectInfoHolders.Length);
                selectedObject = objectInfoHolders[randomIndex];

                Debug.Log($"Selected object: {selectedObject.name}");

                //  PrintAllObjectInfoHolders();
            }

        }

        public void CheckAndExecuteEvent()
        {
            if (selectedObject != null)
            {
                Collider collider = selectedObject.GetComponent<Collider>();
                if (collider != null && !collider.enabled)
                {
                    MainManager.Instance.interactableDoor.isLocked = false;
                    Debug.Log("완료되었습니다.");
                  //  selectedObject.TriggerEvent();
                }
                else
                {
                    Debug.Log($"Selected object: {selectedObject.name} collider is still enabled.");
                }
            }
        }

        private void PrintAllObjectInfoHolders()
        {
            Debug.Log("All ObjectInfoHolders:");
            foreach (var obj in objectInfoHolders)
            {
                Debug.Log(obj.name);
            }
        }
    }
}
