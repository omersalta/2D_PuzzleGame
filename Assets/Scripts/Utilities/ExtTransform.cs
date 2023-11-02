using System;
using System.Linq;
using UnityEngine;

namespace Utilities
{
    public static class Transforms
    {

        public static void DestroyChildren(this Transform t, bool destroyImmediately = false) {
            
            foreach (Transform child in t) {
                if (destroyImmediately) {
                    MonoBehaviour.DestroyImmediate(child.gameObject);
                } else {
                    MonoBehaviour.Destroy(child.gameObject);
                }
            }
            
        }

    }
    
    public static class EnumExtensions
    {
        public static Enum GetRandomEnumValue(this Type t)
        {
            return Enum.GetValues(t)          // get values from Type provided
                .OfType<Enum>()               // casts to Enum
                .OrderBy(e => Guid.NewGuid()) // mess with order of results
                .FirstOrDefault();            // take first item in result
        }
    }

}

