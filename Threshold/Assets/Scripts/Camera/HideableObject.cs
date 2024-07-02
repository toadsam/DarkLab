using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HideableObject : MonoBehaviour
{
    private HideableObject hideObject;
    private static Dictionary<Collider, List<HideableObject>> hideableObjectsMap = new Dictionary<Collider, List<HideableObject>>();

    [SerializeField]
    private Collider Collider = null;

    private void Start()
    {
        InitHideObject();
    }

    //저장된 정보 초기화
    public static void InitHideObject()
    {
        //이전 정보가 있다면 오브젝트를 다시 보이게 해주고 초기화
        foreach(var objList in hideableObjectsMap.Values)
        {
            foreach(var obj in objList)
            {
                if(obj != null && obj.Collider != null)
                {
                    obj.SetVisible(true);
                    obj.hideObject = null;
                }
            }
        }

        hideableObjectsMap.Clear();

        foreach (var obj in FindObjectsOfType<HideableObject>())
        {
            if(obj.Collider != null)
            {
                if (!hideableObjectsMap.ContainsKey(obj.Collider))
                    hideableObjectsMap[obj.Collider] = new List<HideableObject>();
                    
                hideableObjectsMap[obj.Collider].Add(obj);
            }
        }
    }

    //매개변수로 전달된 콜라이더에 대한 HideableObject 리스트 반환
    public static List<HideableObject> GetRootHideableCollider(Collider collider)
    {
        List<HideableObject> objList;

        if(hideableObjectsMap.TryGetValue(collider, out objList))
        {
            List<HideableObject> rootObjects = new List<HideableObject>();
            foreach(var obj in objList)
                rootObjects.Add(GetRoot(obj));

            return rootObjects;
        }
        else
            return null;
    }

    // //매개변수로 전달된 콜라이더에 대한 HideableObject 반환
    // public static HideableObject GetRootHideableCollider(Collider collider)
    // {
    //     HideableObject obj;

    //     if(hideableObjectsMap.TryGetValue(collider, out obj))
    //         return GetRoot(obj);
    //     else
    //         return null;
    // }

    //오브젝트의 루트 오브젝트 반환(계층구조 고려하여)
    private static HideableObject GetRoot(HideableObject obj)
    {
        if(obj.hideObject == null)
            return obj;
        else
            return GetRoot(obj.hideObject);
    }

    //오브젝트 렌더링 유무
    public void SetVisible(bool visible)
    {
        Renderer rend = this.GetComponent<Renderer>();
        //&& hideableObjectsMap.ContainsKey(Collider)
        if(rend != null && rend.gameObject.activeInHierarchy)
            rend.shadowCastingMode = visible ? ShadowCastingMode.On : ShadowCastingMode.ShadowsOnly;
    }
}
