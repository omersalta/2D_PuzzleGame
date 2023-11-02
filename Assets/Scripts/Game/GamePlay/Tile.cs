using System.Collections.Generic;
using DG.Tweening;
using SO_Scripts.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Game.GamePlay {

    public class Tile : MonoBehaviour {
        
        enum Neighbor_Direction {
            RIGTH,
            DOWN,
            LEFT,
            UP,
        }

        //if the game has more than one player so need to keep list of pickers ex. 
        private static Picker staticPicker;
        public static void SetPicker(Picker picker) {
            staticPicker = picker;
        }
        
        private SpriteRenderer _renderer;
        
        public Vector2Int coordinate { get; private set; }
        public Drop drop;
        
        public void Initialize(int x, int y) {
            coordinate = new Vector2Int(x, y);
        }
        
        public void SwitchDrops(Tile target, UnityAction method)
        {
            drop.transform.DOLocalMove(target.drop.transform.localPosition, 0.6f).SetEase(Ease.InOutSine);
            target.drop.transform.DOLocalMove(drop.transform.localPosition, 0.6f).SetEase(Ease.InOutSine)
                .OnComplete(method.Invoke);
            Drop myBefore = drop;
            drop = target.drop;
            target.drop = myBefore;
        }
        public void explodeDrop() {
            drop?.Explode();
            drop = null;
        }
        public void CheckExplodeList(List<Tile> list) {

            int enterCount = list.Count;
            
            if (CheckDir(Neighbor_Direction.RIGTH)) {
                if (CheckDir(Neighbor_Direction.LEFT)) {
                    CheckLine(this,Neighbor_Direction.RIGTH,list,1);
                } else {
                    CheckLine(this, Neighbor_Direction.RIGTH,list,0);
                }
            }
            
            if (CheckDir(Neighbor_Direction.LEFT)) {
                if (CheckDir(Neighbor_Direction.RIGTH)) {
                    CheckLine(this, Neighbor_Direction.LEFT,list,1);
                } else {
                    CheckLine(this, Neighbor_Direction.LEFT,list,0);
                }
            }
            
            if (CheckDir(Neighbor_Direction.UP)) {
                if (CheckDir(Neighbor_Direction.DOWN)) {
                    CheckLine(this, Neighbor_Direction.UP,list,1);
                } else {
                    CheckLine(this, Neighbor_Direction.UP,list,0);
                }
            }
            
            if (CheckDir(Neighbor_Direction.DOWN)) {
                if (CheckDir(Neighbor_Direction.UP)) {
                    CheckLine(this, Neighbor_Direction.DOWN,list,1);
                } else {
                    CheckLine(this, Neighbor_Direction.DOWN,list,0);
                }
            }
            
            
            if (list.Count != enterCount) { //if something added to explode list the node one must added 
                list.Add(this);
            }
            
            
        }

        public Spawner GetSpawner() {
            return MasterManager.boardManager.AskSpawner(this);
        }

        #region Private Methods
        private void OnMouseDown() {
            staticPicker.PickTile(this);
        }
        private Tile GetTile(int x, int y) {
            return MasterManager.boardManager.GetTile(x, y);
        }
        private void CheckLine(Tile tile, Neighbor_Direction checkingDir, List<Tile> list, int stack) {

            Tile neighbor = tile.GetNeighbor(checkingDir);

            if (!neighbor) {
                return;
            }
               
            if (tile.drop?.color == neighbor.drop?.color) { //İF SAME COLOR
                
                if (stack > 0) { //if it coming stacked call must be added
                    list.Add(neighbor);
                } else {
                    if (neighbor.CheckDir(checkingDir)) { //if return true for one step further so; must be added
                        list.Add(neighbor);
                    }
                }
                CheckLine(neighbor, checkingDir, list, stack + 1);
            }
        }
        private bool CheckDir(Neighbor_Direction checkingDir) {
            Tile neighbor = GetNeighbor(checkingDir);

            if (!neighbor || !neighbor.drop || !drop) {
                return false;
            }

            if (GetNeighbor(checkingDir)?.drop?.color == drop?.color) {
                return true;
            }
                
            return false;
        }
        private Tile GetNeighbor(Neighbor_Direction neighborNeighborDirection) {
            
            Tile returnTile = null;
            
            switch (neighborNeighborDirection) {
                
                case Neighbor_Direction.RIGTH:
                    returnTile = GetTile(coordinate.x + 1, coordinate.y);
                    break;
                case Neighbor_Direction.DOWN:
                    returnTile = GetTile(coordinate.x, coordinate.y-1);
                    break;
                case Neighbor_Direction.LEFT:
                    returnTile = GetTile(coordinate.x-1, coordinate.y);
                    break;
                case Neighbor_Direction.UP:
                    returnTile = GetTile(coordinate.x, coordinate.y+1);
                    break;
                
            }

            return returnTile;
        }
        #endregion
        
    }

}
