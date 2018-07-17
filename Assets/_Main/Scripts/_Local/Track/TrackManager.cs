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

        [Header("Variables")]
        public FloatReference floatVariable;
        public StringReference stringVariable;

        //the minimum number of segments at a time
        protected int m_MinSegmentCount = 4;
        //the maximum number of safe segments at the beginning of track
        protected int m_MaxSafeSegmentCount = 2;
        //the maximum countdown time
        protected int m_MaxCountDownTimer = 3;
        // If this is set to -1, random seed is init to system clock, otherwise init to that value
        // Allow to play the same game multiple time (useful to make specific competition/challenge fair between players)
        protected int m_TrackSeed = -1;
        //list of current segments
        protected List<TrackSegment> m_CurrentSegments = new List<TrackSegment>();

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

        }

        // Update is called once per frame
        protected void Update()
        {
            while(m_CurrentSegments.Count < m_MinSegmentCount)
            {
                SpawnNewSegment();
            }



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
                m_CurrentSafeSegmentCount++;
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

            //Spawn segment
            if (segmentToUse == safeSegment)
            {
                m_CurrentSegment = Instantiate(segmentToUse, spawnPosition, Quaternion.identity, transform);
            }
            {
                DefaultObjectPool poolToUse = DefaultObjectPool.GetObjectPool(segmentToUse.gameObject, 2);
                DefaultPoolObject poolObject = poolToUse.Pop(spawnPosition);
                m_CurrentSegment = poolObject.transform.GetComponent<TrackSegment>();
                m_CurrentSegment.poolObject = poolObject;

                SpawnObstacle(m_CurrentSegment);
            }

            //Add to list
            m_CurrentSegments.Add(m_CurrentSegment);

        }
        
        protected void SpawnObstacle(TrackSegment trackSegment)
        {




        }

        public void RemoveFromCurrentSegments(TrackSegment trackSegment)
        {
            m_CurrentSegments.Remove(trackSegment);
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