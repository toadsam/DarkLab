using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScaryLightEffect))]
public class ScaryLightEffectEditor : Editor
{
    SerializedProperty effectTypeProp;
    SerializedProperty lightComponentProp;
    SerializedProperty targetColorProp;
    SerializedProperty targetIntensityProp;
    SerializedProperty targetIndirectMultiplierProp;
    SerializedProperty targetShadowTypeProp;
    SerializedProperty targetDrawHaloProp;
    SerializedProperty durationProp;

    void OnEnable()
    {
        
        effectTypeProp = serializedObject.FindProperty("effectType");
        lightComponentProp = serializedObject.FindProperty("lightComponent");
        targetColorProp = serializedObject.FindProperty("targetColor");
        targetIntensityProp = serializedObject.FindProperty("targetIntensity");
        targetIndirectMultiplierProp = serializedObject.FindProperty("targetIndirectMultiplier");
        targetShadowTypeProp = serializedObject.FindProperty("targetShadowType");
        targetDrawHaloProp = serializedObject.FindProperty("targetDrawHalo");
        durationProp = serializedObject.FindProperty("duration");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(effectTypeProp);

        LightEffectType effectType = (LightEffectType)effectTypeProp.enumValueIndex;

        
        switch (effectType)
        {
            case LightEffectType.Flicker:
                EditorGUILayout.PropertyField(lightComponentProp);
                EditorGUILayout.PropertyField(targetIntensityProp);
                EditorGUILayout.PropertyField(durationProp);
                break;
            case LightEffectType.ColorChange:
                EditorGUILayout.PropertyField(lightComponentProp);
                EditorGUILayout.PropertyField(targetColorProp);
                EditorGUILayout.PropertyField(durationProp);
                break;
            case LightEffectType.IntensityChange:
                EditorGUILayout.PropertyField(lightComponentProp);
                EditorGUILayout.PropertyField(targetIntensityProp);
                EditorGUILayout.PropertyField(durationProp);
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }
}