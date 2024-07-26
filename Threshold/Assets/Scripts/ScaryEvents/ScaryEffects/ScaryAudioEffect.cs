using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace ScaryEvents.ScaryEffects
{
    [System.Flags]
    public enum AudioEffectType
    {
        None = 0,
        Reverb = 1 << 0,
        Echo = 1 << 1,
        LowPass = 1 << 2,
        HighPass = 1 << 3,
        Distortion = 1 << 4,
        Chorus = 1 << 5
    }

    
    public class ScaryAudioEffect : ScaryEffect
    {
        [Header("Audio Settings")]
        public AudioClip audioClip;
        public Vector3 soundRelativePositionFromListener = Vector3.zero;
        public float spatialBlend = 1.0f; // 0 is 2D sound, 1 is 3D sound.

        private AudioSource audioSource;
        private List<Behaviour> audioFilters = new List<Behaviour>(7);
        
        // Audio Filter Settings : Reverb, Echo, LowPass, HighPass, Distortion, Chorus
        // Detail 한 요소들이 모두 포함됨.
        [Header("Audio Filter Settings")]
        public AudioEffectType audioEffectType;
        
        [Header("Reverb Settings")]
        public AudioReverbPreset reverbPreset;
        [Range(-10000, 0)] public float dryLevel = 0;
        [Range(-10000, 0)] public float room = 0;
        [Range(-10000, 0)] public float roomHF = 0;
        [Range(0.1f, 20)] public float decayTime = 1;
        [Range(0.1f, 2.0f)] public float decayHFRatio = 0.5f;
        [Range(-10000, 1000)] public float reflectionsLevel = -10000;
        [Range(0.0f, 0.3f)] public float reflectionsDelay = 0;
        [Range(-10000, 2000)] public float reverbLevel = 0;
        [Range(0, 1)] public float reverbDelay = 0.04f;
        [Range(0, 100)] public float diffusion = 100;
        [Range(0, 100)] public float density = 100;
        
        [Header("Echo Settings")]
        [Range(10, 5000)] public float echoDelay = 500;
        [Range(0.1f, 1.0f)] public float echoDecayRatio = 0.5f; 
        [Range(0.0f, 1.0f)] public float echoWetMix = 1; 
        
        [Header("Low Pass Settings")]
        [Range(10, 22000)] public float lowPassCutoff = 5000; 
        [Range(1, 10)] public float lowPassResonance = 1; 
        
        [Header("High Pass Settings")]
        [Range(10, 22000)] public float highPassCutoff = 5000; 
        [Range(1, 10)] public float highPassResonance = 1; 

        [Header("Distortion Settings")]
        [Range(0, 1)] public float distortionLevel = 0.5f; 
        
        [Header("Chorus Settings")]
        [Range(0.0f, 1.0f)] public float chorusDryMix = 0.5f; 
        [Range(0.0f, 1.0f)] public float chorusWetMix1 = 0.25f; 
        [Range(0.0f, 1.0f)] public float chorusWetMix2 = 0.25f; 
        [Range(0.0f, 1.0f)] public float chorusWetMix3 = 0.25f; 
        [Range(0.1f, 100f)] public float chorusDelay = 40.0f; 
        [Range(0.0f, 20.0f)] public float chorusRate = 1.5f; 
        [Range(0.0f, 1.0f)] public float chorusDepth = 0.7f; 



        public override void StartEffectInternal()
        {
            SetupAudioSource();
            PlaySound();
        }

        private void PlaySound()
        {
            audioSource.PlayDelayed(delay);
            DelayAndStopEffect(audioClip.length);
        }

        public override void StopEffect()
        {
            audioSource.Stop();
            ClearAudioEffects();
            base.StopEffect();
        }
        
        private void SetupAudioSource()
        {
            GameObject audioSourceObject = FindObjectOfType<AudioSourcePoolManager>().Pool.Get();
            audioSource = audioSourceObject.GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = audioSourceObject.AddComponent<AudioSource>();

            audioSource.clip = audioClip;
            audioSource.spatialBlend = spatialBlend;
            audioSource.loop = loops == -1;
            
            if (Camera.main != null)
                audioSource.transform.position = Camera.main.transform.position + soundRelativePositionFromListener;

            ApplyAudioEffects();
        }

        private void ApplyAudioEffects()
        {
            if (audioEffectType.HasFlag(AudioEffectType.Reverb))
            {
                var reverbFilter = audioSource.gameObject.GetComponent<AudioReverbFilter>();
                if (reverbFilter == null)
                    reverbFilter = audioSource.gameObject.AddComponent<AudioReverbFilter>();
                else 
                    reverbFilter.enabled = true;
                
                reverbFilter.reverbPreset = reverbPreset;
                reverbFilter.dryLevel = dryLevel;
                reverbFilter.room = room;
                reverbFilter.roomHF = roomHF;
                reverbFilter.decayTime = decayTime;
                reverbFilter.decayHFRatio = decayHFRatio;
                reverbFilter.reflectionsLevel = reflectionsLevel;
                reverbFilter.reflectionsDelay = reflectionsDelay;
                reverbFilter.reverbLevel = reverbLevel;
                reverbFilter.reverbDelay = reverbDelay;
                reverbFilter.diffusion = diffusion;
                reverbFilter.density = density;
                audioFilters.Add(reverbFilter);
            }

            if (audioEffectType.HasFlag(AudioEffectType.Echo))
            {
                var echoFilter = audioSource.gameObject.GetComponent<AudioEchoFilter>();
                if (echoFilter == null)
                    echoFilter = audioSource.gameObject.AddComponent<AudioEchoFilter>();
                else 
                    echoFilter.enabled = true;
                
                echoFilter.delay = echoDelay;
                echoFilter.decayRatio = echoDecayRatio;
                echoFilter.wetMix = echoWetMix;
                audioFilters.Add(echoFilter);
            }

            if (audioEffectType.HasFlag(AudioEffectType.LowPass))
            {
                var lowPassFilter = audioSource.gameObject.GetComponent<AudioLowPassFilter>();
                if (lowPassFilter == null)
                    lowPassFilter = audioSource.gameObject.AddComponent<AudioLowPassFilter>();
                else 
                    lowPassFilter.enabled = true;
                
                lowPassFilter.cutoffFrequency = lowPassCutoff;
                lowPassFilter.lowpassResonanceQ = lowPassResonance;
                audioFilters.Add(lowPassFilter);
            }

            if (audioEffectType.HasFlag(AudioEffectType.HighPass))
            {
                var highPassFilter = audioSource.gameObject.GetComponent<AudioHighPassFilter>();
                if (highPassFilter == null)
                    highPassFilter = audioSource.gameObject.AddComponent<AudioHighPassFilter>();
                else 
                    highPassFilter.enabled = true;
                
                highPassFilter.cutoffFrequency = highPassCutoff;
                highPassFilter.highpassResonanceQ = highPassResonance;
                audioFilters.Add(highPassFilter);
            }

            if (audioEffectType.HasFlag(AudioEffectType.Distortion))
            {
                var distortionFilter = audioSource.gameObject.GetComponent<AudioDistortionFilter>();
                if (distortionFilter == null)
                    distortionFilter = audioSource.gameObject.AddComponent<AudioDistortionFilter>();
                else 
                    distortionFilter.enabled = true;
                
                distortionFilter.distortionLevel = distortionLevel;
                audioFilters.Add(distortionFilter);
            }

            if (audioEffectType.HasFlag(AudioEffectType.Chorus))
            {
                var chorusFilter = audioSource.gameObject.GetComponent<AudioChorusFilter>();
                if (chorusFilter == null)
                    chorusFilter = audioSource.gameObject.AddComponent<AudioChorusFilter>();
                else 
                    chorusFilter.enabled = true;
                
                chorusFilter.dryMix = chorusDryMix;
                chorusFilter.wetMix1 = chorusWetMix1;
                chorusFilter.wetMix2 = chorusWetMix2;
                chorusFilter.wetMix3 = chorusWetMix3;
                chorusFilter.delay = chorusDelay;
                chorusFilter.rate = chorusRate;
                chorusFilter.depth = chorusDepth;
                audioFilters.Add(chorusFilter);
            }
        }
        
        private void ClearAudioEffects()
        {
            foreach (var filter in audioFilters)
            {
                filter.enabled = false;
            }
            audioFilters.Clear();
            FindObjectOfType<AudioSourcePoolManager>().Pool.Release(audioSource.gameObject);
        }

    }
}