using System.Collections.Generic;
using DG.Tweening;
using SO_Scripts.Managers;
using UnityEngine;
using Utilities;
using Utilities.RecycleGameObject;

namespace Game {

    public class GameManager : MonoBehaviour
    {
        enum State {
            INITIAL,
            MOVE,
            ANIMATION,
            GAME_OVER,
        };
        
        private State _currentState;
        private int animationCounter = 0;
        private Tile firstTile, secondTile; //its for reverse move if no explosion
        private bool isThereExplosion; //if first move has no explosion use it
        private List<Tile> lastMovedDropsTiles = new List<Tile>();

        private void Awake() {
            _currentState = State.MOVE;
            DOTween.Init();
            Spawner.SetGameManager(this);
            Drop.SetGameManager(this);
        }
        
        private void Update() {
        
            switch (_currentState) {
                //////////////////////////////////////////
                //////////////////////////////////////////
                case State.INITIAL:
                    //TODO game manager initilization state if its has long time
                    _currentState = State.MOVE;
                    break;
                //////////////////////////////////////////
                //////////////////////////////////////////
                case State.MOVE:
                    //TODO do if gonna do extra something when choosing move 
                    break;
                //////////////////////////////////////////
                //////////////////////////////////////////
                case State.ANIMATION:
                    //TODO do if gonna do extra something when Tweening
                    break;
                //////////////////////////////////////////
                //////////////////////////////////////////
                case State.GAME_OVER:
                    //TODO show game over panel and make recycle the game to play again
                    break;
            }
        
        }

        public void Tweening(Drop drop, Tile targetTile) {
            _currentState = State.ANIMATION;
            animationCounter++;
            lastMovedDropsTiles.Add(targetTile);
            drop.transform.DOLocalMove(targetTile.transform.localPosition, 1.4f).SetEase(Ease.InOutSine)
                .OnComplete(OnCompleteTween);
            targetTile.drop = drop;
        }
        
        public void TweeningSwitch(Tile first, Tile second) {
            _currentState = State.ANIMATION;
            animationCounter+=2;
            lastMovedDropsTiles.Add(first);
            lastMovedDropsTiles.Add(second);
            first.drop.transform.DOLocalMove(second.transform.localPosition, 0.6f).SetEase(Ease.InOutSine)
                .OnComplete(OnCompleteTween);
            second.drop.transform.DOLocalMove(first.transform.localPosition, 0.6f).SetEase(Ease.InOutSine)
                .OnComplete(OnCompleteTween);
            first.SwitchDrops(second);
        }

        public void ExplosionTween(Drop drop) {
            drop.transform.DOScale(2f, 0.8f);
            drop.myRenderer.DOFade(0, 1f).OnComplete(() => OnCompleteExpTween(drop));
        }
        
        public void ReverseExplosionTweenSuddenly(Drop drop) {
            drop.transform.DOScale(1f, 0f);
            drop.myRenderer.DOFade(1, 0f);
        }

        private void OnCompleteExpTween(Drop drop) {
            GameObjectUtil.Destroy(drop.gameObject);
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
        
        public void MoveReverse () {
            isThereExplosion = true;
            TweeningSwitch(firstTile,secondTile);
        }

        public void TweeningFakeMove(Drop drop, Picker.Direction dir ) {
            
            _currentState = State.ANIMATION;
            animationCounter++;
            Vector3 v3;
            Transform t = drop.transform;
            
            switch (dir) {
                case Picker.Direction.RIGHT://rigth
                    v3 = Vector3.right;
                    break;
                case Picker.Direction.DOWN://down
                    v3 = Vector3.down;
                    break;
                case Picker.Direction.LEFT://left
                    v3 = Vector3.left;
                    break;
                case Picker.Direction.UP://up
                    v3 = Vector3.up;
                    break;
                default:
                    Debug.LogWarning("unknown direction value");
                    v3 = Vector3.zero;
                    break;
            }
            t.DOMove(t.position + v3, 0.2f).SetEase(Ease.InOutSine)
                .SetLoops(2,LoopType.Yoyo).OnComplete(OnCompleteTween);
        }
        
        public void Move(Tile firts, Tile second) {
            
            if (_currentState != State.MOVE) {
                return;
            }
            
            firstTile = firts;
            secondTile = second;
            isThereExplosion = false;
            TweeningSwitch(firstTile,secondTile);
        }

        public void OnInitialize() {
            
            foreach (var tile in MasterManager.boardManager.GetTileList()) {
                if (tile && tile.drop) {
                    while (tile.Check()) {
                        tile.drop.FirstInitialize((Drop.dropColors) typeof(Drop.dropColors).GetRandomEnumValue());
                    }
                }
            }
            isThereExplosion = true;
            lastMovedDropsTiles.Clear();
        }

        private bool CheckExplosion(List<Tile> chekingList) {

            if (chekingList.Count < 1)
                return false;
            
            //List<Tile> list = MasterManager.boardManager.GetTileList();
            List<Tile> explodeList = new List<Tile>();
            
            foreach (var tile in chekingList) {
                tile.CheckAndFillExplodeList(explodeList);
            }
            
            if (explodeList.Count > 0) {
                ExplodeThese(explodeList);
                isThereExplosion = true;
                return true;
            }
            
            return false;
        }
        
        private void ExplodeThese(List<Tile> explodeList) {
            
            List<Spawner> toTriggerSpawners = new List<Spawner>();
            
            foreach (var tile in explodeList) {

                Spawner spawner = tile.GetSpawner();
                if (!toTriggerSpawners.Contains(spawner)) {
                    toTriggerSpawners.Add(spawner);
                    spawner.SetDropCountBeforeExplosion();
                }
                
                tile.explodeDrop();
            }
            lastMovedDropsTiles.Clear();
            foreach (var spawner in toTriggerSpawners) {
                spawner.FallDropsAndFillList(lastMovedDropsTiles,true);
            }

            if (lastMovedDropsTiles.Count < 1) {
                _currentState = State.MOVE;
            }
            
        }
        
    }

}
