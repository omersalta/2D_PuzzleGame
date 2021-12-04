using SO_Scripts.Managers;
using UnityEngine;

namespace Game {

    public class Drop : MonoBehaviour {
        
        public enum eColor {
            Blue,
            Green,
            Red,
            Yellow
        }
        
        private SpriteRenderer _mySpriteRenderer;
        public eColor color { get; private set; }

        public void FirstInitialize(eColor color) {
            _mySpriteRenderer = GetComponent<SpriteRenderer>();
            this.color = color;
            _mySpriteRenderer.sprite = MasterManager.dropSprites[(int) color];
            
        }
    
    }

}
