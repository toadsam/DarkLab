using UnityEngine;

[CreateAssetMenu(fileName = "InteractionObject", menuName = "New InteractionObject")]
public class InteractionObjectData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
   
    //public GameObject dropPrefab;

    //[Header("Stacking")]
    //public bool canStack;
    //public int maxStackAmount;

    [Header("Bool")]
    public bool isChage;
    public bool isComplete;

    public class InteractionObject : MonoBehaviour
    {
        [SerializeField]private InteractionObjectData data;

    }
}