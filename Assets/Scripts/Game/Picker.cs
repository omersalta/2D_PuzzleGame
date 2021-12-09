using SO_Scripts.Managers;
using UnityEngine;

namespace Game {

    public class Picker : MonoBehaviour {
    
        public Tile TouchedTile;
        private InputState _input;
        private GameManager _gameManager;
        
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
            Debug.Log(tile);
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
                Try(getTile(TouchedTile.coordinate.x, TouchedTile.coordinate.y + 1));
                TouchedTile = null;
            }else if (_input.swipeRight) {
                Try(getTile(TouchedTile.coordinate.x + 1, TouchedTile.coordinate.y));
                TouchedTile = null;
            }else if (_input.swipeDown) {
                Try(getTile(TouchedTile.coordinate.x, TouchedTile.coordinate.y - 1));
                TouchedTile = null;
            }else if (_input.swipeLeft) {
                Try(getTile(TouchedTile.coordinate.x - 1, TouchedTile.coordinate.y));
                TouchedTile = null;
            }
        }

        private void Try(Tile targetTile) {
        
            Debug.Log("move gonna happend : " + TouchedTile +",  "+targetTile);
            
            if (targetTile == null || TouchedTile == null) {
                return;
            }
            
            if (targetTile.drop == null || TouchedTile.drop == null) {
                //TODO FakeMove Func call
                return;
            }
            
            _gameManager.Move(TouchedTile, targetTile);
        }
   

        private Tile getTile(int x, int y) {
            return MasterManager.boardManager.GetTile(x, y);
        }


    }

}
