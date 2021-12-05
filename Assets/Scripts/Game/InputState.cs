using UnityEngine;
using UnityEngine.Serialization;

namespace Game {

    public class InputState : MonoBehaviour {

        public bool swipeLeft, swipeRight, swipeUp, swipeDown, isDraging;
        private bool _tap;
        private Vector2 _startTouch, _swipeDelta;
    
        private void Update() {
            _tap = swipeLeft = swipeRight = swipeUp = swipeDown = isDraging = false;

            #region Standalone Input

            //click
            if (Input.GetMouseButtonDown(0)) {
                _tap = true;
                isDraging = true;
                _startTouch = Input.mousePosition;
            }
            //release
            else if (Input.GetMouseButtonUp(0)) {
                isDraging = false;
                Reset();
            }

            #endregion

            #region Moblie Inputs

            if (Input.touches.Length > 0) {
                if (Input.touches[0].phase == TouchPhase.Began) {
                    isDraging = true;
                    _tap = true;
                    _startTouch = Input.touches[0].position;
                } else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled) {
                    isDraging = false;
                    Reset();
                }

            }

            #endregion

            #region Swipe Check

            _swipeDelta = Vector2.zero;

            if (isDraging) {
                if (Input.touches.Length > 0) { _swipeDelta = Input.touches[0].position - _startTouch; } else if (
                    Input.GetMouseButton(0)) {
                    _swipeDelta = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - _startTouch;
                }
            }

            if (_swipeDelta.magnitude > 125) {
                //which direction are we swiping in
                float x = _swipeDelta.x;
                float y = _swipeDelta.y;

                if (Mathf.Abs(x) > Mathf.Abs(y)) {
                    //left or right
                    if (x < 0) { swipeLeft = true; } else { swipeRight = true; }

                } else {
                    //up or down
                    if (y < 0) { swipeDown = true; } else { swipeUp = true; }
                }


                Reset();
            }

            #endregion

            void Reset() {
                _swipeDelta = Vector2.zero;
                isDraging = false;
            }
        }

    }

}
