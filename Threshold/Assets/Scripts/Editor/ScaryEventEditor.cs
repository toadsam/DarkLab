using System.Collections;
using System.Collections.Generic;
using ScaryEvents;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[CustomEditor(typeof(ScaryEvent))]
public class ScaryEventEditor : Editor
{
    private bool showOnStartEvents = false;
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

        serializedObject.ApplyModifiedProperties();
    }
}
