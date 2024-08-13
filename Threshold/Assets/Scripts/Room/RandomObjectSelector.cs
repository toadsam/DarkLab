using System;
using System.Collections;
using UnityEngine;
using TMPro;

namespace ScaryEvents
{
    public class RandomObjectSelector : MonoBehaviour
    {
        public AudioSource doorOpenAudio;
        public TextMeshProUGUI doorOpenText;

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
                    StartCoroutine(StartFadeCoroutine());
                    //selectedObject.TriggerEvent();
                }
                else
                {
                    Debug.Log($"Selected object: {selectedObject.name} collider is still enabled.");
                }
            }
        }

        IEnumerator StartFadeCoroutine()
        {
            doorOpenText.gameObject.SetActive(true);

            float fadeCount = 0;
            Color originalColor = doorOpenText.color;

            // Fade in
            while (fadeCount < 1.0f)
            {
                fadeCount += 0.01f;
                yield return new WaitForSeconds(0.01f);
                doorOpenText.color = new Color(originalColor.r, originalColor.g, originalColor.b, fadeCount);
            }

            yield return new WaitForSeconds(0.3f);

            fadeCount = 1.0f;

            // Fade out
            while (fadeCount > 0.0f)
            {
                fadeCount -= 0.01f;
                yield return new WaitForSeconds(0.01f);
                doorOpenText.color = new Color(originalColor.r, originalColor.g, originalColor.b, fadeCount);
            }

            doorOpenText.gameObject.SetActive(false);
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
