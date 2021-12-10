using System;
using System.Collections.Generic;
using System.Linq;
using SO_Scripts.Managers;
using UnityEngine;

namespace Game {

    public class Tile : MonoBehaviour {
        
        enum nDirection {
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
        
        public void SwitchDrops(Tile target) {
            Drop myBefore = drop;
            drop = target.drop;
            target.drop = myBefore;
        }
        
        private void OnMouseDown() {
            staticPicker.PickTile(this);
        }

        public void explodeDrop() {
            drop?.Explode();
            drop = null;
        }

        private Tile GetTile(int x, int y) {
            return MasterManager.boardManager.GetTile(x, y);
        }
        
        private void CheckLine(Tile tile, nDirection checkingDir, List<Tile> list, int stack) {

            Tile neighbor = tile.GetNeighbor(checkingDir);

            if (!neighbor) {
                return;
            }
               
            if (tile.drop?.color == neighbor.drop?.color) {
                if (stack > 0) {
                    //if it coming stacked call must be added
                    list.Add(neighbor);
                } else {
                    if (neighbor.CheckDir(checkingDir)) {
                        //if return true for one step further so; must be added
                        list.Add(neighbor);
                    }
                }
                CheckLine(neighbor, checkingDir, list, stack + 1);
            }
        }

        private Tile GetNeighbor(nDirection neighborDirection) {
            
            Tile returnTile = null;
            
            switch (neighborDirection) {
                
                case nDirection.RIGTH:
                    returnTile = GetTile(coordinate.x + 1, coordinate.y);
                    break;
                 case nDirection.DOWN:
                     returnTile = GetTile(coordinate.x, coordinate.y-1);
                    break;
                 case nDirection.LEFT:
                     returnTile = GetTile(coordinate.x-1, coordinate.y);
                    break;
                 case nDirection.UP:
                     returnTile = GetTile(coordinate.x, coordinate.y+1);
                    break;
                
            }

            return returnTile;
        }
        
        private bool CheckDir(nDirection checkingDir) {
            Tile neighbor = GetNeighbor(checkingDir);

            if (!neighbor || !neighbor.drop || !drop) {
                return false;
            }

            if (GetNeighbor(checkingDir)?.drop?.color == drop?.color) {
                return true;
            }
                
            return false;
        }

        public bool Check() {
            //TODO optimize this method (implemented quickly)
            List<Tile> list = new List<Tile>();
            CheckAndFillExplodeList(list);
            if (list.Count > 0)
                return true;
            return false;
        }
        
        public void CheckAndFillExplodeList(List<Tile> list) {

            int enterCount = list.Count;
            
            if (CheckDir(nDirection.RIGTH)) {
                if (CheckDir(nDirection.LEFT)) {
                    CheckLine(this,nDirection.RIGTH,list,1);
                } else {
                    CheckLine(this, nDirection.RIGTH,list,0);
                }
            }
            
            if (CheckDir(nDirection.LEFT)) {
                if (CheckDir(nDirection.RIGTH)) {
                    CheckLine(this, nDirection.LEFT,list,1);
                } else {
                    CheckLine(this, nDirection.LEFT,list,0);
                }
            }
            
            if (CheckDir(nDirection.UP)) {
                if (CheckDir(nDirection.DOWN)) {
                    CheckLine(this, nDirection.UP,list,1);
                } else {
                    CheckLine(this, nDirection.UP,list,0);
                }
            }
            
            if (CheckDir(nDirection.DOWN)) {
                if (CheckDir(nDirection.UP)) {
                    CheckLine(this, nDirection.DOWN,list,1);
                } else {
                    CheckLine(this, nDirection.DOWN,list,0);
                }
            }
            
            
            if (list.Count != enterCount) { //if something added to explode list the node one must added 
                list.Add(this);
            }
            
            
        }

        public Spawner GetSpawner() {
            return MasterManager.boardManager.AskSpawner(this);
        }
    }

}
