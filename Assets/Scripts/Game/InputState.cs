using UnityEngine;
using UnityEngine.Serialization;

// public bool swipeLeft, swipeRight, swipeUp, swipeDown, isDraging;
// private bool _tap;
// private Vector2 _startTouch, _swipeDelta;


namespace Game {

    public class InputState : MonoBehaviour {
        
        #region Privates
        
        private bool _tap, _swipeLeft, _swipeRight, _swipeUp, _swipeDown;
        private Vector2 _startTouch, _swipeDelta;
        private Vector2 _currentPos;
        private bool _isDraging = false;
        private Vector2 _downPos;
        private Vector2 _upPos;
        private bool _mouseUp;
        
        #endregion

        #region Publics
        
        public bool swipeDown { get { return _swipeDown; } }
        public Vector2 swipeDelta { get { return _swipeDelta; } }
        
        public bool tap { get { return _tap; } }
        public bool mouseUp { get { return _tap; } }
        
        public bool isDraging { get { return _isDraging; } }
        public bool swipeLeft { get { return _swipeLeft; } }
        public bool swipeRight { get { return _swipeRight; } }
        public bool swipeUp { get { return _swipeUp; } }
        
        #endregion
        
        
        private void Update() {
            _mouseUp = _tap = _swipeLeft = _swipeRight = _swipeUp = _swipeDown = false;

            #region Standalone Inputs

            _currentPos = Input.mousePosition;

            if (Input.GetMouseButtonDown(0)) {
                _tap = true;
                _isDraging = true;
                _downPos = _currentPos;
                _startTouch = Input.mousePosition;
            } else if (Input.GetMouseButtonUp(0)) {
                _mouseUp = true;
                _isDraging = false;
                _upPos = _currentPos;
                Reset();
            }

            #endregion

            #region Mobile Input

            if (Input.touches.Length > 0) {
                if (Input.touches[0].phase == TouchPhase.Moved)
                    _currentPos = Input.touches[0].position;

                if (Input.touches[0].phase == TouchPhase.Began) {
                    _isDraging = true;
                    _tap = true;
                    _downPos = _startTouch = Input.touches[0].position;
                } else if (Input.touches[0].phase == TouchPhase.Ended ||
                           Input.touches[0].phase == TouchPhase.Canceled) {
                    _isDraging = false;
                    _upPos = _currentPos;
                    _mouseUp = true;
                    Reset();
                }
            }

            #endregion

            // Calculate the distance
            _swipeDelta = Vector2.zero;

            if (_isDraging) {
                if (Input.touches.Length > 0)
                    _swipeDelta = Input.touches[0].position - _startTouch;
                else if (Input.GetMouseButton(0))
                    _swipeDelta = (Vector2) Input.mousePosition - _startTouch;
            }

            //Did we cross the distance?
            if (_swipeDelta.magnitude > 50) {
                //Which direction?
                float x = _swipeDelta.x;
                float y = _swipeDelta.y;

                if (Mathf.Abs(x) > Mathf.Abs(y)) {
                    //Left or right
                    if (x < 0)
                        _swipeLeft = true;
                    else
                        _swipeRight = true;
                } else {
                    // Up or down
                    if (y < 0)
                        _swipeDown = true;
                    else
                        _swipeUp = true;
                }

                Reset();
            }

            if (swipeRight)
                Debug.Log("rigt");
            if (swipeLeft)
                Debug.Log("left");
            if (swipeUp)
                Debug.Log("up");
            if (swipeDown)
                Debug.Log("down");
            
        }
        
        

        void Reset() {
            _startTouch = _swipeDelta = Vector2.zero;
            _isDraging = false;
        }

    }

}
