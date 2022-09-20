using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elang
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Projectile2D : MonoBehaviour
    {
        public enum eProjectileState { Preheat, Firing, Hit, Done }
        public eProjectileState beginState;
        public LayerMask targetMask;
        public LayerMask wallMask;

        public UnityEvent onFire;
        public UnityEvent onTargetHit;
        public UnityEvent onWallHit;

        public string preState, fireState, hitState;

        public Transform origin { get; set; }


        eProjectileState _state;
        Animator _anim;


        void Awake() {
            _anim = GetComponent<Animator>();
        }

        void OnEnable() {
            _state = beginState;
            if (_state == eProjectileState.Preheat) {
                _anim?.Play(preState);
            } else if (_state == eProjectileState.Firing) {
                _anim?.Play(fireState);
                onFire.Invoke();
            }
        }

        public void NextState() {
            if (_state == eProjectileState.Preheat) {
                _state = eProjectileState.Firing;
                _anim?.Play(fireState);
                onFire.Invoke();
            } else if (_state == eProjectileState.Firing) {
                _state = eProjectileState.Hit;
                _anim?.Play(hitState);
            } else if (_state == eProjectileState.Hit) {
                _state = eProjectileState.Done;
                gameObject.SetActive(false);
            }
        }

        public void OnTriggerStay2D(Collider2D collision) {
            if (!TimeMgr.Instance.Paused && _state == eProjectileState.Firing) {
                var layer = 1 << collision.gameObject.layer;
                if ((targetMask.value & layer) == layer) {
                    Hit();
                    onTargetHit.Invoke();
                }
                if ((wallMask.value & layer) == layer) {
                    Hit();
                    onWallHit.Invoke();
                }
            }
        }

        public void Hit() {
            _state = eProjectileState.Hit;
            _anim?.Play(hitState);
        }
    }
}