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

        [HideInInspector]
        public DefaultPoolObject poolObject;

        protected float m_ScreenOffsetError = 0.1f;

        private Camera mainCam;



        // Use this for initialization
        protected void Awake()
        {
            mainCam = Camera.main;
        }


        protected void OnEnable()
        {

        }

        // Update is called once per frame
        protected void Update()
        {
            //Return to pool when out of view
            if(mainCam.transform.position.x - mainCam.GetHalfCameraWidth() - endPoint.position.x > m_ScreenOffsetError)
            {
                if (poolObject != null)
                {
                    poolObject.ReturnToPool();
                }
                else
                {
                    Destroy(gameObject);
                }

                TrackManager.Instance.RemoveFromCurrentSegments(this);
            }
        }





    }
}