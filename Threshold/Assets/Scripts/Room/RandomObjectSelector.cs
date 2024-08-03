using System;
using UnityEngine;

namespace ScaryEvents
{
    public class RandomObjectSelector : MonoBehaviour
    {
        public AudioSource doorOpenAudio;

        private ObjectInfoHolder[] objectInfoHolders;
        private ObjectInfoHolder selectedObject;
        private System.Random random = new System.Random(); // ���� �ν��Ͻ� ����

        private void Start()
        {
            // Ȱ��ȭ �Ǿ� �ִ� ��� ObjectInfoHolder ��ũ��Ʈ�� ���� ������Ʈ���� �迭�� ����
            objectInfoHolders = FindObjectsOfType<ObjectInfoHolder>();

            // �迭�� ������� �ʴٸ�
            if (objectInfoHolders.Length > 0)
            {
                // �������� �ϳ� ����
                int randomIndex = random.Next(objectInfoHolders.Length);
                selectedObject = objectInfoHolders[randomIndex];

                Debug.Log($"Selected object: {selectedObject.name}");

              //  PrintAllObjectInfoHolders();
            }
        }

        public void ResetSelectedObject()
        {
            Debug.Log("���� Ȱ��ȭ �Ǿ����ϴ�.");
            // Ȱ��ȭ �Ǿ� �ִ� ��� ObjectInfoHolder ��ũ��Ʈ�� ���� ������Ʈ���� �迭�� ����
            objectInfoHolders = FindObjectsOfType<ObjectInfoHolder>();

            // �迭�� ������� �ʴٸ�
            if (objectInfoHolders.Length > 0)
            {
                // �������� �ϳ� ����
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
                    doorOpenAudio.Play();
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
