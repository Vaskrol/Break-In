using UnityEngine;

namespace Assets.Scripts.Basics {
    class MbSingleton <T> : MonoBehaviour where T : MonoBehaviour {
        private static T s_Instance;
        private static readonly object m_Lock = new object();
        private static bool s_ApplicationStopping;

        public static T Instance{
            get {
                lock (m_Lock) {
                    if (s_Instance == null) {
                        T[] objects = FindObjectsOfType<T>();
                        if (objects.Length > 0) {
                            if (objects.Length > 1) {
                                Debug.LogError("MBSingleton: Critical Error! There are more than one instance of" +
                                               typeof(T));
                            }
                            s_Instance = objects[0];
                            Debug.Log("MBSingleton: Using instance of " + typeof(T) + " already created at: " +
                                      s_Instance.gameObject.name);
                        }
                        else if (!s_ApplicationStopping) {
                            GameObject singleton = new GameObject();
                            s_Instance = singleton.AddComponent<T>();
                            singleton.name = typeof(T).ToString();
                            if (Application.isPlaying) {
                                DontDestroyOnLoad(singleton);
                            }
                            Debug.Log("MBSingleton: An instance of " + typeof(T) +
                                      " is needed in the scene, so it was created with DontDestroyOnLoad.");
                        }
                    }
                    return s_Instance;
                }
            }
        }

        public static bool HasInstance() {
            return s_Instance != null;
        }

        protected virtual void OnApplicationQuit() {
            s_ApplicationStopping = true;
        }
    }
}