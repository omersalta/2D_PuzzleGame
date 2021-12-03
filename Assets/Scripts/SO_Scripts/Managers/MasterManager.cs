using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace SO_Scripts.Managers {

    [CreateAssetMenu(menuName = "SO/MasterManager")]
    public class MasterManager : SingletonScriptableObject<MasterManager> {
        
        // this master manager script is must customize for your project specialy and use it 
        // your prefered, it should give easy reacing to your singletons referance.
        
        [SerializeField] private GameObject _gameSettings;
        public static GameObject gameSettings { get => Instance._gameSettings; }
        

        
        

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void FirstInitilize() {
            Debug.Log("this massage will output before awake");
        } 
        

    }

}




