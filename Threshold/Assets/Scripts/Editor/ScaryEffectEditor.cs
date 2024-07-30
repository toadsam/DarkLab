using UnityEngine;
using UnityEditor;
using ScaryEvents.ScaryEffects;  // Ensure this namespace matches your actual namespace

[CustomEditor(typeof(ScaryEffect), true), CanEditMultipleObjects]
public class ScaryEffectEditor : Editor
{
    // Default properties
    SerializedProperty duration;
    SerializedProperty delay;
    SerializedProperty ease;
    SerializedProperty loops;
    SerializedProperty showProperties;
    SerializedProperty onStart;
    SerializedProperty onPlay;
    SerializedProperty onUpdate;
    SerializedProperty onComplete;
    
    
    // ScaryDoTweenEffect properties
    SerializedProperty doTweenType;
    SerializedProperty targetPosition;
    SerializedProperty targetRotation;
    SerializedProperty targetScale;
    SerializedProperty shakePosition;
    SerializedProperty isRelative;
    SerializedProperty doTweenLoopType;
    SerializedProperty doTweenLoops;
    SerializedProperty amplitude;
    SerializedProperty frequency;
    SerializedProperty speed;
    SerializedProperty material;

    // ScaryLightEffect properties
    SerializedProperty lightEffectType;
    SerializedProperty targetColor;
    SerializedProperty targetIntensity;
    SerializedProperty targetRange;
    SerializedProperty targetSpotAngle;
    SerializedProperty targetShadowStrength;
    SerializedProperty flickerCount;
    SerializedProperty flickerDuration;
    
    // ScaryPostProcessingEffect properties
    SerializedProperty postProcessingEffectType;
    SerializedProperty weight;
    SerializedProperty onTransitionDuration;
    SerializedProperty offTransitionDuration;
    SerializedProperty targetVolume;
    
    // ScaryAudioEffect properties
    SerializedProperty audioClip;
    SerializedProperty audioVolume;
    SerializedProperty soundRelativePositionFromListener;
    SerializedProperty spatialBlend;
    SerializedProperty audioEffectType;
    // Reverb properties
    SerializedProperty reverbPreset, dryLevel, room, roomHF, decayTime, decayHFRatio,
        reflectionsLevel, reflectionsDelay, reverbLevel, reverbDelay, diffusion, density;
    // Echo properties
    SerializedProperty echoDelay, echoDecayRatio, echoWetMix;
    // LowPass properties
    SerializedProperty lowPassCutoff, lowPassResonance;
    // HighPass properties
    SerializedProperty highPassCutoff, highPassResonance;
    // Distortion properties
    SerializedProperty distortionLevel;
    // Chorus properties
    SerializedProperty chorusDryMix, chorusWetMix1, chorusWetMix2, chorusWetMix3, chorusDelay, chorusRate, chorusDepth;

    //ScaryObjectState properties
    SerializedProperty objectStateType;

