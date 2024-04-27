using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickRenderTexture : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RectTransform renderTextureUI; // Render Texture를 가지고 있는 UI 요소의 RectTransform

    [SerializeField]private Camera renderTextureCamera; // Render Texture를 렌더링하는 카메라
    [SerializeField]private RenderTexture renderTexture; // Render Texture
    [SerializeField] private GameObject FocusCamera;

    void Start()
    {
        // Render Texture를 렌더링하는 카메라를 가져옵니다.
        renderTextureCamera = GetComponentInChildren<Camera>();
        if (renderTextureCamera == null)
        {
            Debug.LogError("카메라 컴포넌트를 찾을 수 없습니다.");
        }
    }

    void OnEnable()
    {
        renderTextureCamera = GetComponentInChildren<Camera>();
        if (renderTextureCamera == null)
        {
            Debug.LogError("카메라 컴포넌트를 찾을 수 없습니다.");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 클릭된 UI 요소의 스크린 좌표를 가져옵니다.
        Vector2 screenPoint = eventData.position;

        // 스크린 좌표를 렌더 텍스처 좌표로 변환합니다.
        Vector2 normalizedPoint = new Vector2(screenPoint.x / Screen.width, screenPoint.y / Screen.height);
        Vector2 renderTextureSize = new Vector2(renderTexture.width, renderTexture.height);
        Vector2 renderTexturePoint = Vector2.Scale(normalizedPoint, renderTextureSize);

        // 스크린 좌표를 렌더 텍스처 좌표로 변환한 레이를 생성합니다.
        Ray ray = renderTextureCamera.ScreenPointToRay(renderTexturePoint);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // 클릭된 지점의 월드 좌표를 출력합니다.
            Debug.Log("클릭된 위치의 월드 좌표: " + hit.point);
            FocusCamera.transform.position = hit.point;
        }
        else
        {
            Debug.Log("레이캐스트에 의한 충돌이 없습니다.");
        }
        // // 클릭된 UI 요소의 스크린 좌표를 가져옵니다.
        // Vector2 screenPoint = eventData.position;
        //
        // Vector3 viewportPoint = new Vector3(screenPoint.x / Screen.width, 1 - (screenPoint.y / Screen.height), 0);//new Vector3(screenPoint.x / Screen.width, screenPoint.y / Screen.height, 0);
        //
        // // 스크린 좌표를 월드 좌표로 변환합니다.
        // Ray ray = renderTextureCamera.ScreenPointToRay(viewportPoint);
        // RaycastHit hit;
        // if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        // {
        //     // 클릭된 지점의 월드 좌표를 출력합니다.
        //     Debug.Log("클릭된 위치의 월드 좌표: " + hit.point);
        //     FocusCamera.transform.position = hit.point;
        // }
        // else
        // {
        //     Debug.Log("레이캐스트에 의한 충돌이 없습니다.");
        // }
    }
}