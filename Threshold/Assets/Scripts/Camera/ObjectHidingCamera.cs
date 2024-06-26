using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHidingCamera : MonoBehaviour
{
    //가려진 물체를 확인하는 데 사용되는 둥근 캐스트의 반지름
    [SerializeField]
    private float sphereCastRadius = 1f;

    //Raycast 사용하여 반환되는 충돌 정보
    private RaycastHit[] hitBuffer = new RaycastHit[32];

    //숨길 오브젝트와 보여줄 오브젝트 정보를 담는 변수
    private List<HideableObject> hiddenObjects = new List<HideableObject>();
    private List<HideableObject> previouslyHiddenObjects = new List<HideableObject>();

    private GameObject player;
    private Transform playerTr;

    //물리 연산 후에 작동하기 위해 LateUpdate 사용
    private void LateUpdate()
    {
        RefreshHiddenObjects();
    }

    //숨겨진 오브젝트 갱신
    public void RefreshHiddenObjects()
    {
        if(player == null)
        {
            player = GameObject.FindWithTag("Player");
            if(player != null)
                playerTr = player.transform;
        }

        //플레이어 위치에서 카메라까지의 방향과 거리 계산
        Vector3 toTarget = (playerTr.position - transform.position);
        float targetDistance = toTarget.magnitude;
        Vector3 targetDirection = toTarget / targetDistance;

        //플레이어 뒤의 벽에 부딪히지 않도록 거리 조정
        targetDistance -= sphereCastRadius * 1.1f;

        //이전에 감지된 오브젝트 리스트 초기화
        hiddenObjects.Clear();

        //sphereCastRadius로 주변 오브젝트 감지
        int hitCount = Physics.SphereCastNonAlloc(transform.position,
                                                    sphereCastRadius,
                                                    targetDirection,
                                                    hitBuffer,
                                                    targetDistance,
                                                    -1,
                                                    QueryTriggerInteraction.Ignore);
        
        //숨길 수 있는 오브젝트 추가
        for(int i = 0; i < hitCount; i++)
        {
            var hit = hitBuffer[i];

            var hideableObjs = HideableObject.GetRootHideableCollider(hit.collider);

            if(hideableObjs != null)
                hiddenObjects.AddRange(hideableObjs);
                
            // var hideableObj = HideableObject.GetRootHideableCollider(hit.collider);

            // if(hideableObj != null)
            //     hiddenObjects.Add(hideableObj);
        }

        //이전에 숨겨진 오브젝트와 비교하여 숨겨야 할 오브젝트 숨기기
        foreach(var hideableObj in hiddenObjects)
        {
            if(!previouslyHiddenObjects.Contains(hideableObj))
                hideableObj.SetVisible(false);
        }

        //숨겨진 오브젝트 중 보여야 할 오브젝트 표시
        foreach(var hideableObj in previouslyHiddenObjects)
        {
            if(!hiddenObjects.Contains(hideableObj))
                hideableObj.SetVisible(true);
        }

        // //숨겨진 오브젝트 리스트와 이전에 숨겨진 오브젝트 리스트 교환
        var temp = hiddenObjects;
        hiddenObjects = previouslyHiddenObjects;
        previouslyHiddenObjects = temp;
    }
}
