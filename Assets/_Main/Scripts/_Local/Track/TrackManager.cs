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

        }



        protected void SpawnNewSegment()
        {

        }





    }
}