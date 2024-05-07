using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ObjectInfoHolder : MonoBehaviour
{
    public List<Light> lightTargets = new List<Light>();
    public List<Transform> transformTargets = new List<Transform>();
    public List<AudioSource> audioTargets = new List<AudioSource>();
    public scaryEventTier ObjectTier;
}
