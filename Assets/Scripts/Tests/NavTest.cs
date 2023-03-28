using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DarknessWarGodLearning;

namespace DarknessWarGodLearning.Test
{
    public class NavTest : MonoBehaviour
    {
        public NavMeshAgent agent;
        public MainCityMap mainCityMap;
        private Transform destination;

        private void Start()
        {
            destination = mainCityMap.NpcPosTrans[0];

        }
        private void OnGUI()
        {
            if (GUILayout.Button("Start"))
            {
                agent.SetDestination(destination.position);
            }
        }
    }
}