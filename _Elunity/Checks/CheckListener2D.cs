using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Elang
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CheckListener2D : MonoBehaviour
    {
        [SerializeField]
        GameObject _rootObject;
        [SerializeField]
        InputActionReference _action;

        void Awake() {
            GetComponent<BoxCollider2D>().isTrigger = true;
            if (!_rootObject)
                _rootObject = gameObject;
        }

        public void Listen(GameObject checkerRootObject) {
            if (_action.action.triggered) {
                _rootObject.SendMessage("OnCheckListen", checkerRootObject);
            }
        }

        public void EnterListen(GameObject checkerRootObject) {
            _rootObject.SendMessage("OnCheckEnter", checkerRootObject);
        }
        public void ExitListen(GameObject checkerRootObject) {
            _rootObject.SendMessage("OnCheckExit", checkerRootObject);
        }

        //public delegate void OnCheckDelegate(GameObject other);
        //public OnCheckDelegate OnCheck, OnEnter, OnExit;

#if UNITY_EDITOR
        [CustomEditor(typeof(CheckListener2D))]
        public class Checkable2DEditor : Editor
        {
            public CheckListener2D check { get { return target as CheckListener2D; } }

            void OnEnable() {
                check.gameObject.SetLayer("Check Listener");
            }
        }
#endif
    }
}
