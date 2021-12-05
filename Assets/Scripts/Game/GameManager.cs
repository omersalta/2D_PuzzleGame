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

    
        private int animationCounter;
        private State _currentState;

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

                    break;
                //////////////////////////////////////////
                //////////////////////////////////////////
                case State.ANIMATION:

                    break;
                //////////////////////////////////////////
                //////////////////////////////////////////
                case State.GAME_OVER:
                
                    break;
            }
        
        }

        public void Move(Tile firtsTile, Tile secondTile) {
            Debug.Log("move on "+ firtsTile +", "+secondTile);
            //TODO start sequance and after check explosion
        }

        public static void OnAnimationStart() {
        
        }
    
        public static void OnAnimationFinish() {
        
        }

    }

}
