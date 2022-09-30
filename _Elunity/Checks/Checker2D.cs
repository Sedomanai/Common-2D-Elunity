using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

namespace Elang
{
    public delegate void CheckDelegate(GameObject checker, InputAction input = null);

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Checker2D : MonoBehaviour {

        [SerializeField]
        bool _useInputCheck = true;
        [SerializeField]
        bool _useStay = false;
        [SerializeField]
        bool _passThrough;

        [SerializeField]
        GameObject _rootObject;
        HashSet<CheckListener2D> _hash = new HashSet<CheckListener2D>();
        List<CheckListener2D> _list = new List<CheckListener2D>();
        CheckListener2D _current = null;

        public event CheckDelegate OnCheck, OnEnter, OnExit, OnStay;

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
                    _current.CheckExit(OnExit, _rootObject);
                if (expected)
                    expected.CheckEnter(OnEnter, _rootObject);
            }
            _current = expected;
        }

        void Clear() {
            foreach (var listener in _list) {
                if (listener)
                    listener.CheckExit(OnExit, _rootObject);
            } 
            _list.Clear();
            _hash.Clear();
            _current = null;
        }

        void Update() {
            if (_current) {
                if (_useInputCheck) {
                    if (_passThrough) {
                        foreach (var listener in _list) {
                            if (listener)
                                listener.CheckListen(OnCheck, _rootObject);
                        }
                    } else
                        _current.CheckListen(OnCheck, _rootObject);
                }

                if (_useStay) {
                    if (_passThrough) {
                        foreach (var listener in _list) {
                            if (listener)
                                listener.CheckStay(OnStay, _rootObject);
                        }
                    } else
                        _current.CheckStay(OnStay, _rootObject);
                }
            }
        }

        void OnDisable() {
            Clear();
        }


//#if UNITY_EDITOR
//        [CustomEditor(typeof(Checker2D))]
//        public class Checker2DEditor : Editor
//        {
//            public Checker2D check { get { return target as Checker2D; } }

//            void Reset() {
//                check.gameObject.SetLayer("Checker");
//            }
//        }
//#endif
    }
}