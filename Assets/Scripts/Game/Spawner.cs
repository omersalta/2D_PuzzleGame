using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;
using Game;
using SO_Scripts.Managers;
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
        myColumnTiles.OrderByDescending(x=>x.coordinate.y).ToList();
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
        myColumnTiles[0].drop = drop;
        FallDrops();
        return drop.GetComponent<Drop>();
        
    }

     public void FallDrops() {
         int currentIndex = -1;
         foreach (Tile tile in myColumnTiles) {
             currentIndex++;
             int filledCount = 0;
             int emptyCount = 0;
             for (int i = currentIndex; i < myColumnTiles.Count-1; i++) {
                 if (myColumnTiles[i + 1].drop == null) {
                     emptyCount++;
                 } else {
                     filledCount++;
                 }
             }
             
             tile.drop?.MoveTo(myColumnTiles[currentIndex + emptyCount]);
             //MasterManager.boardManager.GetTile()
         }
     }
    
    
    private Drop.eColor GetRandomColor() {
        Drop.eColor color;
        color = (Drop.eColor) typeof(Drop.eColor).GetRandomEnumValue();
        return color;
    }
}
