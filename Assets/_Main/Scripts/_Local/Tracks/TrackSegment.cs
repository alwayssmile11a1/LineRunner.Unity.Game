using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

namespace LineRunner
{
    public class TrackSegment : MonoBehaviour
    {

        [Header("Path")]
        public Transform startPoint;
        public Transform endPoint;

        [Header("Obstacles")]
        public Obstacle[] possibleObstacles;
        public float[] possibleObstaclePositions;

        [HideInInspector]
        public DefaultPoolObject poolObject;
        

        protected List<Obstacle> m_Obstacles = new List<Obstacle>();

        public void SpawnObstacles()
        {
            if (possibleObstacles.Length == 0 || possibleObstaclePositions.Length == 0) return;

            Obstacle obstacleToUse = possibleObstacles[Random.Range(0, possibleObstacles.Length)];
            float positionToUse = possibleObstaclePositions[Random.Range(0, possibleObstaclePositions.Length)];

            //Spawn obstacle from pool
            DefaultObjectPool poolToUse = DefaultObjectPool.GetObjectPool(obstacleToUse.gameObject, 1);
            DefaultPoolObject obstacleObject = poolToUse.Pop((endPoint.position - startPoint.position) * positionToUse + startPoint.position);
            Obstacle obstacle = obstacleObject.transform.GetComponent<Obstacle>();
            obstacle.poolObject = obstacleObject;
            obstacle.Spawn();
            m_Obstacles.Add(obstacle);
        }

        public void RemoveSelf()
        {
            for (int i = 0; i < m_Obstacles.Count; i++)
            {
                m_Obstacles[i].RemoveSelf();
                m_Obstacles.RemoveAt(i);
            }

            if (poolObject != null)
            {
                poolObject.ReturnToPool();      
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            if (startPoint == null || endPoint == null)
                return;

            Gizmos.color = Color.green;
            for (int i = 0; i < possibleObstaclePositions.Length; i++)
            {
                Gizmos.DrawSphere((endPoint.position - startPoint.position) * possibleObstaclePositions[i] + startPoint.position, 0.5f);
            }
        }
        #endif

    }
}