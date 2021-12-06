using System;
using DG.Tweening;
using UnityEngine;

namespace Game {

    public class GameManager : MonoBehaviour
    {
        enum State {
            INITIAL,
            CHOSE_MOVE,
            ANIMATION,
            GAME_OVER,
        };
        
        //private int animationCounter;
        private State _currentState;
        private Sequence _sequence;

        private void Awake() {
            DOTween.Init();
        }

        private void Update() {
        
            switch (_currentState) {
                //////////////////////////////////////////
                //////////////////////////////////////////
                case State.INITIAL:
                    //TODO game manager initilization state if its has long time
                    _currentState = State.CHOSE_MOVE;
                    break;
                //////////////////////////////////////////
                //////////////////////////////////////////
                case State.CHOSE_MOVE:
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

        private void StartTween() {
            _currentState = State.ANIMATION;
            _sequence = DOTween.Sequence();
            //_sequence.onKill(OnStopTween);
        }
        
        private void Tweening(Drop drop, Tile targetTile) {
            _sequence.Join(drop.transform.DOMove(targetTile.transform.position, 1f));
        }
        
        private void OnStopTween() {
           Debug.Log("cheking explosion and calling another tweening after...");
           //_sequence.Complete();
           CheckExplosion();
           _currentState = State.CHOSE_MOVE;
        }

        public void Move(Tile firtsTile, Tile secondTile) {

            if (_currentState != State.CHOSE_MOVE) {
                return;
            }
            
            Debug.Log("move on "+ firtsTile +", "+secondTile);
            StartTween();
            Tweening(firtsTile.drop,secondTile);
            Tweening(secondTile.drop,firtsTile);
            SwitchTile(firtsTile, secondTile);
            //TODO start sequance and after check explosion

        }

        private void SwitchTile(Tile firtsTile, Tile secondTile) {
            firtsTile.SwitchDrops(secondTile);
        }

        private void CheckExplosion() {
            //TODO if there is a explosion after move so; explode these drops and call CreateDrops() 
    
        }

        private void CreateDrops() {
            //TODO get spawners and how many create with a generic pair like KeyValuePair 
        }
        
    }

}
