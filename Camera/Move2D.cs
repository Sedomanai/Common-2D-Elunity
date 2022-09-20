using UnityEngine;
using DG.Tweening;

namespace Elang {
    public class Move2D : MonoBehaviour {
        public bool acceptMovement = true;

        public KeyCode left = KeyCode.A;
        public KeyCode right = KeyCode.D;
        public KeyCode up = KeyCode.W;
        public KeyCode down = KeyCode.S;

        public float horizontalSpeed = 0.1f;
        public float verticalSpeed = 0.1f;

        public Vector2 Direction {
            get {
                var dir = new Vector2();
                if (acceptMovement) {
                    if (Input.GetKey(left)) {
                        dir += new Vector2(-horizontalSpeed, 0.0f);
                    } else if (Input.GetKey(right)) {
                        dir += new Vector2(horizontalSpeed, 0.0f);
                    }
                    if (Input.GetKey(down)) {
                        dir += new Vector2(0.0f, -verticalSpeed);
                    } else if (Input.GetKey(up)) {
                        dir += new Vector2(0.0f, verticalSpeed);
                    }
                }
                return dir;
            }
        }

        public void Move(Transform transform) {
            transform.position += (Vector3)Direction;
        }
        public void Move(Rigidbody2D body) {
            body.velocity = Direction;
        }
        public void Move(Inertia2D inertia) {
            inertia.moveSpeed = Direction;
        }
    }
}

