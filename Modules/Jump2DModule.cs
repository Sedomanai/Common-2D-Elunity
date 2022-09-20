using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elang
{   
    public class Jump2DModule : MonoBehaviour
    {
        public float jumpHeight = 4;
        public float jumpWidth = 0;
        public uint jumpCount = 1;
        public bool JustJumped { get; private set; }
        public bool Jumping { get; private set; }
        public float AbsoluteYAxis { get; private set; }

        uint _jumpCountLeft;

        // for scrolling maps


        public float JumpVelocity { get { return KinematicEquation.U_SVA(jumpHeight, 0.0f, Physics2D.gravity.y); } }

        public bool IsGround { get { return _jumpCountLeft == jumpCount; } }

        public UnityEvent onHitGround, onJump;

        public void ApplyGravityTo(Inertia2D inertia) {
            inertia.moveAccel = Physics2D.gravity;
        }
        public bool JumpWith(ref Vector2 axis) {
            if (_jumpCountLeft > 0 && !TimeMgr.Instance.Paused) {
                JustJumped = Jumping = true;
                _jumpCountLeft--;
                axis.x += jumpWidth;
                AbsoluteYAxis = axis.y = JumpVelocity;
                onJump.Invoke();
                return true;
            } return false;
        }

        public bool JumpWith(Inertia2D inertia) {
            if (_jumpCountLeft > 0 && !TimeMgr.Instance.Paused) {
                JustJumped = Jumping = true;
                inertia.moveSpeed.x = jumpWidth;
                inertia.moveSpeed.y = JumpVelocity;
                _jumpCountLeft--;
                onJump.Invoke();
                return true;
            } return false;
        }


        public void JumpPreCheck(bool hitGround) {
            JustJumped = false;
            AbsoluteYAxis += Physics2D.gravity.y * Time.deltaTime;
            if (hitGround) {
                AbsoluteYAxis = 0.0f;
                Jumping = false;
                _jumpCountLeft = jumpCount;
                onHitGround.Invoke();
            } else if (!Jumping) {
                _jumpCountLeft = jumpCount - 1;
            }
        }
        public void HitGround(Inertia2D inertia) {
            inertia.moveSpeed.x = 0.0f;
            //JustJumped = true;
        }
    }
}