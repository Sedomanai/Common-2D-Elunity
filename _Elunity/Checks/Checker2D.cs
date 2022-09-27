using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Elang
{

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Checker2D : MonoBehaviour {
        [SerializeField]
        GameObject _rootObject;
        HashSet<CheckListener2D> _hash = new HashSet<CheckListener2D>();
        List<CheckListener2D> _list = new List<CheckListener2D>();
        CheckListener2D _current = null;

        void Awake() {
            if (!_rootObject)
                _rootObject = gameObject;
            var box = GetComponent<BoxCollider2D>();
            box.isTrigger = true;
            var rigid = GetComponent<Rigidbody2D>();
            rigid.bodyType = RigidbodyType2D.Kinematic;
            rigid.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rigid.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        void OnTriggerEnter2D(Collider2D collision) {
            var listener = collision.GetComponent<CheckListener2D>();
            if (listener && !_hash.Contains(listener)) { // just a double check
                _hash.Add(listener);
                _list.Add(listener);
                Swap();
            }
        }

        void OnTriggerExit2D(Collider2D collision) {
            var listener = collision.GetComponent<CheckListener2D>();
            if (listener && _hash.Contains(listener)) {
                _hash.Remove(listener);
                _list.Remove(listener); 
                Swap();
            }
        }

        void Swap() {
            var expected = _list.LastOrDefault();
            if (_current != expected) {
                if (_current)
                    _current.ExitListen(_rootObject);
                if (expected)
                    expected.EnterListen(_rootObject);
            }
            _current = expected;
        }

        public void Check() {
            _current.Listen(_rootObject);
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