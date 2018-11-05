using UnityEngine;

namespace Basics {
    public class MbSingleton <T> : MonoBehaviour where T : MonoBehaviour {
        private static T _instance;
        private static readonly object _lock = new object();
        private static bool _applicationStopping;

        public static T Instance{
            get {
                lock (_lock) {
                    if (_instance != null) 
                        return _instance;
                    
                    T[] objects = FindObjectsOfType<T>();
                    if (objects.Length > 0) {
                        if (objects.Length > 1) {
                            Debug.LogError("MBSingleton: Critical Error! There are more than one instance of" +
                                           typeof(T));
                        }
                        _instance = objects[0];
                    }
                    else if (!_applicationStopping) {
                        var singleton  = new GameObject();
                        _instance      = singleton.AddComponent<T>();
                        singleton.name = typeof(T).ToString();
                        if (Application.isPlaying) 
                            DontDestroyOnLoad(singleton);
                    }
                    return _instance;
                }
            }
        }

        public static bool HasInstance() {
            return _instance != null;
        }

        protected virtual void OnApplicationQuit() {
            _applicationStopping = true;
        }
    }
}