using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;
using Game;
using Utilities.RecycleGameObject;

public class Spawner : MonoBehaviour {

    public int columnNo { get; private set; }
    private GameObject _dropPrefab;
    public bool Activate;
    private List<Tile> myColumnTiles;

    public int GetCurrentTileCount() {
        return myColumnTiles.Count;
    }

    public void AddTile(Tile tile) {
        myColumnTiles.Add(tile);
        //after every adding ordering coordinate y values
        myColumnTiles.OrderBy(x=>x.coordinate.y).ToList();
    }
    
    public void Initialize(int myColumnNo , GameObject dropPrefab) {
        _dropPrefab = dropPrefab;
        columnNo = myColumnNo;
        Activate = true;
        myColumnTiles = new List<Tile>();
    }
    
    public Drop CreateDrop() {
        
        GameObject dropGO;
        dropGO = GameObjectUtil.Instantiate(_dropPrefab, transform.localPosition);
        dropGO.transform.localPosition = transform.position;
        Drop drop = dropGO.GetComponent<Drop>();
        drop.FirstInitialize(GetRandomColor());
            
        return drop.GetComponent<Drop>();
    }

    // public FallColumnDropsIfExist() {
    //     
    // }
    
    
    private Drop.eColor GetRandomColor() {
        Drop.eColor color;
        color = (Drop.eColor) typeof(Drop.eColor).GetRandomEnumValue();
        return color;
    }
}
