using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * 이동 스크립트
 * 횡스크롤도 된다
 * 
 */

namespace Elang
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class TopDown2D : MonoBehaviour
    {
        
        protected Vector2 _axis;
        protected BoxCollider2D _box;
        protected Rigidbody2D _body;

        public bool acceptControls = true;
        public float speed = 5;

        void setupBody() {
            _body = gameObject.GetComponent<Rigidbody2D>();
            _body.angularDrag = 0;
            _body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _body.sleepMode = RigidbodySleepMode2D.StartAwake;
            _body.interpolation = RigidbodyInterpolation2D.Interpolate;
            _body.freezeRotation = true;
        }

        void setupBox() {
            _box = gameObject.GetComponent<BoxCollider2D>();
            _box.isTrigger = false;

            // Rigidbody to rigidbody collider that shouldn't accept 2d physics force. (i.e. should not be pushed)
            // Kinematic copy of the original collider that interacts only with other rigidbodies.
            // The specifics should then be set via turning off certain layer collisions.
            var obj = new GameObject();
            obj.name = "R2R Collider";
            obj.transform.position = transform.position;
            obj.transform.SetParent(transform);

            var copybox = obj.AddComponent<BoxCollider2D>();
            copybox.size = _box.size;
            copybox.offset = _box.offset;

            var body = obj.AddComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Kinematic;
            obj.SetLayer("Player Static Collider");

            //// Ignore collision with the original (yeets into stratosphere otherwise)
            //Physics2D.IgnoreCollision(copybox, _box);
        }

        protected virtual void Awake() {
            setupBox();
            setupBody();

            _body.gravityScale = 0;
            //_mask.Add("Wall", "Enemy", "Player", "Collider");
            //_mask.Remove(LayerMask.LayerToName(gameObject.layer));
        }

        void Update() {
            _axis.x = acceptControls ? Input.GetAxis("Horizontal") * speed : 0.0f;
            _axis.y = acceptControls ? Input.GetAxis("Vertical") * speed : 0.0f;

            _body.velocity = _axis;
        }
    }

}