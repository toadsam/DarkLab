using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ScaryEvents.ScaryEffects
{
    public class ScaryActivateDeactivateEffect : ScaryEffect
    {
        public override void StartEffectInternal()
        {
            ActivateObject();
            DelayAndDeactivateObject();
        }

        private void ActivateObject()
        {
            targetSource.gameObject.SetActive(true);
        }

        private void DelayAndDeactivateObject()
        {
            effectCoroutines.Add(StartCoroutine(DeactivateObjectAfterDelay()));
        }

        private IEnumerator DeactivateObjectAfterDelay()
        {
            yield return new WaitForSeconds(duration);
            targetSource.gameObject.SetActive(false);
            StopEffect();
        }
    }
}
