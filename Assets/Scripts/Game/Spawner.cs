using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;
using Utilities.RecycleGameObject;

namespace Game {

    public class Spawner : MonoBehaviour {

        public int columnNo { get; private set; }
        private GameObject _dropPrefab;
        public bool Activate;
        [SerializeField]
        private List<Tile> myColumnTiles;
        private static GameManager _gameManager;

        public static void SetGameManager(GameManager manager) {
            _gameManager = manager;
        }
        
        
        public int GetCurrentTileCount() { return myColumnTiles.Count; }

        public void AddTile(Tile tile) {
        
            if (myColumnTiles.Any(t => t.coordinate.y == tile.coordinate.y)) {
                Debug.LogWarning("there is already same level tile");
            }
        
            myColumnTiles.Add(tile);

            //after every adding ordering coordinate y values (y = 0 is the last one)
            myColumnTiles = myColumnTiles.OrderBy(tile => tile.coordinate.y).ToList();

        }
    
        public void OnClick_AvtisionButton() {
            //TODO add clickable activision spawner and show to user with visual something (text,sprite...)
        }

        public void Initialize(int myColumnNo, GameObject dropPrefab) {
            _dropPrefab = dropPrefab;
            columnNo = myColumnNo;
            Activate = true;
            myColumnTiles = new List<Tile>();
        }
    
        public void Initialize(int myColumnNo, GameObject dropPrefab, bool activationState) {
            _dropPrefab = dropPrefab;
            columnNo = myColumnNo;
            Activate = activationState;
            myColumnTiles = new List<Tile>();
        }

        public void CreateDrop() {
        
            if (!Activate) {
                return;
            }

            _CreateDrop();
        }
    
        private void _CreateDrop() {
        
            GameObject dropGO;
            dropGO = GameObjectUtil.Instantiate(_dropPrefab, transform.localPosition);
            dropGO.transform.localPosition = transform.position;
            Drop drop = dropGO.GetComponent<Drop>();
            drop.FirstInitialize(GetRandomColor());
            Tile EmptyTile = GetUndermostEmptyTile();
            _gameManager.Tweening(drop,EmptyTile);
        }

        private Tile GetUndermostEmptyTile() {
            //its return undermost empty tile if correctly ordered from coordinate.y
            foreach (var tile in myColumnTiles) {
                if (tile.drop == null) {
                    
                    return tile;
                }
            }
            
            return null;
        }
        
        private Tile GetUndermostEmptyTileFromİndex(int index) {
            //its return undermost empty tile
            int lastEmptyIndex = -1;

            if (index < 0 || myColumnTiles.Count-2 <= index)
                return null;
            
            for (int i = index; i > -1; i--) {
                if (myColumnTiles[i].drop == null) {
                    lastEmptyIndex = i;
                }
            }

            if (lastEmptyIndex == -1)
                return null;
            
            return myColumnTiles[lastEmptyIndex];
        }
        
        public void FallDropsAndFillList(List<Tile> list,bool outoSpawn) {
            //it fall all drops need to drop and fill given list with last moved drop for cheking again

            
            //Falling Drops
            for (int i = 0; i < myColumnTiles.Count; i++) {
                
                Tile tile = myColumnTiles[i];
                
                if(!tile){break;}
                
                if (tile.drop) {
                    Tile undermostEmpty = GetUndermostEmptyTile();
                    if (undermostEmpty && undermostEmpty.coordinate.y < tile.coordinate.y) {
                        list.Add(undermostEmpty);
                        _gameManager.Tweening(tile.drop,undermostEmpty);
                        tile.drop = null;
                    }
                }
                
            }
            
            if (outoSpawn) {
                // for (int i = 0; i < emptyCount; i++) {
                //     //if you want outospawn work even when inactive so call _CreateDrop();
                //     CreateDrop();
                // }
            }
            
        }
    
        private Drop.dropColors GetRandomColor() {
            Drop.dropColors color;
            color = (Drop.dropColors) typeof(Drop.dropColors).GetRandomEnumValue();
            return color;
        }

    }

}
