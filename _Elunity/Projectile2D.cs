using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elang
{
    public class Projectile2D : MonoBehaviour
    {
        [SerializeField]
        GameObject _rootObject;

        public enum eProjectileState { Preheat, Firing, Hit, Done }
        public eProjectileState beginState;
        public LayerMask targetMask;
        public LayerMask wallMask;
        public bool passThrough = false;

        public UnityEvent OnFire;
        public UnityEvent OnTargetHit;
        public UnityEvent OnWallHit;

        public event CheckDelegate OnTargetHitCheck, OnWallHitCheck;

        public string preState, fireState, hitState;

        public Transform origin { get; set; }

        eProjectileState _state;
        Animator _anim;


        void Awake() {
            _anim = GetComponent<Animator>();
            if (!_rootObject) _rootObject = gameObject;
        }

        void OnEnable() {
            _state = beginState;
            if (_state == eProjectileState.Preheat && _anim) {
                _anim.Play(preState, -1, 0.0f);
            } else if (_state == eProjectileState.Firing) {
                if (_anim)
                    _anim.Play(fireState, -1, 0.0f);
                OnFire.Invoke();
            }
        }

        public void NextState() {
            if (_state == eProjectileState.Preheat) {
                _state = eProjectileState.Firing;
                if (_anim)
                    _anim.Play(fireState);
                OnFire.Invoke();
            } else if (_state == eProjectileState.Firing) {
                _state = eProjectileState.Hit; 
                if (_anim)
                    _anim.Play(hitState);
            } else if (_state == eProjectileState.Hit) {
                _state = eProjectileState.Done;
                gameObject.SetActive(false);
            }
        }

        void OnTriggerStay2D(Collider2D collision) {
            if (_state == eProjectileState.Firing) {
                var layer = 1 << collision.gameObject.layer;
				if ((targetMask.value & layer) == layer) {
                    Hit();
                    OnTargetHit.Invoke();
                    OnTargetHitCheck?.Invoke(_rootObject);
                } else if ((wallMask.value & layer) == layer) {
                    Hit();
                    OnWallHit.Invoke();
                    OnWallHitCheck?.Invoke(_rootObject);
                }
            }
        }

        void Hit() {
            if (!passThrough) {
                _state = eProjectileState.Hit;
                if (_anim)
                    _anim.Play(hitState);
            }
        }
    }
}