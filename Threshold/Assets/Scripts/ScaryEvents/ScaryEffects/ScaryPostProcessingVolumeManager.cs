using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace ScaryEvents.ScaryEffects
{
    public class ScaryPostProcessingVolumeManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Dictionary<PostProcessingEffectType, Volume> postProcessingVolumes = new Dictionary<PostProcessingEffectType, Volume>();
            ScaryPostProcessingSeperator[] postProcessingSeperators = FindObjectsOfType<ScaryPostProcessingSeperator>();
            foreach (ScaryPostProcessingSeperator seperator in postProcessingSeperators)
            {
                postProcessingVolumes.Add(seperator.effectType, seperator.GetComponent<Volume>());
                seperator.gameObject.SetActive(false);
            }
        
            ScaryPostProcessingEffect[] postProcessingEffects = FindObjectsOfType<ScaryPostProcessingEffect>();
            foreach (ScaryPostProcessingEffect effect in postProcessingEffects)
            {
                effect.targetVolume = postProcessingVolumes[effect.effectType];
            }
        }
    }
}
