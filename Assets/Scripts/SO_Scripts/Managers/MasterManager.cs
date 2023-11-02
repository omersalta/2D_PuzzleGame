using System.Collections.Generic;
using Game;
using UnityEngine;
using Utilities;

namespace SO_Scripts.Managers {

    [CreateAssetMenu(menuName = "SO/MasterManager")]
    public class MasterManager : SingletonScriptableObject<MasterManager> {
        
        // this master manager script is must customize for your project specialy and use it 
        // your prefered, it should give easy reacing to your singletons referance.
        
         [SerializeField] private BoardManager _boardManager;
         public static BoardManager boardManager { get => Instance._boardManager; }
         
         [SerializeField] private List<Sprite> _dropSprites;
         public static List<Sprite> dropSprites { get => Instance._dropSprites; }
         

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void FirstInitilize() {
            boardManager.Initialize();
            FindObjectOfType<CameraPositioner>().Position();
        }
        
        

    }

}




