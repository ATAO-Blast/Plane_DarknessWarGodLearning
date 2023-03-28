using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarknessWarGodLearning.Test
{
    public class PowerLimitTest : MonoBehaviour
    {
        public int lv;

        private void OnGUI()
        {
            if (GUILayout.Button("output"))
            {
                Debug.Log(2 ^ 2);
            }
        }
    }
}