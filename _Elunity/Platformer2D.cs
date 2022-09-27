using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Elang
{

    [RequireComponent(typeof(Jump2DModule))]
    public class Platformer2D : TopDown2D
    {
        [SerializeField]
        InputActionReference _jumpAction;

        public LayerMask mask;
        protected Jump2DModule _jump;

        override protected void Awake() {
            base.Awake();
            _jump = GetComponent<Jump2DModule>();
            _body.gravityScale = 1.0f;
            mask.Add("Wall");
        }

        // Update is called once per frame
        void Update() {
            UpdatePlatformer();
        }

        protected void UpdatePlatformer() {
            _axis.x = Direction.x;
            _axis.y = _body.velocity.y;
            
            if (_jumpAction.action.enabled) {
                var halfWidth = new Vector3(_box.size.x / 2.0f - 0.02f, 0, 0);
                var distance = _box.size.y / 2.0f + 0.02f;
                Vector3 center = _box.bounds.center;
                RaycastHit2D mhit = Physics2D.Raycast(center, Vector2.down, distance, mask);
                RaycastHit2D lhit = Physics2D.Raycast(center - halfWidth, Vector2.down, distance, mask);
                RaycastHit2D rhit = Physics2D.Raycast(center + halfWidth, Vector2.down, distance, mask);
                _jump.JumpPreCheck(mhit || lhit || rhit);

#if UNITY_EDITOR
                Debug.DrawRay(center, Vector2.down * distance, Color.green);
                Debug.DrawRay(center - halfWidth, Vector2.down * distance, Color.green);
                Debug.DrawRay(center + halfWidth, Vector2.down * distance, Color.green);
#endif
                if (_jumpAction.action.triggered)
                    _jump.JumpWith(ref _axis);
            }
            _body.velocity = _axis;
        }
    }

}