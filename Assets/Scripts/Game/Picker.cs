using SO_Scripts.Managers;
using UnityEngine;

namespace Game {

    public class Picker : MonoBehaviour {
    
        public Tile TouchedTile;
        private InputState _input;
        private GameManager _gameManager;
        public Direction _direction;

        public enum Direction {
            RIGHT,
            DOWN,
            LEFT,
            UP,
        }
        
        
        private void Start() {
            Tile.SetPicker(this);
            _gameManager = FindObjectOfType<GameManager>();
            _input = GetComponent<InputState>();
        }

        void Update() {
            if(!_input.mouseUp)
                Try();
        }
        
        public void PickTile(Tile tile) {
            if(tile.drop != null)
                TouchedTile = tile;
        }

        private void Try() {
            
            if (!TouchedTile || TouchedTile?.drop == null) { 
                /*empty tile Check: this checking already made on PickTile() method.
                cheap checking cost so, no reason to dont double check*/
                TouchedTile = null;
                return;
            }
            
            
            if (_input.swipeUp) {
                _direction = Direction.UP;
                Try(getTile(TouchedTile.coordinate.x, TouchedTile.coordinate.y + 1));
                TouchedTile = null;
            }else if (_input.swipeRight) {
                _direction = Direction.RIGHT;
                Try(getTile(TouchedTile.coordinate.x + 1, TouchedTile.coordinate.y));
                TouchedTile = null;
            }else if (_input.swipeDown) {
                _direction = Direction.DOWN;
                Try(getTile(TouchedTile.coordinate.x, TouchedTile.coordinate.y - 1));
                TouchedTile = null;
            }else if (_input.swipeLeft) {
                _direction = Direction.LEFT;
                Try(getTile(TouchedTile.coordinate.x - 1, TouchedTile.coordinate.y));
                TouchedTile = null;
            }
        }

        private void Try(Tile targetTile) {
            
            if (targetTile == null || TouchedTile == null) {
                if(TouchedTile)
                    _gameManager.TweeningFakeMove(TouchedTile.drop, _direction);
                return;
            }
            
            if (targetTile.drop == null || TouchedTile.drop == null) {
                if(TouchedTile.drop)
                    _gameManager.TweeningFakeMove(TouchedTile.drop, _direction);
                return;
            }
            
            _gameManager.Move(TouchedTile, targetTile);
        }
   

        private Tile getTile(int x, int y) {
            return MasterManager.boardManager.GetTile(x, y);
        }


    }

}
