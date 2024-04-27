using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RenderTextureUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]private Camera renderTextureCamera; // Render Texture를 렌더링하는 카메라
    public bool focusInteraction;

    // Start is called before the first frame update
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!focusInteraction)
        {
            // 클릭된 UI 요소의 스크린 좌표를 가져옵니다.
            Vector2 screenPoint = eventData.position;

            Debug.Log("스크린 좌표: " + screenPoint);

            // 스크린 좌표를 월드 좌표로 변환합니다.
            Ray ray = renderTextureCamera.ScreenPointToRay(screenPoint);
            RaycastHit hit;

            Debug.Log("ray 크기" + ray);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                renderTextureCamera.transform.position = hit.point;
                renderTextureCamera.orthographicSize = 0.5f;
                focusInteraction = true;
                // 클릭된 지점의 월드 좌표를 출력합니다.
                Debug.Log("클릭된 위치의 월드 좌표: " + hit.point);
            }
            else
            {
                Debug.Log("레이캐스트에 의한 충돌이 없습니다.");
            }
        }
    }
}
