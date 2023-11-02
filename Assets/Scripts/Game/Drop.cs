using DG.Tweening;
using SO_Scripts.Managers;
using UnityEngine;
using UnityEngine.Events;
using Utilities.RecycleGameObject;

namespace Game {

    public class Drop : MonoBehaviour,IRecyle 
    {
        
        private static GameManager _gameManager;

        public static void SetGameManager(GameManager manager) {
            _gameManager = manager;
        }
        
        public dropColors color { get; private set; }
        public SpriteRenderer myRenderer;
        
        public void Initialize(dropColors color) {
            this.color = color;
            Restart();
        }
        
        public void Restart() {
            SetSpriteRenderer();
        }
        
        public void Shutdown() {
            //TODO call explosion effect and add some score to player...
            this.transform.DOScale(1f, 0f);
            this.myRenderer.DOFade(1, 0f);
        }
        
        private void SetSpriteRenderer() {
            myRenderer = GetComponent<SpriteRenderer>();
            myRenderer.sprite = MasterManager.dropSprites[(int) color];
        }

        public void Explode() {
            //TODO create explosion effect
            transform.DOScale(2f, 0.8f);
            myRenderer.DOFade(0, 1f).OnComplete(() => GameObjectUtil.Destroy(gameObject));
        }
        
        public void FakeMove(Vector3 dir, UnityAction OnCompleate) {
            
            transform.DOMove(transform.position + dir, 0.2f)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo).OnComplete(OnCompleate.Invoke);
        }
        
        public void Move(Tile targetTile, UnityAction OnCompleate) {
            
            targetTile.drop = this;
            transform.DOLocalMove(targetTile.transform.localPosition, 1.4f)
                .SetEase(Ease.InOutSine).OnComplete(OnCompleate.Invoke);
        }
        
        public enum dropColors {
            //these enums's queue must same as drop sprites's queue
            Yellow,
            Blue,
            Green,
            Red,
        }

    }

}
