using System;
using DG.Tweening;
using SO_Scripts.Managers;
using UnityEngine;
using Utilities.RecycleGameObject;

namespace Game {

    public class Drop : MonoBehaviour,IRecyle 
    {

        public enum dropColors {
            //these enums's queue must same as drop sprites's queue
            Yellow,
            Blue,
            Green,
            Red,
        }
        
        public dropColors color { get; private set; }
        
        public void FirstInitialize(dropColors color) {
            this.color = color;
            Restart();
        }
        
        public void Restart() {
            SetSpriteRenderer();
        }
        
        public void Shutdown() {
            //TODO call explosion effect and add some score to player...
        }
        
        private void SetSpriteRenderer() {
            GetComponent<SpriteRenderer>().sprite = MasterManager.dropSprites[(int) color];
        }

        public void Explode() {
            //TODO create explosion effect
            GameObjectUtil.Destroy(gameObject);
        }

    }

}
