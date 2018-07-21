using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineRunner
{
    public class RainObstacle : Obstacle
    {

        public GameObject[] rainDrops;


        private Vector3[] m_LocalRainDropPositions;


        private void Awake()
        {
            m_LocalRainDropPositions = new Vector3[rainDrops.Length];

            for (int i = 0; i < rainDrops.Length; i++)
            {
                m_LocalRainDropPositions[i] = rainDrops[i].transform.localPosition;
            }
        }


        public override void OnSpawn()
        {
            if (m_LocalRainDropPositions == null) return;

            for (int i = 0; i < m_LocalRainDropPositions.Length; i++)
            {
                rainDrops[i].transform.localPosition = m_LocalRainDropPositions[i];
            }
        }
    }
}