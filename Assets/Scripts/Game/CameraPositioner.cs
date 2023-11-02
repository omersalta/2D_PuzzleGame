using UnityEngine;

namespace Game
{
    public class CameraPositioner : MonoBehaviour
    {
        [SerializeField] private BoardOriantionPoint _boardOriantionPoint;
        private float sizeMargin = 1f;
        public void Position()
        {
            CalculateSize();
        }
    
        public void CalculateSize()
        {
            //the Bounds object we want the camera to follow
            Bounds targetBounds = _boardOriantionPoint.GetBoardBound();
        
            float screenRatio = (float)Screen.width / (float)Screen.height;
            float targetRatio = targetBounds.size.x / targetBounds.size.y;

            if (screenRatio >= targetRatio)
            {
                Camera.main.orthographicSize = targetBounds.size.y / 2;
            }
            else
            { 
                float differenceInSize = targetRatio / screenRatio;
                Camera.main.orthographicSize = targetBounds.size.y / 2 * differenceInSize;
            }

            Camera.main.orthographicSize += sizeMargin;
            transform.position = new Vector3(targetBounds.center.x, targetBounds.center.y, -1f);
        }
    }
}
