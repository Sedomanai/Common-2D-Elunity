using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Elang {
    //public class Projectile : Inertia2D
    //{
    //    [SerializeField]
    //    LayerMask _hitMask;
    //    [SerializeField]
    //    LayerMask _wallMask;
    //    [SerializeField]
    //    UnityEvent _onHitTrigger;

    //    public Transform origin { get; set; }
    //    SpriteRenderer _rend;
    //    SimpleAnimation _anim;

    //    void Awake() {
    //        var body = gameObject.AddComponentIfNone<Rigidbody2D>();
    //        body.isKinematic = true;

    //        var box = gameObject.AddComponentIfNone<BoxCollider2D>();
    //        box.isTrigger = true;


    //        //gameObject.AddComponentIfNone((ActivationToggle c) => {
    //        //    c.automateOption = ActivationToggle.eInitialization.onEnable;
    //        //    c.type = ActivationToggle.eType.disactivate;
    //        //    c.waitSeconds = 0.5f;
    //        //});

    //        //_anim = gameObject.AddComponentIfNone<SimpleAnimation>();
    //        //_anim.onPreBegin.AddListener(() => { moving = false; });
    //        //_anim.onLoopBegin.AddListener(() => { moving = true; });
    //        //_anim.onLoopEnd.AddListener(() => { moving = false; });
    //        //_anim.onPostEnd.AddListener(() => { gameObject.SetActive(false); });


    //        //var body = gameObject.AddComponent<Rigidbody2D>();
    //        //body.isKinematic = true;

    //        //var box = gameObject.AddComponentIfNone<BoxCollider2D>();
    //        //box.isTrigger = true;
    //    }

    //    public void OnTriggerEnter2D(Collider2D collision) {
    //        //var layer = 1 << collision.gameObject.layer;
    //        //if ((_hitMask.value & layer) == layer) {
    //        //    _onHitTrigger.Invoke();
    //        //    _anim.EndLoop();
    //        //}
    //        //if ((_wallMask.value & layer) == layer) {
    //        //    _anim.EndLoop();
    //        //}
    //    }

    //    public void SetSprite(Sprite spr) {
    //        //_rend.sprite = spr;
    //    }
    //}
}