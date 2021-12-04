using System.Collections.Generic;
using Game;
using UnityEditor;
using UnityEngine;
using Utilities;
using Utilities.RecycleGameObject;

namespace SO_Scripts.Managers {

    public class BoardManager : ScriptableObject {

        private static int rowCount = 8;
        private static int columnCount = 8;
        private Vector2 tileAspectLength;
        [SerializeField] private GameObject originPoint;
        private List<Tile> _tiles; //indexing  x*ColumnCount + y


        public void Initialize() {
            ResetBoard();
        }

        private void ResetBoard() {
            _tiles = PopulateTiles(rowCount, columnCount);
        }

        List<Tile> PopulateTiles(int rowCount, int columnCount) {

            List<Tile> tempList = new List<Tile>();

            //row*column times create tiles
            for (int y = 0; y < rowCount; y++) {
                for (int x = 0; x < columnCount; x++) {
                    
                    //first rows creating so indexing is Ex.(3,7) = 3*columnCount + 7
                    Tile tile = CreateTile(x, y);
                    if (tile != null) {
                        tempList.Add(tile);
                    } else {
                        Debug.LogWarning("Tile Creating fail");
                    }
                    
                }
            }

            if (tempList.Contains(null)) {
                Debug.LogWarning("Tile Board fail");
            }

            return tempList;
        }

        Tile CreateTile(int x, int y) {
            GameObject prefab = Resources.Load<GameObject>("tile");
            var instance = GameObjectUtil.Instantiate(prefab, CalculatePosition(x, y));
            instance.name = instance.name = x + ", " + y;
            return instance.GetComponent<Tile>();
        }

        Vector3 CalculatePosition(int x, int y) {
            return new Vector3(tileAspectLength.x * x, tileAspectLength.y * y, 0);
        }

        public Tile GetTile(int x, int y) {
            return _tiles[x * columnCount + y];
        }
        

    }

}
