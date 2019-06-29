using JetBrains.Annotations;
using UnityEngine;

namespace Utilities
{
    public class Singleton<T> : Singleton where T : MonoBehaviour
    {
        #region Fields

        [CanBeNull] private static T _instance;
        
        // ReSharper disable once StaticMemberInGenericType
        [NotNull] private static readonly object Lock = new object();

        [SerializeField] private bool persistent = true;

        #endregion

        #region Properties

        public static T Instance
        {
            get
            {
                if (Quitting)
                {
                    Debug.LogWarning(
                        $"[{nameof(Singleton)}<{typeof(T)}>] Instance will not be returned because the application is quitting.");
                    return null;
                }

                lock (Lock)
                {
                    if (_instance != null)
                        return _instance;

                    var instances = FindObjectsOfType<T>();
                    switch (instances.Length)
                    {
                        case 0:
                            Debug.LogWarning(
                                $"[{nameof(Singleton)}<{typeof(T)}>] An instance is needed in the scene and no existing instances were found, so a new instance will be created.");
                            return _instance = new GameObject($"({nameof(Singleton)}){typeof(T)}").AddComponent<T>();
                        case 1:
                            return _instance = instances[0];
                        default:
                            Debug.LogWarning(
                                $"[{nameof(Singleton)}<{typeof(T)}>] There should never be more than one {nameof(Singleton)} of type {typeof(T)} in the scene, but {instances.Length} were found. The first instance found will be used, and all others destroyed.");
                            for (var i = 1; i < instances.Length; i++)
                                Destroy(instances[i]);
                            return _instance = instances[0];
                    }
                }
            }
        }

        #endregion

        #region Methods

        private void Awake()
        {
            if (persistent)
                DontDestroyOnLoad(gameObject);

            OnAwake();
        }
        
        protected virtual void OnAwake() {}

        #endregion
    }
    
    public class Singleton : MonoBehaviour
    {
        #region Properties
        
        protected static bool Quitting { get; private set; }
        
        #endregion

        #region Methods

        private void OnApplicationQuit()
            => Quitting = true;

        #endregion
    }
}
