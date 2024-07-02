using UnityEngine;
using Cinemachine; // Cinemachine을 사용하기 위해 추가

public class CameraPersist : MonoBehaviour
{
    public GameObject virtualCamera;
    void Awake()
    {
        // 동일한 오브젝트가 여러 개 생기지 않도록 체크
        int numOfCameras = FindObjectsOfType<CameraPersist>().Length;
        if (numOfCameras > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);

            // 가상 카메라가 자식 오브젝트로 있는 경우에 대해 처리
           
                DontDestroyOnLoad(virtualCamera);
            
        }
    }
}
