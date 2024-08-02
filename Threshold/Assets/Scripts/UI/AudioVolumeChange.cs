using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
    public class AudioVolumeChange : MonoBehaviour
    {
        public AudioMixer audioMixer;
        public Slider audioBGMSlider;
        public Slider audioEffectsSlider;

        private void Start()
        {
            // Set Slider Value
            audioBGMSlider.value = PlayerPrefs.GetFloat("BGM", 0);
            audioMixer.SetFloat("BGM", PlayerPrefs.GetFloat("BGM", 0));
            audioEffectsSlider.value = PlayerPrefs.GetFloat("SFX", 0);
            audioMixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFX", 0));
        }

        public void BGMControl()
        {
            float volume = audioBGMSlider.value;
            if (volume == -40f) audioMixer.SetFloat("BGM", -80);
            else
            {
                audioMixer.SetFloat("BGM", volume);
                PlayerPrefs.SetFloat("BGM", volume);
            }
        }
    
        public void EffectsControl()
        {
            float volume = audioEffectsSlider.value;
            if (volume == -40f) audioMixer.SetFloat("SFX", -80);
            else
            {
                audioMixer.SetFloat("SFX", volume);
                PlayerPrefs.SetFloat("SFX", volume);
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            audioBGMSlider.value = PlayerPrefs.GetFloat("BGM", 0);
            audioMixer.SetFloat("BGM", PlayerPrefs.GetFloat("BGM", 0));
            audioEffectsSlider.value = PlayerPrefs.GetFloat("SFX", 0);
            audioMixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFX", 0));
        }
    }
}
