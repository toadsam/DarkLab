using System.Collections.Generic;
using UnityEngine;

namespace ScaryEvents
{
    public class ObjectInfoHolder : MonoBehaviour
    {
        public List<Light> lightTargets = new List<Light>();
        public List<Transform> transformTargets = new List<Transform>();
        public List<AudioSource> audioTargets = new List<AudioSource>();
        public scaryEventTier ObjectTier;
    }
}
