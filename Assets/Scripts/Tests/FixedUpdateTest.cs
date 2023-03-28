using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarknessWarGodLearning.Test
{
    public class FixedUpdateTest : MonoBehaviour
    {
        private float lastFixedUpdate = 0f;
        private float lastUpdateDuration = 0f;

        private void FixedUpdate()
        {
            var cur = Time.realtimeSinceStartup;
            lastUpdateDuration = cur - lastFixedUpdate;
            lastFixedUpdate = cur;
            Debug.Log("UpdateDuration£º" + lastUpdateDuration.ToString());
            Debug.Log("FixedDeltatime£º" + Time.fixedDeltaTime.ToString());
        }
    }
}