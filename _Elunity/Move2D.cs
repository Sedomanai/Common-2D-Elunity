using UnityEngine;
using UnityEngine.InputSystem;

namespace Elang {
    public class MoveInput2D : MonoBehaviour {
        public float horizontalSpeed = 5.0f;
        public float verticalSpeed = 5.0f;

        [SerializeField]
        InputActionReference _moveInputAction;

        Vector2 _direction;

        public Vector2 Direction {
            set { _direction = value; }
            get {
                return (_moveInputAction ?
                    _moveInputAction.action.ReadValue<Vector2>() * new Vector2(horizontalSpeed, verticalSpeed) :
                    _direction);
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

