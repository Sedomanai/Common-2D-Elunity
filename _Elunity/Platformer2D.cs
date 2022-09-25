using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elang
{

    [RequireComponent(typeof(Jump2DModule))]
    public class Platformer2D : TopDown2D
    {
        public KeyCode jumpKey;
        protected Jump2DModule _jump;
        public LayerMask mask;

        override protected void Awake() {
            base.Awake();
            _jump = GetComponent<Jump2DModule>();
            _body.gravityScale = 1.0f;
            mask.Add("Wall");
        }

        // Update is called once per frame
        void Update() {
            UpdatePlatformer(true);
        }

        protected void UpdatePlatformer(bool acceptJump) {
            _axis.x = acceptControls ? Input.GetAxisRaw("Horizontal") * speed * TimeMgr.Instance.TimeScale : 0.0f;
            _axis.y = _body.velocity.y;

            if (acceptJump) {
                var halfWidth = new Vector3(_box.size.x / 2.0f - 0.02f, 0, 0);
                var distance = _box.size.y / 2.0f + 0.02f;
                Vector3 center = _box.bounds.center;
                RaycastHit2D mhit = Physics2D.Raycast(center, Vector2.down, distance, mask);
                RaycastHit2D lhit = Physics2D.Raycast(center - halfWidth, Vector2.down, distance, mask);
                RaycastHit2D rhit = Physics2D.Raycast(center + halfWidth, Vector2.down, distance, mask);
                _jump.JumpPreCheck(mhit || lhit || rhit);


                Debug.DrawRay(center, Vector2.down * distance, Color.green);
                Debug.DrawRay(center - halfWidth, Vector2.down * distance, Color.green);
                Debug.DrawRay(center + halfWidth, Vector2.down * distance, Color.green);

                if (acceptControls) {
                    if (Input.GetKeyDown(jumpKey)) {
                        _jump.JumpWith(ref _axis);
                    }
                }

            }
            _body.velocity = _axis;
        }
    }

}