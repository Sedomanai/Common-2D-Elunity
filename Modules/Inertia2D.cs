using UnityEngine;
using System.Collections;

//기본 동선/관성 스크립트
//위치, 변속, 기울기 등에 지속적인 변화를 준다
//물리 충돌이 없는 간단한 오브젝트에 사용

namespace Elang {
    public class Inertia2D : MonoBehaviour {
        public bool isMoving = true;
        public Vector2 moveSpeed;
        public Vector2 moveAccel;
        public float rotateSpeed;
        public bool rotateToMovement;
        public bool resetOnDisable;
        //public Transform faceTransform;

        void OnDisable() {
            if (resetOnDisable)
                ResetValues();
        }

        public void ResetValues() {
            moveSpeed = new Vector2(0.0f, 0.0f);
            moveAccel = new Vector2(0.0f, 0.0f);
            rotateSpeed = 0.0f;
        }

        // Update is called once per frame
        void Update() {
            if (isMoving) {
                Move();
            }
        }

        virtual protected void Move() {
            if (rotateSpeed != 0)
                transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime * 50);

            if (moveAccel != Vector2.zero)
                moveSpeed += moveAccel * Time.deltaTime;

            if (moveSpeed != Vector2.zero) {
                transform.Translate(moveSpeed * Time.deltaTime, Space.World);
            }

            if (rotateToMovement) {
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(moveSpeed.y, moveSpeed.x));
            }
        }

        public void ChangeAngle(float angle) {
            var mag = moveSpeed.magnitude;
            moveSpeed = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            moveSpeed.Normalize();
            moveSpeed *= mag;
        }

        public void SyncAccelDirection() {
            var amag = moveAccel.magnitude;
            moveAccel = moveSpeed.normalized;
            moveAccel *= amag;
        }
    }
}