    private void OnEnable()
    {
        duration = serializedObject.FindProperty("duration");
        delay = serializedObject.FindProperty("delay"); 
        ease = serializedObject.FindProperty("ease");
        loops = serializedObject.FindProperty("loops");
        showProperties = serializedObject.FindProperty("showProperties");
        onStart = serializedObject.FindProperty("onStart");
        onPlay = serializedObject.FindProperty("onPlay");
        onUpdate = serializedObject.FindProperty("onUpdate");
        onComplete = serializedObject.FindProperty("onComplete");
        
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
            amplitude = serializedObject.FindProperty("amplitude");
            frequency = serializedObject.FindProperty("frequency");
            speed = serializedObject.FindProperty("speed");
            material = serializedObject.FindProperty("material");
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
        
        // Initialize properties for ScaryPostProcessingEffect
        if (target is ScaryPostProcessingEffect)
        {
            postProcessingEffectType = serializedObject.FindProperty("effectType");
            weight = serializedObject.FindProperty("weight");
            onTransitionDuration = serializedObject.FindProperty("onTransitionDuration");
            offTransitionDuration = serializedObject.FindProperty("offTransitionDuration");
        }
        
        // Initialize properties for ScaryAudioEffect
        if (target is ScaryAudioEffect)
        {
            // Common properties
            audioClip = serializedObject.FindProperty("audioClip");
            audioVolume = serializedObject.FindProperty("volume");
            soundRelativePositionFromListener = serializedObject.FindProperty("soundRelativePositionFromListener");
            spatialBlend = serializedObject.FindProperty("spatialBlend");
            audioEffectType = serializedObject.FindProperty("audioEffectType");

            // Reverb properties initialization
            reverbPreset = serializedObject.FindProperty("reverbPreset");
            dryLevel = serializedObject.FindProperty("dryLevel");
            room = serializedObject.FindProperty("room");
            roomHF = serializedObject.FindProperty("roomHF");
            decayTime = serializedObject.FindProperty("decayTime");
            decayHFRatio = serializedObject.FindProperty("decayHFRatio");
            reflectionsLevel = serializedObject.FindProperty("reflectionsLevel");
            reflectionsDelay = serializedObject.FindProperty("reflectionsDelay");
            reverbLevel = serializedObject.FindProperty("reverbLevel");
            reverbDelay = serializedObject.FindProperty("reverbDelay");
            diffusion = serializedObject.FindProperty("diffusion");
            density = serializedObject.FindProperty("density");

            // Echo properties initialization
            echoDelay = serializedObject.FindProperty("echoDelay");
            echoDecayRatio = serializedObject.FindProperty("echoDecayRatio");
            echoWetMix = serializedObject.FindProperty("echoWetMix");

            // LowPass properties initialization
            lowPassCutoff = serializedObject.FindProperty("lowPassCutoff");
            lowPassResonance = serializedObject.FindProperty("lowPassResonance");

            // HighPass properties initialization
            highPassCutoff = serializedObject.FindProperty("highPassCutoff");
            highPassResonance = serializedObject.FindProperty("highPassResonance");

            // Distortion properties initialization
            distortionLevel = serializedObject.FindProperty("distortionLevel");

            // Chorus properties initialization
            chorusDryMix = serializedObject.FindProperty("chorusDryMix");
            chorusWetMix1 = serializedObject.FindProperty("chorusWetMix1");
            chorusWetMix2 = serializedObject.FindProperty("chorusWetMix2");
            chorusWetMix3 = serializedObject.FindProperty("chorusWetMix3");
            chorusDelay = serializedObject.FindProperty("chorusDelay");
            chorusRate = serializedObject.FindProperty("chorusRate");
            chorusDepth = serializedObject.FindProperty("chorusDepth");
        }

        if (target is ScaryObjectStateEffect)
            objectStateType = serializedObject.FindProperty("objectStateType");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ScaryEffect effect = target as ScaryEffect;

        // Use a foldout for ScaryEffect properties
        if (effect != null)
        {
            if (effect is not ScaryAudioEffect)
                EditorGUILayout.PropertyField(duration);
            EditorGUILayout.PropertyField(delay);
            if (effect is not ScaryAudioEffect)
                EditorGUILayout.PropertyField(ease);
            EditorGUILayout.PropertyField(loops);
            
            effect.showProperties = EditorGUILayout.Foldout(effect.showProperties, "UnityEvents", true);
            if (effect.showProperties)
            {
                // UnityEvents drawing
                EditorGUILayout.PropertyField(onStart, new GUIContent("On Start"));
                EditorGUILayout.PropertyField(onPlay, new GUIContent("On Play"));
                EditorGUILayout.PropertyField(onUpdate, new GUIContent("On Update"));
                EditorGUILayout.PropertyField(onComplete, new GUIContent("On Complete"));
            }

            if (effect is ScaryDoTweenEffect)
            {
                DrawDoTweenEffectProperties();
            }

            if (effect is ScaryLightEffect)
            {
                DrawLightEffectProperties();
            }
            
            if (effect is ScaryPostProcessingEffect)
            {
                DrawPostProcessingEffectProperties();
            }

            if (effect is ScaryAudioEffect)
            {
                DrawAudioEffectProperties(effect as ScaryAudioEffect);
            }

            if (effect is ScaryObjectStateEffect)
            {
                DrawObjectStateEffectProperties();
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawDoTweenEffectProperties()
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
            case DoTweenType.WavyTexture:
                EditorGUILayout.PropertyField(amplitude);
                EditorGUILayout.PropertyField(frequency);
                EditorGUILayout.PropertyField(speed);
                EditorGUILayout.PropertyField(material);
                break;
            case DoTweenType.MoveAllRoomObjectsUp:
                EditorGUILayout.PropertyField(targetPosition);
                EditorGUILayout.PropertyField(shakePosition);
                break;
        }
    }

    private void DrawLightEffectProperties()
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
    
    private void DrawPostProcessingEffectProperties()
    {
        EditorGUILayout.PropertyField(postProcessingEffectType);
        EditorGUILayout.PropertyField(weight);
        EditorGUILayout.PropertyField(onTransitionDuration);
        EditorGUILayout.PropertyField(offTransitionDuration);
    }
    
    private void DrawAudioEffectProperties(ScaryAudioEffect effect)
    {
        // General Audio Settings
        EditorGUILayout.PropertyField(audioClip, new GUIContent("Audio Clip"));
        EditorGUILayout.PropertyField(audioVolume, new GUIContent("Volume"));
        EditorGUILayout.PropertyField(soundRelativePositionFromListener, new GUIContent("Sound Position Relative To Listener"));
        EditorGUILayout.PropertyField(spatialBlend, new GUIContent("Spatial Blend (0 = 2D, 1 = 3D)"));

        // Audio Effects Configuration
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(audioEffectType, new GUIContent("Audio Effect Types"));

        // Reverb Settings
        if (effect.audioEffectType.HasFlag(AudioEffectType.Reverb))
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(reverbPreset, new GUIContent("Reverb Preset"));
            EditorGUILayout.PropertyField(dryLevel, new GUIContent("Dry Level"));
            EditorGUILayout.PropertyField(room, new GUIContent("Room"));
            EditorGUILayout.PropertyField(roomHF, new GUIContent("Room HF"));
            EditorGUILayout.PropertyField(decayTime, new GUIContent("Decay Time"));
            EditorGUILayout.PropertyField(decayHFRatio, new GUIContent("Decay HF Ratio"));
            EditorGUILayout.PropertyField(reflectionsLevel, new GUIContent("Reflections Level"));
            EditorGUILayout.PropertyField(reflectionsDelay, new GUIContent("Reflections Delay"));
            EditorGUILayout.PropertyField(reverbLevel, new GUIContent("Reverb Level"));
            EditorGUILayout.PropertyField(reverbDelay, new GUIContent("Reverb Delay"));
            EditorGUILayout.PropertyField(diffusion, new GUIContent("Diffusion"));
            EditorGUILayout.PropertyField(density, new GUIContent("Density"));
        }

        // Echo Settings
        if (effect.audioEffectType.HasFlag(AudioEffectType.Echo))
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(echoDelay, new GUIContent("Echo Delay"));
            EditorGUILayout.PropertyField(echoDecayRatio, new GUIContent("Echo Decay Ratio"));
            EditorGUILayout.PropertyField(echoWetMix, new GUIContent("Echo Wet Mix"));
        }

