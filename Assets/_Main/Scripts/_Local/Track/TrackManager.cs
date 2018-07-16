using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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



        public TrackSegment trackSegments;


        //the minimum number of segments at a time
        protected int m_MinSegmentCount = 5;
        //the maximum number of safe segments at the beginning of track
        protected int m_MaxSafeSegmentCount = 2;



        //list of current segments
        protected List<TrackSegment> m_CurrentSegments = new List<TrackSegment>();


        private int m_CurrentSafeSegmentCount = 0;
        private TrackSegment m_CurrentSegment;



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

            }


        }



        protected void SpawnNewSegment()
        {
            if(m_CurrentSafeSegmentCount < m_MaxSafeSegmentCount)
            {

                m_CurrentSafeSegmentCount++;
            }



        }





    }
}