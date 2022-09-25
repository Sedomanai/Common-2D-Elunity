using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 
// 

namespace Elang {

    /// <summary>
    /// <br> This is just a template class. In reality you want something more fine tuned to your game. </br>
    /// <br> You can either inherit directly from Platformer2D or this SimplePlayer class to get you started. </br>
    /// </summary>
    public class SimplePlayer : Platformer2D
    {
        Animator[] _anims;
        float direction;
        bool _justShot = false;

        [SerializeField]
        bool _haveCamFollow;
        [SerializeField]
        bool initiallyFacingLeft;

        protected override void Awake() {
            base.Awake();
            _anims = gameObject.GetComponentsInChildren<Animator>();
            //_gun = GetComponentInChildren<Shooter>();
            direction = initiallyFacingLeft ? -1.0f : 1.0f;
        }

        void Update() {
            UpdatePlatformer(true);

            //_justShot = false;
            //if (acceptControls && Input.GetAxis("Fire1") > 0.01f && _gun) {
            //    var bullet = _gun.CreateProjectile();
            //    if (bullet) {
            //        _justShot = true;
            //        if (transform.localScale.x > 0) {
            //            bullet.moveSpeed.x = 15.0f * direction;
            //        } else {
            //            bullet.moveSpeed.x = -15.0f * direction;
            //        }
            //    }
            //}
        }

        void LateUpdate() {
            if (!TimeMgr.Instance.Paused) {
                if (_axis.x > 0.01f) {
                    transform.localScale = new Vector3(direction, 1, 1);
                } else if (_axis.x < -0.01f) {
                    transform.localScale = new Vector3(-direction, 1, 1);
                }
            }

            for (uint i = 0; i < _anims.Length; i++) {
                var anim = _anims[i];

                anim.SetFloat("Axis X", _axis.x);
                anim.SetFloat("Axis Y", _axis.y);

                if (_axis.y == _jump.jumpHeight)
                    anim.SetTrigger("Jump");
                

                if (_justShot) {
                    anim.SetTrigger("Shoot");
                }
            }
        }
    }
}