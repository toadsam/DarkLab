using UnityEngine;
using UnityEditor;
using ScaryEvents.ScaryEffects;  // Ensure this namespace matches your actual namespace

[CustomEditor(typeof(ScaryEffect), true), CanEditMultipleObjects]
public class ScaryEffectEditor : Editor
{
    // ScaryDoTweenEffect properties
    SerializedProperty doTweenType;
    SerializedProperty targetPosition;
    SerializedProperty targetRotation;
    SerializedProperty targetScale;
    SerializedProperty shakePosition;
    SerializedProperty isRelative;
    SerializedProperty doTweenLoopType;
    SerializedProperty doTweenLoops;

    // ScaryLightEffect properties
    SerializedProperty lightEffectType;
    SerializedProperty targetColor;
    SerializedProperty targetIntensity;
    SerializedProperty targetRange;
    SerializedProperty targetSpotAngle;
    SerializedProperty targetShadowStrength;
    SerializedProperty flickerCount;
    SerializedProperty flickerDuration;

    private void OnEnable()
    {
        // Find properties for ScaryDoTweenEffect
        if (target is ScaryDoTweenEffect)
        {
            doTweenType = serializedObject.FindProperty("doTweenType");
            targetPosition = serializedObject.FindProperty("targetPosition");
            targetRotation = serializedObject.FindProperty("targetRotation");
            targetScale = serializedObject.FindProperty("targetScale");
            shakePosition = serializedObject.FindProperty("shakePosition");
            isRelative = serializedObject.FindProperty("isRelative");
            doTweenLoopType = serializedObject.FindProperty("doTweenLoopType");
            doTweenLoops = serializedObject.FindProperty("doTweenLoops");
        }

        // Find properties for ScaryLightEffect
        if (target is ScaryLightEffect)
        {
            lightEffectType = serializedObject.FindProperty("effectType");
            targetColor = serializedObject.FindProperty("targetColor");
            targetIntensity = serializedObject.FindProperty("targetIntensity");
            targetRange = serializedObject.FindProperty("targetRange");
            targetSpotAngle = serializedObject.FindProperty("targetSpotAngle");
            targetShadowStrength = serializedObject.FindProperty("targetShadowStrength");
            flickerCount = serializedObject.FindProperty("flickerCount");
            flickerDuration = serializedObject.FindProperty("flickerDuration");
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ScaryEffect effect = target as ScaryEffect;

        // Use a foldout for ScaryEffect properties
        if (effect != null)
        {
            effect.showProperties = EditorGUILayout.Foldout(effect.showProperties, "Scary Effect Properties", true);
            if (effect.showProperties)
            {
                DrawDefaultInspector(); // Draws all the default items
            }

            if (effect is ScaryDoTweenEffect)
            {
                DrawDoTweenEffectProperties(effect as ScaryDoTweenEffect);
            }

            if (effect is ScaryLightEffect)
            {
                DrawLightEffectProperties(effect as ScaryLightEffect);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawDoTweenEffectProperties(ScaryDoTweenEffect effect)
    {
        EditorGUILayout.PropertyField(doTweenType);
        DoTweenType type = (DoTweenType)doTweenType.enumValueIndex;
        
        EditorGUILayout.PropertyField(isRelative);
        EditorGUILayout.PropertyField(doTweenLoopType);
        EditorGUILayout.PropertyField(doTweenLoops);
        
        switch (type)
        {
            case DoTweenType.Move:
                EditorGUILayout.PropertyField(targetPosition);
                break;
            case DoTweenType.Rotate:
                EditorGUILayout.PropertyField(targetRotation);
                break;
            case DoTweenType.Scale:
                EditorGUILayout.PropertyField(targetScale);
                break;
            case DoTweenType.Shake:
                EditorGUILayout.PropertyField(shakePosition);
                break;
        }
    }

    private void DrawLightEffectProperties(ScaryLightEffect effect)
    {
        EditorGUILayout.PropertyField(lightEffectType);
        LightEffectType type = (LightEffectType)lightEffectType.enumValueIndex;

        switch (type)
        {
            case LightEffectType.ColorChange:
                EditorGUILayout.PropertyField(targetColor);
                break;
            case LightEffectType.IntensityChange:
                EditorGUILayout.PropertyField(targetIntensity);
                break;
            case LightEffectType.Flicker:
                EditorGUILayout.PropertyField(targetIntensity);
                EditorGUILayout.PropertyField(flickerCount);
                EditorGUILayout.PropertyField(flickerDuration);
                break;
            case LightEffectType.RangeChange:
                EditorGUILayout.PropertyField(targetRange);
                break;
            case LightEffectType.SpotAngleChange:
                EditorGUILayout.PropertyField(targetSpotAngle);
                break;
            case LightEffectType.ShadowStrengthChange:
                EditorGUILayout.PropertyField(targetShadowStrength);
                break;
        }
    }
}
