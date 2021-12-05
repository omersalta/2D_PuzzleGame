using System;
using SO_Scripts.Managers;
using UnityEngine;
using Utilities.RecycleGameObject;

namespace Game {

    public class Drop : MonoBehaviour,IRecyle 
    {

        public enum dropColors {
            Blue,
            Green,
            Red,
            Yellow
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
        
        public void SetTile(Tile target) {
            target.drop = this;
        }
        
        private void SetSpriteRenderer() {
            GetComponent<SpriteRenderer>().sprite = MasterManager.dropSprites[(int) color];
        }
        
    }

}
