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
        [Range(0,10)]
        public int minObstacleCount = 0;
        [Range(0,10)]
        public int maxObstacleCount = 1;
        [Space(5)]
        public Obstacle[] possibleObstacles;
        public Vector2[] possibleObstaclePositions;


        [HideInInspector]
        public DefaultPoolObject poolObject;
        

        protected List<Obstacle> m_Obstacles = new List<Obstacle>();
        private List<Vector2> m_TempPositions = new List<Vector2>();

        public void SpawnObstacles()
        {
            if (possibleObstacles.Length == 0 || possibleObstaclePositions.Length == 0) return;

            //Get obstacleCount
            int obstacleCount = Random.Range(minObstacleCount, maxObstacleCount + 1);
            if (obstacleCount > possibleObstaclePositions.Length) obstacleCount = possibleObstaclePositions.Length;

            m_TempPositions.Clear();
            m_TempPositions.InsertRange(0, possibleObstaclePositions);
            
            for (int i = 0; i < obstacleCount; i++)
            {
                //Get obstacle to use
                Obstacle obstacleToUse = possibleObstacles[Random.Range(0, possibleObstacles.Length)];

                //Get position to use
                Vector2 positionToUse = m_TempPositions[Random.Range(0, m_TempPositions.Count)];
                m_TempPositions.Remove(positionToUse);

                //Spawn obstacle from pool
                DefaultObjectPool poolToUse = DefaultObjectPool.GetObjectPool(obstacleToUse.gameObject, 1);
                DefaultPoolObject obstacleObject = poolToUse.Pop((endPoint.position - startPoint.position) * positionToUse.x + Vector3.up * positionToUse.y + startPoint.position);
                Obstacle obstacle = obstacleObject.transform.GetComponent<Obstacle>();
                obstacle.poolObject = obstacleObject;
                obstacle.OnSpawn();
                m_Obstacles.Add(obstacle);
            }


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
                Gizmos.DrawSphere((endPoint.position - startPoint.position) * possibleObstaclePositions[i].x + Vector3.up * possibleObstaclePositions[i].y + startPoint.position, 0.5f);
            }
        }
        #endif

    }
}