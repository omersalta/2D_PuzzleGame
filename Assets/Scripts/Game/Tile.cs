using UnityEngine;

namespace Game {

    public class Tile : MonoBehaviour {

        //if the game has more than one player so need to keep list of pickers ex. 
        private static Picker staticPicker;
        private SpriteRenderer _renderer;
        public Vector2Int coordinate { get; private set; }
        public Drop drop;
        
        public void Initialize(int x, int y) {
            coordinate = new Vector2Int(x, y);
        }
        
        private void OnMouseDown() {
            staticPicker.PickTile(this);
        }

    }

}
