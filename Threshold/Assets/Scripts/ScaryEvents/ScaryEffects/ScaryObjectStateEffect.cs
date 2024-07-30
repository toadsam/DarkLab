using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ScaryEvents.ScaryEffects
{
    public enum ObjectStateType
    {
        None,
        Active,
        Deactive
    }

    public class ScaryObjectStateEffect : ScaryEffect
    {
        [Header("Object State Settings")]
        public ObjectStateType objectStateType;

        public override void StartEffectInternal()
        {
            GameObject currentObject = targetSource.GetCurrentTarget<Transform>("transform").gameObject;

            switch (objectStateType)
            {
                case ObjectStateType.Active:
                    Active(currentObject);
                    break;
                case ObjectStateType.Deactive:
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
