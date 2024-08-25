using UnityEngine;

namespace Utils
{
    public class ObjectToggler : MonoBehaviour
    {
        public GameObject targetObject; // 활성화/비활성화할 GameObject

        private void Update()
        {
            // ESC 키를 누르면 오브젝트 활성화 상태 토글
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (targetObject != null)
                {
                    // 현재 상태의 반대로 설정
                    targetObject.SetActive(!targetObject.activeSelf);
                }
                else
                {
                    Debug.LogWarning("Target object is not assigned!");
                }
            }
        }
    }
}