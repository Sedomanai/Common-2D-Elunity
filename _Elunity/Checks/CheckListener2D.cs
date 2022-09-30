using UnityEngine;
using UnityEngine.InputSystem;

namespace Elang
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CheckListener2D : MonoBehaviour {
        [SerializeField]
        GameObject _rootObject;
        [SerializeField]
        InputActionReference _action;

        public event CheckDelegate OnListen, OnEnter, OnExit, OnStay;

        void Awake() {
            GetComponent<BoxCollider2D>().isTrigger = true;
            if (!_rootObject)
                _rootObject = gameObject;
        }

        public void CheckListen(CheckDelegate check, GameObject checkerRoot) {
            if (_action != null && _action.action.triggered) {
                check?.Invoke(_rootObject, _action);
                OnListen?.Invoke(checkerRoot, _action);
            }
        }

        public void CheckEnter(CheckDelegate enter, GameObject checkerRoot) {
            enter?.Invoke(_rootObject);
            OnEnter?.Invoke(checkerRoot);
        }

        public void CheckExit(CheckDelegate exit, GameObject checkerRoot) {
            exit?.Invoke(_rootObject);
            OnExit?.Invoke(checkerRoot);
        }

        public void CheckStay(CheckDelegate stay, GameObject checkerRoot) {
            stay?.Invoke(_rootObject);
            OnStay?.Invoke(checkerRoot);
        }

        //public delegate void OnCheckDelegate(GameObject other);
        //public OnCheckDelegate OnCheck, OnEnter, OnExit;

//#if UNITY_EDITOR
//        [CustomEditor(typeof(CheckListener2D))]
//        public class Checkable2DEditor : Editor
//        {
//            public CheckListener2D check { get { return target as CheckListener2D; } }

//            void OnEnable() {
//                check.gameObject.SetLayer("Check Listener");
//            }
//        }
//#endif
    }
}
