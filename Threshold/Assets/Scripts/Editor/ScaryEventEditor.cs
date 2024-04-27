using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[CustomEditor(typeof(ScaryEvent))]
public class ScaryEventEditor : Editor
{
    private bool showOnStartEvents = false;
    private bool showOnPlayEvents = false;
    private bool showOnUpdateEvents = false;
    private bool showOnCompleteEvents = false;
    public override void OnInspectorGUI()
    {
        ScaryEvent script = (ScaryEvent)target;
        serializedObject.Update();

        
        script.scaryEventTier = (scaryEventTier)EditorGUILayout.EnumPopup("Scary Event Tier", script.scaryEventTier);
        script.scaryEventWhen = (scaryEventWhen)EditorGUILayout.EnumPopup("Scary Event When", script.scaryEventWhen);

        
        EditorGUILayout.ObjectField("Current Event Target", script.currentEventTarget, typeof(ObjectInfoHolder), true);

        
        showOnStartEvents = EditorGUILayout.Foldout(showOnStartEvents, "On Start Events");
        if (showOnStartEvents)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onStart"), new GUIContent("On Start"));

        showOnPlayEvents = EditorGUILayout.Foldout(showOnPlayEvents, "On Play Events");
        if (showOnPlayEvents)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onPlay"), new GUIContent("On Play"));

        showOnUpdateEvents = EditorGUILayout.Foldout(showOnUpdateEvents, "On Update Events");
        if (showOnUpdateEvents)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onUpdate"), new GUIContent("On Update"));

        showOnCompleteEvents = EditorGUILayout.Foldout(showOnCompleteEvents, "On Complete Events");
        if (showOnCompleteEvents)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onComplete"), new GUIContent("On Complete"));

        serializedObject.ApplyModifiedProperties();
    }
}
