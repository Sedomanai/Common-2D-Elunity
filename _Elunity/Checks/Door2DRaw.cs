using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elang {

    public class Door2DRaw : CheckListener2D
    {
        [SerializeField]
        Transform destination;
        [SerializeField]
        float _waitSeconds;

        void Update() {
            //if (Input.GetKeyDown(KeyCode.DownArrow) && !TimeMgr.Instance.Paused && checker) {
            //    OnCheck.Invoke();
            //    StartCoroutine(CoOpen());
            //}
        }

        IEnumerator CoOpen() {
            yield return new WaitForSecondsRealtime(_waitSeconds);
            //if (destination) {
            //    var pos = destination.transform.position;
            //    checker.transform.position = new Vector3(pos.x, pos.y, checker.transform.position.z);
            //} else {
            //    Debug.Log("Door has no destination?..");
            //}
            
        }
    }

}