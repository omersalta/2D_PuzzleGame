using System.Collections.Generic;
using System.Linq;
using Game;
using Unity.Mathematics;
using UnityEngine;
using Utilities.RecycleGameObject;

namespace SO_Scripts.Managers {

    [CreateAssetMenu(menuName = "SO/BoardManager")]
    public class BoardManager : ScriptableObject {

        //theese are need to drag and drop from inspector
        [SerializeField] private GameObject _spawnerPrefab;
        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private GameObject _dropPrefab;

        public GameObject _originPoint { get; private set; }
        private GameObject _contentTiles;
        private GameObject _contentSpawners;
        
        private static int rowCount = 8;
        private static int columnCount = 8;
        private Vector2 tileAspectLength;
        private List<Spawner> _spawners;
        [SerializeField]private List<Tile> _tiles; //indexing  x*ColumnCount + y
        
        public void Initialize() {
            _originPoint = GameObject.FindWithTag("BoardOrigin");
            /*if you run into a problem when finding content object you can handle with spesified method
            but not necessery now*/
            _contentSpawners = _originPoint.transform.GetChild(0).gameObject;
            _contentTiles = _originPoint.transform.GetChild(1).gameObject;
            tileAspectLength = findAspectLengths(_tilePrefab);
            ResetBoard();
            ModifyOriginPosition();
        }
        
        void ModifyOriginPosition() {
            //its for showing all tiles on center of camera
            //TODO calculate with coding not manuel
            Vector3 offset = new Vector3(columnCount * tileAspectLength.x, rowCount * tileAspectLength.x, 0);
            _originPoint.transform.position = -offset/2;
        }

        Vector2 findAspectLengths(GameObject tilePrefab) {
            //its return prefab of tile's width and height values
            //its for when creating tiles calculate distance between
            SpriteRenderer renderer = tilePrefab.GetComponent<SpriteRenderer>();
            return new Vector2(renderer.bounds.size.x, renderer.bounds.size.y);
        }

        void ResetBoard() {
            PopulateSpawners(columnCount);
            PopulateTiles(rowCount, columnCount);
            PopulateDrops();
            FindObjectOfType<GameManager>().Initialize();
        }
        
        void PopulateTiles(int rowCount, int columnCount) {

            int size = rowCount * columnCount;
            _tiles = new List<Tile>(size);
            for (int i = 0; i < size; i++) _tiles.Add(null);
            
            //row*column times create tiles
            for (int y = 0; y < rowCount; y++) {
                for (int x = 0; x < columnCount; x++) {
                    //first rows creating so indexing is Ex.(3,7) = 3 + rowCount * 7
                    Tile tile = CreateTile(x, y, _contentTiles);

                    if (tile != null) {
                        int index = x + (y * rowCount);
                        _tiles.Insert(index, tile);
                        _spawners[x].AddTile(tile); //assign tile to response spawners
                    } else { Debug.LogWarning("Tile Creating fail"); }

                }
            }
            
        }

        void PopulateSpawners(int columnCount) {
            
            _spawners = new List<Spawner>();
            
            for (int i = 0; i < columnCount; i++) {
                _spawners.Add(CreateSpawner(i));
            }
            
        }
        
        Spawner CreateSpawner(int columnNo) {
            
            if (_spawners.Count > 0) {
                if (_spawners.Any(x => x.columnNo == columnNo)) { 
                    //if already exist same columnNo spawner
                    Debug.LogWarning("this columnNumber's Spawner is already exist");
                    return null;
                }
            }
            
            //Instantiation
            GameObject instance;
            Vector3 pos = CalculateSpawnerPos(columnNo);
            instance = Instantiate(_spawnerPrefab,pos,quaternion.identity);
            instance.transform.parent = _contentSpawners.transform;
            Spawner spawner = instance.GetComponent<Spawner>();
            spawner.Initialize(columnNo, _dropPrefab, true);
            return spawner;
            
        }
        
        Vector3 CalculateSpawnerPos(int columnNo) {
            Vector2 t = tileAspectLength;
            return _originPoint.transform.position + new Vector3(t.x * columnNo, rowCount*t.y, 0);
        }

        Tile CreateTile(int x, int y, GameObject content) {
            var instance = GameObjectUtil.Instantiate(_tilePrefab, CalculateTilePosition(x, y));
            instance.name = instance.name = x + ", " + y;
            instance.transform.parent = content.transform;
            Tile tile = instance.GetComponent<Tile>();
            tile.Initialize(x,y);
            return tile;
        }
        
        void PopulateDrops () {
            //this method sould call only when reset board
            foreach (var spawner in _spawners) {
                for (int i = 0; i < spawner.GetCurrentTileCount(); i++) {
                    spawner.CreateDrop();
                }
            }
        }
        
        Vector3 CalculateTilePosition(int x, int y) {
            return new Vector3(tileAspectLength.x * x, tileAspectLength.y * y, 0);
        }
        
        public Tile GetTile(int x, int y) {
            int index = x + (y * rowCount);
            
            if(!IsInRange (x, y) || _tiles.ElementAtOrDefault(index) == null )
                return null;
            return _tiles[index];
        }

        private bool IsInRange(int x, int y) {
            if (x < 0 || x > columnCount - 1 || y < 0 || y > rowCount - 1)
                return false;
            return true;
        }

        public List<Tile> GetTileList() {
            return _tiles;
        }
        
        public Spawner AskSpawner(Tile tile) {
            /*if you run into a problem when finding spawners you can handle with find with loop (columnNo == columnNumber)
            but not necessery now*/
           return _spawners[tile.coordinate.x];
        }
        
    }

}
