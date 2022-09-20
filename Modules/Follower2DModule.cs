using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elang
{
    public class Follower2DModule : MonoBehaviour
    {
        public Transform target;
        public float velocity;
        public float acceleration;
        public float vincinity;

        public bool OnTarget { get; private set; }

        public void FollowWith(Inertia2D inertia) {
            Vector2 c = target.transform.position - transform.position;
            if (c.magnitude > vincinity && !TimeMgr.Instance.Paused) {
                inertia.rotateToMovement = true;
                inertia.moveSpeed = new Vector2(0, velocity);
                inertia.moveAccel = new Vector2(0, acceleration);
                inertia.ChangeAngle(Mathf.Atan2(c.y, c.x));
                inertia.SyncAccelDirection();
                OnTarget = false;
            } else {
                inertia.rotateToMovement = false;
                inertia.ResetValues();
                OnTarget = true;
            }
        }
    }
}