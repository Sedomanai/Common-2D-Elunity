using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elang
{
    public class Arrow2DModule : MonoBehaviour
    {
        public Transform target;
        public float velocity;
        public Vector2 marginOfError;

        public Vector2 FireVelocity {
            get {
                Vector2 delta = target.transform.position - transform.position;
                delta += new Vector2(Random.Range(-marginOfError.x, marginOfError.x), Random.Range(-marginOfError.y, marginOfError.y));
                var time = (velocity == 0.0f) ? 0.001f : delta.magnitude / velocity;
                float ay = Physics2D.gravity.y;
                var xVelocity = KinematicEquation.U_SAT(delta.x, 0.0f, time);
                var yVelocity = KinematicEquation.U_SAT(delta.y, ay, time);
                return new Vector2(xVelocity, yVelocity);
            }
        }

        public void FireWith(Inertia2D inertia) {
            inertia.moveSpeed = FireVelocity;
            inertia.SyncAccelDirection();
            inertia.moveAccel += Physics2D.gravity;
        }
    }
}