using UnityEngine;

namespace Game {

    public class Tile : MonoBehaviour {

        private SpriteRenderer _renderer;
        public Vector2 coordinate { get; private set;  }
        public Drop drop;
    
    }

}
