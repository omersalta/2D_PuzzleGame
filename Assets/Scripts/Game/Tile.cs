using UnityEngine;

namespace Game {

    public class Tile : MonoBehaviour {

        private SpriteRenderer _renderer;
        public Vector2 coordinate { get; private set; }
        public Drop drop;
        public Spawner spawner { get; private set; }
    
        public void Initialize(int x, int y, Spawner mySpawner) {
            spawner = mySpawner;
            coordinate = new Vector2(x, y);
        }
    }

}
