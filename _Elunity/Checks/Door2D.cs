using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Elang 
{
    [RequireComponent(typeof(Animator))]
    public class Door2D : CheckListener2D
    {
        [SerializeField]
        Transform destination;
        [SerializeField]
        string _openDoorState;

        Animator _anim;
        void Awake() {
            _anim = GetComponent<Animator>();
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.DownArrow) && !TimeMgr.Instance.Paused && checker) {
                OnCheck.Invoke();
                _anim.Play(_openDoorState);
            }
        }

        public void NextState() {
            var pos = destination.transform.position;
            checker.transform.position = new Vector3(pos.x, pos.y, checker.transform.position.z);
        }
    }
}
