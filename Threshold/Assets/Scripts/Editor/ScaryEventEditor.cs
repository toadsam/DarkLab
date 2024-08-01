using ScaryEvents;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScaryEvent))]
public class ScaryEventEditor : Editor
{
    private bool showOnStartEvents = false;
    private SerializedProperty metaDataProperty;
    private SerializedProperty tierProperty;

    private void OnEnable()
    {
        metaDataProperty = serializedObject.FindProperty("metaData");
        tierProperty = metaDataProperty.FindPropertyRelative("tier");
    }

    public override void OnInspectorGUI()
    {
        ScaryEvent script = (ScaryEvent)target;
        serializedObject.Update();
        
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(tierProperty, new GUIContent("Scary Event Tier"));
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
        
        EditorGUILayout.ObjectField("Current Event Target", script.currentEventTarget, typeof(ObjectInfoHolder), true);
        
        showOnStartEvents = EditorGUILayout.Foldout(showOnStartEvents, "On Start Events");
        if (showOnStartEvents)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onStart"), new GUIContent("On Start"));
        
        serializedObject.ApplyModifiedProperties();
    }
}