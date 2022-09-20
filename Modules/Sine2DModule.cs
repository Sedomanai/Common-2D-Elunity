using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elang
{
    public class Sine2DModule : MonoBehaviour
    {
        public float amplitude = 1;
        public float frequency = 1;
        public float phaseShift;

        float _time = 0.0f;

        public float SineValue {
            get {
                return amplitude * Mathf.Sin(frequency * _time + phaseShift);
            }
        }

        public void TimeStep() {
            _time += Time.deltaTime;
        }

    }
}