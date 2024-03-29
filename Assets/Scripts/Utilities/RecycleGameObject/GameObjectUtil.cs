﻿using System.Collections.Generic;
using SO_Scripts.Managers;
using UnityEngine;

namespace Utilities.RecycleGameObject {

    public class GameObjectUtil {
        private static Dictionary<RecycleGameObject, ObjectPool> pools = new Dictionary<RecycleGameObject, ObjectPool>();

    
        public static GameObject Instantiate(GameObject prefab, Vector3 pos) {
            GameObject instance = null;
            var recycleScript = prefab.GetComponent<RecycleGameObject>();

            if (recycleScript != null) {
                var pool = GetObjectPool(recycleScript);
                instance = pool.NextObject(pos).gameObject;
            }
            else {
                instance = GameObject.Instantiate(prefab);
                instance.transform.position = pos;
            }

            return instance;
        }

        public static void Destroy(GameObject gameObject) {
            var recycleGameObject = gameObject.GetComponent<RecycleGameObject>();

            if (recycleGameObject != null) {
                recycleGameObject.Shutdown();
            }
            else {
                GameObject.Destroy(gameObject);
            }
        }
    
        private static ObjectPool GetObjectPool(RecycleGameObject reference) {
            ObjectPool pool = null;
            if (pools.ContainsKey(reference)) {
                pool = pools[reference];
            }
            else {
                var poolContainer = new GameObject(reference.gameObject.name + "ObjectPool");
                
                if (reference.gameObject.tag == "Drop") {
                    poolContainer.transform.parent = MasterManager.boardManager._oriantPoint.transform;
                }
                
                pool = poolContainer.AddComponent<ObjectPool>();
                pool.prefab = reference;
                pools.Add(reference, pool);
            }

            return pool;
        }
    }

}