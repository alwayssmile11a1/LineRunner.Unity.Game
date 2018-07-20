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

        [Tooltip("a collection of segments. Note that a safe segment should at the beginning of the list")]
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
            bool spawnObstacles = true;

            //Use safe segment
            if (m_CurrentSafeSegmentCount < m_MaxSafeSegmentCount)
            {
                segmentToUse = trackSegments[0];
                m_CurrentSafeSegmentCount++;
                spawnObstacles = false;
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

            //Spawn segment from object pool
            DefaultObjectPool poolToUse = DefaultObjectPool.GetObjectPool(segmentToUse.gameObject, 1);
            DefaultPoolObject poolObject = poolToUse.Pop(spawnPosition);
            m_CurrentSegment = poolObject.transform.GetComponent<TrackSegment>();
            m_CurrentSegment.poolObject = poolObject;
            //Spawn obstacles
            if (spawnObstacles)
            {
                m_CurrentSegment.SpawnObstacles();
            }

            //Add to list
            m_CurrentSegments.Add(m_CurrentSegment);

        }

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