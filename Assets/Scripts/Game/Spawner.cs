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

        public int GetCurrentTileCount() { return myColumnTiles.Count; }

        public void AddTile(Tile tile) {
        
            if (myColumnTiles.Any(t => t.coordinate.y == tile.coordinate.y)) {
                Debug.LogWarning("there is already same level tile");
            }
        
            myColumnTiles.Insert(0, tile);

            //after every adding ordering coordinate y values (y = 0 is the last one)
            myColumnTiles = myColumnTiles.OrderByDescending(tile => tile.coordinate.y).ToList();

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
            EmptyTile.drop = drop;
            drop.SetTile(EmptyTile);
        
        }

        private Tile GetUndermostEmptyTile() {
            //its return undermost empty tile
            for (int i = myColumnTiles.Count - 1; -1 < i; i--) {
                if (myColumnTiles[i].drop == null) {
                    return myColumnTiles[i];
                }
            }

            Debug.LogWarning("there is no empty tile");
            return null;
        }

        public void FallDrops() {
            int currentIndex = -1;

            foreach (Tile tile in myColumnTiles) {
                currentIndex++;
                int filledCount = 0;
                int emptyCount = 0;

                for (int i = currentIndex; i < myColumnTiles.Count - 1; i++) {
                    if (myColumnTiles[i + 1].drop == null) { emptyCount++; } else { filledCount++; }
                }

                tile.drop?.SetTile(myColumnTiles[currentIndex + emptyCount]);
            
            }
        }
    
        private Drop.dropColors GetRandomColor() {
            Drop.dropColors color;
            color = (Drop.dropColors) typeof(Drop.dropColors).GetRandomEnumValue();
            return color;
        }

    }

}
