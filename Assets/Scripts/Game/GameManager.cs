using System.Collections.Generic;
using DG.Tweening;
using SO_Scripts.Managers;
using UnityEngine;
using Utilities;

namespace Game {

    public class GameManager : MonoBehaviour
    {
        #region Variables and Initialize
        private State _currentState;
        private int animationCounter = 0;
        private Tile firstTile, secondTile; //its for reverse move if no explosion
        private bool isThereExplosion; //if first move has no explosion use it
        private List<Tile> lastMovedDropsTiles = new List<Tile>();
        
        private void Awake() {
            _currentState = State.INITIAL;
            DOTween.Init();
            Spawner.SetGameManager(this);
            Drop.SetGameManager(this);
        }
        
        public void Initialize() {
            
            isThereExplosion = true;
            lastMovedDropsTiles.Clear();
            _currentState = State.MOVE;
            List<Tile> _checkList = new List<Tile>();
            
            foreach (var tile in MasterManager.boardManager.GetTileList()) {
                if (tile && tile.drop) {
                    while (Check(tile)) {
                        tile.drop.Initialize((Drop.dropColors) typeof(Drop.dropColors).GetRandomEnumValue());
                    }
                }
            }
            
            bool Check(Tile tile) {
                _checkList.Clear();
                tile.CheckExplodeList(_checkList);
                if (_checkList.Count > 0)
                    return true;
                return false;
            }
        
        }
        #endregion
        
        #region Public Methods
        public void GoTo(Drop drop, Tile targetTile) {
            _currentState = State.ANIMATION;
            animationCounter++;
            lastMovedDropsTiles.Add(targetTile);
            drop.Move(targetTile,OnCompleteTween);
        }

        public void SwitchTiles(Tile firts, Tile second) {
            
            if (_currentState != State.MOVE) {
                return;
            }
            
            firstTile = firts;
            secondTile = second;
            isThereExplosion = false;
            SwitchTweening(firstTile,secondTile);
        }
        
        public void TweeningFakeMove(Drop drop, Vector3 dir ) {
            
            _currentState = State.ANIMATION;
            animationCounter++;
            drop.FakeMove(dir, OnCompleteTween);
        }
        #endregion
        
        private void SwitchTweening(Tile first, Tile second) {
            _currentState = State.ANIMATION;
            animationCounter++;
            lastMovedDropsTiles.Add(first);
            lastMovedDropsTiles.Add(second);
            first.SwitchDrops(second,OnCompleteTween);
        }
        
        private void OnCompleteTween() {
            animationCounter--;
            if (animationCounter == 0) {
                if (CheckExplosion(lastMovedDropsTiles)) {
                    return;
                } else {
                    if(!isThereExplosion){
                        MoveReverse();
                    }
                }
                _currentState = State.MOVE;
            }
        }
        
        private void MoveReverse () {
            isThereExplosion = true;
            SwitchTweening(firstTile,secondTile);
        }

        #region Explosion Check
        private List<Tile> _explodeList = new List<Tile>();
        private bool CheckExplosion(List<Tile> chekingList) {

            if (chekingList.Count < 1)
                return false;
            
            _explodeList.Clear();
            foreach (var tile in chekingList) {
                tile.CheckExplodeList(_explodeList);
            }
            
            if (_explodeList.Count > 0) {
                ExplodeThese(_explodeList);
                isThereExplosion = true;
                return true;
            }
            
            return false;
        }

        private List<Spawner> _spawners = new List<Spawner>(8);
        
        private void ExplodeThese(List<Tile> explodeList) {
            
            _spawners.Clear();
            foreach (var tile in explodeList)
            {
                Spawner spawner = tile.GetSpawner();
                if (!_spawners.Contains(spawner))
                {
                    spawner.SetDropCountBeforeExplosion();
                    _spawners.Add(spawner);
                }
                tile.explodeDrop();
            }
            lastMovedDropsTiles.Clear();
            
            foreach (Spawner spawner in _spawners) {
                spawner.FallDropsAndFillList(lastMovedDropsTiles);
            }

            if (lastMovedDropsTiles.Count <= 0) {
                _currentState = State.MOVE;
            }
            
        }
        #endregion
        
        enum State {
            INITIAL,
            MOVE,
            ANIMATION,
            GAME_OVER,
        };
        
    }

}