        // LowPass Settings
        if (effect.audioEffectType.HasFlag(AudioEffectType.LowPass))
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(lowPassCutoff, new GUIContent("Low Pass Cutoff"));
            EditorGUILayout.PropertyField(lowPassResonance, new GUIContent("Low Pass Resonance"));
        }

        // HighPass Settings
        if (effect.audioEffectType.HasFlag(AudioEffectType.HighPass))
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(highPassCutoff, new GUIContent("High Pass Cutoff"));
            EditorGUILayout.PropertyField(highPassResonance, new GUIContent("High Pass Resonance"));
        }

        // Distortion Settings
        if (effect.audioEffectType.HasFlag(AudioEffectType.Distortion))
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(distortionLevel, new GUIContent("Distortion Level"));
        }

        // Chorus Settings
        if (effect.audioEffectType.HasFlag(AudioEffectType.Chorus))
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(chorusDryMix, new GUIContent("Chorus Dry Mix"));
            EditorGUILayout.PropertyField(chorusWetMix1, new GUIContent("Chorus Wet Mix 1"));
            EditorGUILayout.PropertyField(chorusWetMix2, new GUIContent("Chorus Wet Mix 2"));
            EditorGUILayout.PropertyField(chorusWetMix3, new GUIContent("Chorus Wet Mix 3"));
            EditorGUILayout.PropertyField(chorusDelay, new GUIContent("Chorus Delay"));
            EditorGUILayout.PropertyField(chorusRate, new GUIContent("Chorus Rate"));
            EditorGUILayout.PropertyField(chorusDepth, new GUIContent("Chorus Depth"));
        }
    }

    private void DrawObjectStateEffectProperties()
    {
        EditorGUILayout.PropertyField(objectStateType);
    }
}
