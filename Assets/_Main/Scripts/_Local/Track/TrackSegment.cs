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


        public void RemoveSelf()
        {
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