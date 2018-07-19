using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

namespace LineRunner
{
    public class TrackManager : MonoBehaviour
    {
        #region Static
        public static TrackManager Instance
        {
            get
            {
                if (m_Instance != null) return m_Instance;

                m_Instance = FindObjectOfType<TrackManager>();

                if (m_Instance != null) return m_Instance;

                //create new
                GameObject gameObject = new GameObject("TrackManager");
                gameObject = Instantiate(gameObject);
                m_Instance = gameObject.AddComponent<TrackManager>();

                return m_Instance;
            }

        }
        protected static TrackManager m_Instance;
        #endregion

        [Header("Segments")]
        [Tooltip("Safe segment at the very beginning of the track")]
        public TrackSegment safeSegment;
        public TrackSegment[] trackSegments;

        //[Header("Variables")]
        //public FloatReference floatVariable;
        //public StringReference stringVariable;

        //the minimum number of segments at a time
        protected int m_MinSegmentCount = 4;
        //the maximum number of safe segments at the beginning of track
        protected int m_MaxSafeSegmentCount = 3;
        //the maximum countdown time
        protected int m_MaxCountDownTimer = 3;
        //If this is set to -1, random seed is init to system clock, otherwise init to that value
        //Allow to play the same game multiple time (useful to make specific competition/challenge fair between players)
        protected int m_TrackSeed = -1;
        //offset used to remove out-of-view segment
        protected float m_ScreenOffsetError = 0.1f;
        //list of current segments
        protected List<TrackSegment> m_CurrentSegments = new List<TrackSegment>();

        private Camera mainCamera;
        private int m_CurrentSafeSegmentCount = 0;
        private TrackSegment m_CurrentSegment = null;
        private bool m_IsRunning = false;




        protected void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            mainCamera = Camera.main;

        }

        // Update is called once per frame
        protected void Update()
        {
            while(m_CurrentSegments.Count < m_MinSegmentCount)
            {
                SpawnNewSegment();
            }


            CheckOutOfViewSegment();

        }


        protected void SpawnNewSegment()
        {

            if (trackSegments.Length == 0) return;

            //Init random seed
            if (m_TrackSeed != -1)
            {
                Random.InitState(m_TrackSeed);
            }
            else
            {
                Random.InitState((int)System.DateTime.Now.Ticks);
            }


            //Spawn segment
            TrackSegment segmentToUse;

            //Use safe segment
            if (m_CurrentSafeSegmentCount < m_MaxSafeSegmentCount)
            {
                segmentToUse = safeSegment;
            }
            else
            {
                //Random track to spawn
                int randomIndex = Random.Range(0, trackSegments.Length);
                segmentToUse = trackSegments[randomIndex];
            }

            //Calculate spawn position
            Vector3 spawnPosition;
            if (m_CurrentSegment == null)
            {
                spawnPosition = transform.position;
            }
            else
            {
                spawnPosition = m_CurrentSegment.endPoint.position + (segmentToUse.transform.position - segmentToUse.startPoint.position);
            }

            //Spawn safe segment
            if (m_CurrentSafeSegmentCount < m_MaxSafeSegmentCount)
            {
                m_CurrentSegment = Instantiate(segmentToUse, spawnPosition, Quaternion.identity, transform);
                m_CurrentSafeSegmentCount++;
            }
            else //Spawn normal segment and obstacles
            {
                //Spawn segment by object pool
                DefaultObjectPool poolToUse = DefaultObjectPool.GetObjectPool(segmentToUse.gameObject, 1);
                DefaultPoolObject poolObject = poolToUse.Pop(spawnPosition);
                m_CurrentSegment = poolObject.transform.GetComponent<TrackSegment>();
                m_CurrentSegment.poolObject = poolObject;

                //Spawn obstacles
                //SpawnObstacles(m_CurrentSegment);
                m_CurrentSegment.SpawnObstacles();             
            }

            //Add to list
            m_CurrentSegments.Add(m_CurrentSegment);

        }
       

        ////public void SpawnObstacles(TrackSegment trackSegment)
        ////{

        ////    if (trackSegment.possibleObstacles.Length == 0 || trackSegment.possibleObstaclePositions.Length == 0) return;

        ////    Obstacle obstacleToUse = trackSegment.possibleObstacles[Random.Range(0, trackSegment.possibleObstacles.Length)];
        ////    float positionToUse = trackSegment.possibleObstaclePositions[Random.Range(0, trackSegment.possibleObstaclePositions.Length)];

        ////    //Spawn obstacle from pool
        ////    DefaultObjectPool poolToUse = DefaultObjectPool.GetObjectPool(obstacleToUse.gameObject, 2);
        ////    DefaultPoolObject obstacleObject = poolToUse.Pop((trackSegment.endPoint.position - trackSegment.startPoint.position) * positionToUse + trackSegment.startPoint.position);
        ////    Obstacle obstacle = obstacleObject.transform.GetComponent<Obstacle>();
        ////    obstacle.poolObject = obstacleObject;

        ////}

        /// <summary>
        /// Check and remove out-of-view segment
        /// </summary>
        public void CheckOutOfViewSegment()
        {
            if (m_CurrentSegments.Count == 0) return;

            //Just have to check the first segment
            TrackSegment segmentToCheck = m_CurrentSegments[0];

            //Return to pool when out of view
            if (mainCamera.transform.position.x - mainCamera.GetHalfCameraWidth() - segmentToCheck.endPoint.position.x > m_ScreenOffsetError)
            {
                segmentToCheck.RemoveSelf();
                m_CurrentSegments.Remove(segmentToCheck);
            }
        }


        public void BeginTrack()
        {
            StartCoroutine(WaitToBeginTrack());
        }


        protected IEnumerator WaitToBeginTrack()
        {

            float timer = m_MaxCountDownTimer;

            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            m_IsRunning = true;
        }



    }
}