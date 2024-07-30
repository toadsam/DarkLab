using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ScaryEvents.ScaryEffects
{
    public enum StateType
    {
        None,
        Active,
        Deactive
    }

    public class ScaryStateEffect : ScaryEffect
    {
        [Header("State Settings")]
        public StateType stateType;

        public override void StartEffectInternal()
        {
            GameObject currentObject = targetSource.GetCurrentTarget<Transform>("transform").gameObject;

            switch (stateType)
            {
                case StateType.Active:
                    Active(currentObject);
                    break;
                case StateType.Deactive:
                    StartCoroutine(Deactive(currentObject));
                    break;
            }
        }

        private void Active(GameObject inputObject)
        {
            inputObject.SetActive(true);
            
            DelayAndStopEffect();
        }

        private IEnumerator Deactive(GameObject inputObject)
        {
            yield return new WaitForSeconds(duration);

            inputObject.SetActive(false);

            StopEffect();
        }
    }
}
