using System.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Singleton design pattern which inherits Monobehaviour
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        // ReSharper disable once StaticMemberInGenericType
        private static bool _shuttingDown = false;
        // ReSharper disable once StaticMemberInGenericType
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private static object _lock = new object();
        // ReSharper disable once InconsistentNaming
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_shuttingDown)
                {
                    Debug.Log("[Singleton] Instance '" + typeof(T) + "' already destroyed. Returning null.");
                    return null;
                }
                lock (_lock) //Thread Safe
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();
                   
                        if (_instance == null)
                        {
                            var singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<T>();
                            singletonObject.name = typeof(T).ToString();
                        }
                    }

                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if(_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }

            else if (this != _instance)
            {
                Destroy(gameObject);
            }
        }
        
        public virtual void Reset ()
        {
            _instance = null;
            Destroy(gameObject);
        }
    }
}