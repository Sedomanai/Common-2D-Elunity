using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Elang
{
    [ExecuteInEditMode]
    public class Camera2D : MonoBehaviour
    {
        [HideInInspector]
        public Camera2DTrigger trigger = null;

        Camera _cam;
        Vector2 _size;

        Vector3 _left, _right;
        Vector3 _top, _bottom;

        [HideInInspector]
        public Camera2DFlagsX flagX = Camera2DFlagsX.Empty();
        [HideInInspector]
        public Camera2DFlagsY flagY = Camera2DFlagsY.Empty();
        Vector3 _ceilLeft, _ceilRight;
        Vector3 _floorLeft, _floorRight;

        public Vector2 Size {
            get { return _size; }
        }
        public float Ceiling {
            get { return _ceilLeft.y; }
        }
        public float Floor {
            get { return _floorLeft.y; }
        }

#if UNITY_EDITOR
        void OnDrawGizmos() {
            UpdatePositions();

            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position + transform.forward, transform.forward, 2); // radius 2

            Handles.color = (flagY.freeze) ? Color.gray : Color.cyan;
            Handles.DrawLine(_ceilLeft, _ceilRight);
            Handles.color = (flagY.freeze) ? Color.gray : Color.blue;
            Handles.DrawLine(_floorLeft, _floorRight);

            Handles.color = Color.yellow;
            if (flagX.freeze && flagX.snap) {
                var snapX = flagX.snapPoint;
                Handles.DrawLine(new Vector3(snapX, 1000, 0), new Vector3(snapX, -1000, 0));
            }
            if (flagY.freeze && flagY.snap) {
                var snapY = flagY.snapPoint;
                Handles.DrawLine(new Vector3(1000, snapY, 0), new Vector3(-1000, snapY, 0));
            }

        }
#endif

        void Awake() {
            _cam = gameObject.AddComponentIfNone<Camera>();
            _cam.orthographic = true;
            _cam.orthographicSize = 5;
            float height = _cam.orthographicSize * 2;
            float width = height * _cam.aspect;
            _size = new Vector2(width, height);
        }

        void Update() {
            UpdatePositions();
        }

        void UpdatePositions() {
            var halfX = _size.x / 2.0f;
            var halfY = _size.y / 2.0f;

            var pos = transform.position + transform.forward;
            _left = pos;
            _left.x -= halfX;
            _right = pos;
            _right.x += halfX;
            _bottom = pos;
            _bottom.y -= halfY;
            _top = pos;
            _top.y += halfY;

            Vector3 ceil = new Vector3(0, halfY, 0);
            Vector3 floor = new Vector3(0, -halfY, 0);
            ceil.y -= flagY.ceiling;
            floor.y += flagY.floor;

            _ceilLeft = _left + ceil;
            _ceilRight = _right + ceil;
            _floorLeft = _left + floor;
            _floorRight = _right + floor;
        }

#if UNITY_EDITOR

        [CustomEditor(typeof(Camera2D))]
        public class Camera2DEditor : Editor
        {
            public Camera2D cam { get { return target as Camera2D; } }

            void OnEnable() {
                cam.gameObject.SetLayer("Camera 2D");
            }
        }
#endif
    }
}