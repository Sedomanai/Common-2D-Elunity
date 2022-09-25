using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Elang
{ 
    [RequireComponent(typeof(BoxCollider2D))]
    public class Camera2DTrigger : MonoBehaviour
    {
        [SerializeField]
        Camera2DSubject _subject;

        BoxCollider2D _box;

        [SerializeField]
        bool _changeX;
        [SerializeField]
        bool _changeY;
        [SerializeField]
        Camera2DFlagsX _flagX = new Camera2DFlagsX();
        [SerializeField]
        Camera2DFlagsY _flagY = new Camera2DFlagsY();

        [SerializeField]
        Transform _snapPoint;

        void Awake() {
            _box = gameObject.GetComponent<BoxCollider2D>();
            _box.isTrigger = true;
            _flagX.snapPoint = _snapPoint ? _snapPoint.position.x : _flagX.snapPoint;
            _flagY.snapPoint = _snapPoint ? _snapPoint.position.y : _flagY.snapPoint;
        }

        void Update() {
            if (_box.bounds.Contains(_subject.transform.position)) {
                if (_changeX)
                    _subject.AcceptTrigger(_flagX);
                if (_changeY)
                    _subject.AcceptTrigger(_flagY);
            }
        }
        public Vector3 Center {
            get { return _box.bounds.center; }
        }


#if UNITY_EDITOR
        [CustomEditor(typeof(Camera2DTrigger))]
        public class Camera2DTriggerEditor : Editor {
            public Camera2DTrigger cam { get { return target as Camera2DTrigger; } }

            void OnEnable() {
                cam.gameObject.SetLayer("Camera Trigger 2D");
            }
            public override void OnInspectorGUI() {
                //base.OnInspectorGUI();
                serializedObject.Update();
                EditorGUI.BeginChangeCheck();

                if (EditorGUI.EndChangeCheck()) {
                    EditorUtility.SetDirty(cam);
                }

                cam._subject = EditorGUILayout.ObjectField("Camera Subject", cam._subject, typeof(Camera2DSubject), true) as Camera2DSubject;
                cam._snapPoint = EditorGUILayout.ObjectField("Snap Transform", cam._snapPoint, typeof(Transform), true) as Transform;
                cam._changeX = EditorGUILayout.Toggle("Change X Policy", cam._changeX);
                if (cam._changeX)
                    cam._flagX.OnInspector(!cam._snapPoint);

                cam._changeY = EditorGUILayout.Toggle("Change Y Policy", cam._changeY);
                if (cam._changeY) {
                    cam._flagY.OnInspector(!cam._snapPoint);
                    cam._flagY.OnInspector2();
                }

                serializedObject.ApplyModifiedProperties();
            }
        }
#endif

        //void Update() {
        //   if (_moving) {
        //       var sub = _subject.transform;
        //       var cam = _subject.camera2d.transform;

        //       Vector2 target = sub.position;
        //       Vector2 current = cam.position;

        //       if (_freezeX) {
        //           target.x = _snapPoint ? _snapPoint.position.x : _box.bounds.center.x;
        //       }
        //       if (_freezeY) {
        //           target.y = _snapPoint ? _snapPoint.position.y : _box.bounds.center.y;
        //       }

        //       var delta = target - current;
        //       if (delta.magnitude < _transitionSpeed) {
        //           cam.position = (Vector3)target + new Vector3(0, 0, cam.position.z);
        //       } else {
        //           cam.position += (Vector3)(delta.normalized * _transitionSpeed);
        //       }
        //   }
        //}
    }


    //    public class Camera2DTrigger : MonoBehaviour
    //    {
    //        // Class declaration
    //        [System.Serializable]
    //        public class Camera2DTriggerCompleteEvent : UnityEvent<Vector3> { }

    //        public enum Direction
    //        {
    //            Vertical,
    //            Horizontal
    //        }
    //        public enum CurrentDirection
    //        {
    //            None,
    //            Vertical,
    //            Horizontal
    //        }
    //        [SerializeField]
    //        Direction _triggerDirection = Direction.Vertical;
    //        [SerializeField]
    //        Transform _leftOrTopTarget;
    //        [SerializeField]
    //        Transform _rightOrBottomTarget;
    //        [SerializeField]
    //        Camera2DTriggerCompleteEvent onCompleteEvent;

    //        BoxCollider2D _box;
    //        public Direction triggerDirection { get { return _triggerDirection; } }
    //        bool _moving = false;

    //        Vector3 _expectingPos = new Vector3(Mathf.Infinity, 0, 0);

    //        void OnTriggerStay2D(Collider2D col) {
    //            var subject = col.GetComponent<Camera2DSubject>();

    //            if (subject) {
    //                Transform tempTarget = null;
    //                switch (_triggerDirection) {
    //                    case Direction.Vertical:
    //                        tempTarget = (subject.transform.position.y > transform.position.y) ? _leftOrTopTarget : _rightOrBottomTarget;
    //                        break;
    //                    case Direction.Horizontal:
    //                        tempTarget = (subject.transform.position.x < transform.position.x) ? _leftOrTopTarget : _rightOrBottomTarget;
    //                        break;
    //                }

    //                //if (_expectingPos != tempTarget.position) {
    //                _expectingPos = tempTarget.position;
    //                subject.camera2d.transform.DOMove(_expectingPos + new Vector3(0, 0, -10), 0.3f).OnComplete(() => {
    //                    onCompleteEvent.Invoke(_expectingPos);
    //                });
    //                //}
    //            }
    //        }

    //        void Awake() {
    //            _box = gameObject.AddComponentIfNone<BoxCollider2D>();
    //            _box.isTrigger = true;
    //        }

    //#if UNITY_EDITOR

    //        [CustomEditor(typeof(Camera2DTrigger))]
    //        public class Camera2DTriggerEditor : Editor
    //        {
    //            private Camera2DTrigger trigg { get { return (target as Camera2DTrigger); } }
    //            void OnEnable() {
    //                trigg.gameObject.SetLayer("Camera Trigger 2D");
    //            }
    //            public override void OnInspectorGUI() {
    //                serializedObject.Update();
    //                EditorGUI.BeginChangeCheck();

    //                trigg._triggerDirection = (Direction)EditorGUILayout.EnumPopup(trigg._triggerDirection, "Trigger Direction");

    //                string lt, rb;
    //                if (trigg._triggerDirection == Direction.Vertical) {
    //                    lt = "Top Target"; rb = "Bottom Target";
    //                } else {
    //                    lt = "Left Target"; rb = "Right Target";
    //                }

    //                EditorGUI.indentLevel++;
    //                trigg._leftOrTopTarget = (Transform)EditorGUILayout.ObjectField(lt, trigg._leftOrTopTarget, typeof(Transform), true);
    //                trigg._rightOrBottomTarget = (Transform)EditorGUILayout.ObjectField(rb, trigg._rightOrBottomTarget, typeof(Transform), true);
    //                EditorGUI.indentLevel--;

    //                EditorGUILayout.PropertyField(serializedObject.FindProperty("onCompleteEvent"), true);
    //                if (EditorGUI.EndChangeCheck()) {
    //                    EditorUtility.SetDirty(trigg);
    //                }

    //                serializedObject.ApplyModifiedProperties();

    //            }

    //        }

    //#endif
    //}
}