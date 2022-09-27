using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

namespace Elang {
    public class Move2D : MonoBehaviour {
        public float horizontalSpeed = 5.0f;
        public float verticalSpeed = 5.0f;

        [SerializeField]
        InputActionReference _moveInputAction;

        public Vector2 Direction {
            get {
                return _moveInputAction.action.ReadValue<Vector2>() * new Vector2(horizontalSpeed, verticalSpeed);
            }
        }

        public void Move(Transform transform) {
            transform.position += (Vector3)Direction * Time.deltaTime;
        }
        public void Move(Rigidbody2D body) {
            body.velocity = Direction;
        }
        public void Move(Inertia2D inertia) {
            inertia.moveSpeed = Direction * Time.deltaTime;
        }
    }
}

