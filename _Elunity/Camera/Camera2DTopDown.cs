using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elang
{
    public class Camera2DTopDown : MonoBehaviour
    {
        public void MoveTo(Vector3 target) {
            transform.position = target + new Vector3(0, 0, -10);
        }
    }
}