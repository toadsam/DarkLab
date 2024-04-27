using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScaryDoTweenEffect))]
public class ScaryDoTweenEffectEditor : Editor
{
    SerializedProperty doTweenTypeProp;
    SerializedProperty targetPositionProp;
    SerializedProperty targetRotationProp;
    SerializedProperty targetScaleProp;
    SerializedProperty shakePositionProp;
    SerializedProperty easeProp;
    SerializedProperty durationProp;

    void OnEnable()
    {
        // ??????? ????????.
        doTweenTypeProp = serializedObject.FindProperty("doTweenType");
        targetPositionProp = serializedObject.FindProperty("targetPosition");
        targetRotationProp = serializedObject.FindProperty("targetRotation");
        targetScaleProp = serializedObject.FindProperty("targetScale");
        shakePositionProp = serializedObject.FindProperty("shakePosition");
        easeProp = serializedObject.FindProperty("ease");
        durationProp = serializedObject.FindProperty("duration");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(doTweenTypeProp);

        DoTweenType currentType = (DoTweenType)doTweenTypeProp.enumValueIndex;

        // ????? DoTweenType?? ???? ????? ????????.
        switch (currentType)
        {
            case DoTweenType.Move:
                EditorGUILayout.PropertyField(targetPositionProp);
                break;
            case DoTweenType.Rotate:
                EditorGUILayout.PropertyField(targetRotationProp);
                break;
            case DoTweenType.Scale:
                EditorGUILayout.PropertyField(targetScaleProp);
                break;
            case DoTweenType.Shake:
                EditorGUILayout.PropertyField(shakePositionProp);
                break;
        }

        // Duration?? ??? ????????.
        EditorGUILayout.PropertyField(easeProp);
        EditorGUILayout.PropertyField(durationProp);

        serializedObject.ApplyModifiedProperties();
    }
}