using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace ScaryEvents.ScaryEffects
{
    [RequireComponent(typeof(Volume))]
    public class ScaryPostProcessingSeperator : MonoBehaviour
    {
        public PostProcessingEffectType effectType;
    }
}
