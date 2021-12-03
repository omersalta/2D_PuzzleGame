using UnityEngine;

namespace Utilities 
{
    public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject 
    {
                
        private static T _instance = null;
        
        public static T Instance {

            get {
                if (_instance == null) {
                    T[] refs = Resources.FindObjectsOfTypeAll<T>();

                    if (refs.Length == 0) {
                        Debug.LogError("SingletonScriptableObject : refs length is 0 of " + typeof(T));
                        return null;
                    }

                    if (refs.Length > 1) {
                        Debug.LogError("SingletonScriptableObject : refs length is 1 of " + typeof(T) );
                    }

                    _instance = refs[0];
                    _instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
                }
                
                return _instance;
            }
        }

    }
}