using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;
using Utilities.RecycleGameObject;

namespace Game {

    public class Spawner : MonoBehaviour {

        public int columnNo { get; private set; }
        private GameObject _dropPrefab;
        public bool Activate = true;
        [SerializeField]
        private List<Tile> myColumnTiles;
        private static GameManager _gameManager;
        private int dropCountBeforeLastExplosion;

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

        private void UpdateRendererColor() {
            
            if (Activate) {
                GetComponent<SpriteRenderer>().color = Color.green;
            } else {
                GetComponent<SpriteRenderer>().color = Color.red;
            }

        }
    
        public void OnMouseUpAsButton() {
            Debug.Log("onActivatebutton in spawner :" + columnNo );
            Activate = !Activate;
            UpdateRendererColor();
        }

        public void Initialize(int myColumnNo, GameObject dropPrefab) {
            _dropPrefab = dropPrefab;
            columnNo = myColumnNo;
            myColumnTiles = new List<Tile>();
            UpdateRendererColor();
        }
    
        public void Initialize(int myColumnNo, GameObject dropPrefab, bool activationState) {
            Activate = activationState;
            Initialize(myColumnNo, dropPrefab);
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
            dropGO.transform.localPosition = transform.localPosition;
            Drop drop = dropGO.GetComponent<Drop>();
            drop.FirstInitialize(GetRandomColor());
            Tile EmptyTile = GetUndermostEmptyTile();
            Debug.Log("emptyTile is ="+EmptyTile.coordinate);
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

        private int GetCurrentEmptyCount() {
            int result = 0;
            
            foreach (var tile in myColumnTiles) {
                if (tile.drop == null) {
                    result++;
                }
            }

            return result;
        }
        
        public void FallDropsAndFillList(List<Tile> list,bool outoSpawn) {
            //it fall all drops need to drop and fill given list with last moved drop for cheking again
            int empty = 0;
            //Falling Drops
            for (int i = 0; i < myColumnTiles.Count; i++) {
                
                Tile tile = myColumnTiles[i];
                
                if(!tile){break;}
                
                if (tile.drop) {
                    Tile undermostEmpty = GetUndermostEmptyTile();
                    if (undermostEmpty && undermostEmpty.coordinate.y < tile.coordinate.y) {
                        list.Add(undermostEmpty);
                        empty++;
                        _gameManager.Tweening(tile.drop,undermostEmpty);
                        tile.drop = null;
                    }
                }
                
            }
            
            Debug.Log("Spawner Columno :"+columnNo +" starting outo create emptyCount:"+GetCurrentEmptyCount());
            int createCount = dropCountBeforeLastExplosion - GetCurrentDropCount();
            if (outoSpawn) {
                for (int i = 0; i < createCount; i++) {
                    //if you want outospawn work even when inactive so call _CreateDrop();
                    CreateDrop();
                }
            }
            
        }
        
        private Drop.dropColors GetRandomColor() {
            Drop.dropColors color;
            color = (Drop.dropColors) typeof(Drop.dropColors).GetRandomEnumValue();
            return color;
        }

        private int GetCurrentDropCount() {
            return myColumnTiles.Count - GetCurrentEmptyCount();
        }

        public void SetDropCountBeforeExplosion() {
            dropCountBeforeLastExplosion = GetCurrentDropCount();
        }

    }

}
