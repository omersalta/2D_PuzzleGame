using SO_Scripts.Managers;
using UnityEngine;

namespace Game {

    public class Picker : MonoBehaviour {
    
        public Tile TouchedTile;
        private InputState _input;
        private GameManager _gameManager;
        
        private void Start() {
            _gameManager = FindObjectOfType<GameManager>();
            _input = GetComponent<InputState>();
        }

        void Update() {
        
            if (!_input.isDraging) {
                TouchedTile = null;
            }
            
            Try();
        }

        public void PickTile(Tile tile) {
            if(tile.drop != null)
                TouchedTile = tile;
        }

        private void Try() {
            
            if (TouchedTile?.drop == null) { 
                /*empty tile Check: this checking already made on PickTile() method.
                cheap checking cost so, no reason to dont double check*/
                
                return;
            }
            
            if (_input.swipeUp) {
                Try(getTile(TouchedTile.coordinate.x, TouchedTile.coordinate.y + 1));
            }else if (_input.swipeRight) {
                Try(getTile(TouchedTile.coordinate.x + 1, TouchedTile.coordinate.y));
            }else if (_input.swipeDown) {
                Try(getTile(TouchedTile.coordinate.x, TouchedTile.coordinate.y - 1));
            }else if (_input.swipeLeft) {
                Try(getTile(TouchedTile.coordinate.x - 1, TouchedTile.coordinate.y));
            }
        }

        private void Try(Tile targetTile) {
        
            if (targetTile == null) {
                return;
            }

            _gameManager.Move(TouchedTile, targetTile);
        }
   

        private Tile getTile(int x, int y) {
            return MasterManager.boardManager.GetTile(x, y);
        }


    }

}
