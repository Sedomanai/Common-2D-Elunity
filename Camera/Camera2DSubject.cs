using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Elang
{
    public class Camera2DSubject : MonoBehaviour
    {
        [SerializeField]
        Camera2D _camera2d;
        public Camera2D camera2d { get { return _camera2d; } }
        
        Camera2DFlagsX _flagX = Camera2DFlagsX.Empty();
        Camera2DFlagsY _flagY = Camera2DFlagsY.Empty();

        public void ResetTrigger() {
            _flagX = Camera2DFlagsX.Empty();
            _flagY = Camera2DFlagsY.Empty();
        }

        public void AcceptTrigger(Camera2DFlagsX fx) {
            if (!TimeMgr.Instance.Paused)
                _flagX = _camera2d.flagX = fx;
        }
        public void AcceptTrigger(Camera2DFlagsY fy) {
            if (!TimeMgr.Instance.Paused)
                _flagY = _camera2d.flagY = fy;
        }

        void recalculateX(float xVelocity) {
            var pos = _camera2d.transform.position;
            var smooth = _flagX.smoothing * xVelocity;
            float target;
            if (_flagX.freeze) {
                target = (_flagX.snap) ? _flagX.snapPoint : pos.x;
            } else {
                target = transform.position.x;
            }
            target -= pos.x;
            target = Mathf.Clamp(target, -smooth, smooth);

            _camera2d.transform.position += new Vector3(target, 0, 0);
        }


        void recalculateY(float yVelocity) {
            var pos = _camera2d.transform.position;
            var smooth = _flagY.smoothing * yVelocity;
            float target;
            if (_flagY.freeze) {
                target = (_flagY.snap) ? _flagY.snapPoint : pos.y;
            } else {
                target = transform.position.y;

                if (target > _camera2d.Ceiling) {
                    target = pos.y + target - _camera2d.Ceiling;
                } else if (target < _camera2d.Floor) {
                    target = pos.y + target - _camera2d.Floor;
                } else
                    target = pos.y;
            }
            target -= pos.y;
            target = Mathf.Clamp(target, -smooth, smooth);

            _camera2d.transform.position += new Vector3(0, target, 0);
        }

        void LateUpdate() {
            recalculateY(1.0f);
            recalculateX(1.0f);
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(Camera2DSubject))]
    public class Camera2DSubjectEditor : Editor
    {
        void OnEnable() {
            (target as Camera2DSubject).gameObject.SetLayer("Camera Subject 2D");
        }
    }

#endif
}