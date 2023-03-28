using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DarknessWarGodLearning.Test
{
    public class TimeZoneTest : MonoBehaviour
    {
        private DateTime startDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private double nowTime;
        private void Start()
        {
            nowTime = (DateTime.UtcNow - startDateTime).TotalMilliseconds;
        }
        private void OnGUI()
        {
            if (GUILayout.Button("DiaPlay Time"))
            {
                var dt2 = TimeZoneInfo.ConvertTimeFromUtc(startDateTime.AddMilliseconds(nowTime), TimeZoneInfo.Local);
                Debug.Log(dt2.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }
    }
}