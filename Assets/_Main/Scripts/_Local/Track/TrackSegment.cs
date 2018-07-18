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
        public GameObject[] possibleObstacles;
        public Transform[] possibleObstaclePositions;


        [HideInInspector]
        public DefaultPoolObject poolObject;


        protected List<DefaultPoolObject> obstacleObjects = new List<DefaultPoolObject>();

        public void SetupObstacles()
        {
            if (possibleObstacles.Length == 0 || possibleObstaclePositions.Length == 0) return;

            GameObject obstacleToUse = possibleObstacles[Random.Range(0, possibleObstacles.Length)];
            Transform transformToUse = possibleObstaclePositions[Random.Range(0, possibleObstaclePositions.Length)];

            //Spawn obstacle from pool
            DefaultObjectPool poolToUse = DefaultObjectPool.GetObjectPool(obstacleToUse, 2);
            DefaultPoolObject obstacleObject = poolToUse.Pop(transformToUse.position);
            obstacleObjects.Add(obstacleObject);
        }


        public void RemoveSelf()
        {
            for (int i = 0; i < obstacleObjects.Count; i++)
            {
                obstacleObjects[i].ReturnToPool();
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





    }
}