using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Elang
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Checker2D : MonoBehaviour {
        [SerializeField]
        GameObject root;

        public bool checking = true;

        void OnTriggerEnter2D(Collider2D collision) {
            var listener = collision.GetComponent<CheckListener2D>();
            if (listener && listener.checker != this) { // just a double check
                listener.OnEnter.Invoke();
                listener.checker = root;
            }
        }

        void OnTriggerExit2D(Collider2D collision) {
            var listener = collision.GetComponent<CheckListener2D>();
            if (listener && listener.checker == root) {
                listener.OnExit.Invoke();
                listener.checker = null;
            }
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(Checker2D))]
        public class Checker2DEditor : Editor
        {
            public Checker2D check { get { return target as Checker2D; } }
            void OnEnable() {
                check.gameObject.SetLayer("Checker");
            }
        }
#endif
    }
}