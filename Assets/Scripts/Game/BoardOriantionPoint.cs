using UnityEngine;

namespace Game
{
    public class BoardOriantionPoint : MonoBehaviour
    {
        [SerializeField] private GameObject _contentSpawners;
        [SerializeField] private GameObject _contentTiles;
        [SerializeField] private GameObject _clickSpawnerText;
    
        private Vector2 boundMin;
        private Vector2 boundMax;
        private Bounds boardBound;

        public void Initialize()
        {
            _clickSpawnerText.transform.position = _contentSpawners.transform.position;
            Destroy(_clickSpawnerText,3f);
        }

        public GameObject GetSpawnersParent()
        {
            return _contentSpawners;
        }
    
        public GameObject GetTilesParent()
        {
            return _contentTiles;
        }

        public void AddBoardBoundObject(GameObject go)
        {
            SpriteRenderer _sr = go.GetComponent<SpriteRenderer>();
            if(!_sr) return;

            if (boundMin.x > _sr.bounds.min.x) boundMin.x = _sr.bounds.min.x;
            if (boundMin.y > _sr.bounds.min.y) boundMin.y = _sr.bounds.min.y;
        
            if (boundMax.x < _sr.bounds.max.x) boundMax.x = _sr.bounds.max.x;
            if (boundMax.y < _sr.bounds.max.y) boundMax.y = _sr.bounds.max.y;
        
            boardBound.SetMinMax(boundMin,boundMax);
        }

        public Bounds GetBoardBound()
        {
            return boardBound;
        }
    }
}
