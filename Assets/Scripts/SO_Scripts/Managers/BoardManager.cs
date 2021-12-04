using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEditor;
using UnityEngine;
using Utilities;
using Utilities.RecycleGameObject;

namespace SO_Scripts.Managers {

    [CreateAssetMenu(menuName = "SO/BoardManager")]
    public class BoardManager : ScriptableObject {

        //theese are need to drag and drop from inspector
        [SerializeField] private GameObject _spawnerPrefab;
        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private GameObject[] _dropPrefabs;

        private GameObject _originPoint;
        private GameObject _content;
        private static int rowCount = 8;
        private static int columnCount = 8;
        private Vector2 tileAspectLength;
        private List<Spawner> _spawners;
        private List<Tile> _tiles; //indexing  x*ColumnCount + y
        
        public void FirstInitialize() {
            _originPoint = GameObject.FindWithTag("BoardOrigin");
            _content = _originPoint.transform.GetChild(0).gameObject;
            tileAspectLength = findAspectLengths(_tilePrefab);
            Debug.Log("this massage will output after awake");
            ResetBoard();
            ModifyOriginPosition();
        }

        void ModifyOriginPosition() {
            //TODO calculate with coding not manuel 
            _originPoint.transform.position = new Vector3(-10, -9, 0);
        }

        Vector2 findAspectLengths(GameObject tilePrefab) {
            SpriteRenderer renderer = tilePrefab.GetComponent<SpriteRenderer>();
            return new Vector2(renderer.bounds.size.x, renderer.bounds.size.y);
        }

        void ResetBoard() {
            _tiles = PopulateTiles(rowCount, columnCount);
        }

        List<Tile> PopulateTiles(int rowCount, int columnCount) {

            List<Tile> tempList = new List<Tile>();
            //find content object of board (for adding tiles as child of content)
            
            Debug.Log(_content.name);
            //row*column times create tiles
            for (int y = 0; y < rowCount; y++) {
                for (int x = 0; x < columnCount; x++) {
                    
                    //first rows creating so indexing is Ex.(3,7) = 3*columnCount + 7
                    Tile tile = CreateTile(x, y, _content);
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

        List<Spawner> PopulateSpawners(int columnCount) {
            
            List<Spawner> tempList = new List<Spawner>();
            
            for (int i = 0; i < columnCount; i++) {
                tempList.Add(CreateSpawner(i));
            }

            return tempList;
        }

        Spawner CreateSpawner(int columnNumber) {
            if (_spawners.Any(x => x.columnNo == columnNumber)) { 
                //if already exist same columnNo spawner
                Debug.LogWarning("this columnNumber's Spawner is already exist");
                return null;
            } else {
                //Instantiation
                GameObject instance;
                instance = Instantiate(_spawnerPrefab);
                instance.transform.parent = instance.transform;
                Spawner spawner = instance.GetComponent<Spawner>();
                spawner.Initialize(columnNumber);
                return spawner;
            }
        }

        Tile CreateTile(int x, int y, GameObject content) {
            var instance = GameObjectUtil.Instantiate(_tilePrefab, CalculatePosition(x, y));
            instance.name = instance.name = x + ", " + y;
            instance.transform.parent = content.transform;
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
