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

        
        script.metaData.tier = (scaryEventTier)EditorGUILayout.EnumPopup("Scary Event Tier", script.metaData.tier);

        
        EditorGUILayout.ObjectField("Current Event Target", script.currentEventTarget, typeof(ObjectInfoHolder), true);

        
        showOnStartEvents = EditorGUILayout.Foldout(showOnStartEvents, "On Start Events");
        if (showOnStartEvents)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onStart"), new GUIContent("On Start"));

        serializedObject.ApplyModifiedProperties();
    }
}
