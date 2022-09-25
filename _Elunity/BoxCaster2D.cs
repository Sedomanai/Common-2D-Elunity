using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elang
{
    public class BoxCaster2D : MonoBehaviour
    {
        BoxCollider2D _box;

        void Awake() {
            _box = gameObject.AddComponentIfNone<BoxCollider2D>();
        }

        void Move(Vector2 dir, LayerMask mask) {
            ContactFilter2D contact = new ContactFilter2D();
            contact.SetLayerMask(mask);
            List<RaycastHit2D> hits = new List<RaycastHit2D>();
            _box.Cast(new Vector2(dir.x, 0), contact, hits, 1.0f, true);
            if (hits.Count == 0) {
                transform.position += new Vector3(dir.x, 0, 0);
            } else {
                bool right = (dir.x > 0.0f);
                float dx = right ? -Mathf.Infinity : Mathf.Infinity;
                for (int i = 0; i < hits.Count; i++) {
                    dx = right ? Mathf.Max(dx, hits[i].point.x) : Mathf.Min(dx, hits[i].point.x);
                }

                var dirx = (right) ?
                    dx - (transform.position.x + _box.bounds.size.x / 2.0f) - 0.2f :
                    dx - (transform.position.x - _box.bounds.size.x / 2.0f) + 0.2f;
                transform.position += new Vector3(dirx, 0, 0);
            }

            _box.Cast(new Vector2(0, dir.y), contact, hits, 1.0f, true);
            if (hits.Count == 0) {
                transform.position += new Vector3(0, dir.y, 0);
            } else {
                bool up = (dir.y > 0.0f);
                float dy = up ? -Mathf.Infinity : Mathf.Infinity;
                for (int i = 0; i < hits.Count; i++) {
                    dy = up ? Mathf.Max(dy, hits[i].point.y) : Mathf.Min(dy, hits[i].point.y);
                }

                var diry = (up) ?
                    dy - (transform.position.y + _box.bounds.size.y / 2.0f) - 0.2f :
                    dy - (transform.position.y - _box.bounds.size.y / 2.0f) + 0.2f;
                transform.position += new Vector3(0, diry, 0);
            }
        }
    }
}