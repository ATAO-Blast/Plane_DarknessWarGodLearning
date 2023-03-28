using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarknessWarGodLearning
{
    public class LoopDragonAni : MonoBehaviour
    {
        private Animation ani;
        private void Awake()
        {
            ani = GetComponent<Animation>();
        }
        void Start()
        {
            if (ani != null)
            {
                InvokeRepeating("PlayDragonAni", 0, 20);
            }
        }
        void PlayDragonAni()
        {
            if(ani != null)
            {
                ani.Play();
            }
        }
        
    }
}