using SO_Scripts.Managers;
using UnityEngine;

namespace Game.GamePlay {

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
                TrySwipe();
        }
        
        public void PickTile(Tile tile) {
            if(tile.drop != null)
                TouchedTile = tile;
        }

        private void TrySwipe() {
            
            if (!TouchedTile || TouchedTile?.drop == null) { 
                /*empty tile Check: this checking already made on PickTile() method.
                cheap checking cost so, no reason to dont double check*/
                TouchedTile = null;
                return;
            }
            
            if (_input.swipeUp) {
                _direction = Direction.UP;
                TryMove(getTile(TouchedTile.coordinate.x, TouchedTile.coordinate.y + 1));
                TouchedTile = null;
            }else if (_input.swipeRight) {
                _direction = Direction.RIGHT;
                TryMove(getTile(TouchedTile.coordinate.x + 1, TouchedTile.coordinate.y));
                TouchedTile = null;
            }else if (_input.swipeDown) {
                _direction = Direction.DOWN;
                TryMove(getTile(TouchedTile.coordinate.x, TouchedTile.coordinate.y - 1));
                TouchedTile = null;
            }else if (_input.swipeLeft) {
                _direction = Direction.LEFT;
                TryMove(getTile(TouchedTile.coordinate.x - 1, TouchedTile.coordinate.y));
                TouchedTile = null;
            }
        }

        private void TryMove(Tile targetTile) {
            
            if (targetTile == null || TouchedTile == null) {
                if(TouchedTile)
                    _gameManager.TweeningFakeMove(TouchedTile.drop, GetV3Dir(_direction));
                return;
            }
            
            if (targetTile.drop == null || TouchedTile.drop == null) {
                if(TouchedTile.drop)
                    _gameManager.TweeningFakeMove(TouchedTile.drop, GetV3Dir(_direction));
                return;
            }
            
            _gameManager.SwitchTiles(TouchedTile, targetTile);
        }
   

        private Tile getTile(int x, int y) {
            return MasterManager.boardManager.GetTile(x, y);
        }

        private Vector3 GetV3Dir(Direction dir)
        {
            switch (dir) {
                case Picker.Direction.RIGHT://rigth
                    return Vector3.right;
                    break;
                case Picker.Direction.DOWN://down
                    return Vector3.down;
                    break;
                case Picker.Direction.LEFT://left
                    return Vector3.left;
                    break;
                case Picker.Direction.UP://up
                    return Vector3.up;
                    break;
                default:
                    Debug.LogWarning("unknown direction value");
                    return Vector3.zero;
                    break;
            }
        }


    }

}
