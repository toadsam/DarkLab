using UnityEngine;
using UnityEngine.Events;

namespace Tutorial
{
    public abstract class TutorialElementTrigger : MonoBehaviour
    {
        /// <summary>
        /// Inherited classes should call this method when the tutorial have to go next.
        /// 실행 시점을 특정지어야 하는 경우 이용.
        /// </summary>
        public UnityEvent onTutorialElementTriggered;
    }
}
