using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineRunner
{
    public class TubeObstacle : Obstacle
    {

        public float lowRandomOffset = 0;
        public float highRandomOffset = 2;


        public override void OnSpawn()
        {
            transform.position += Vector3.up * Random.Range(lowRandomOffset, highRandomOffset);

        }


        #if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawSphere(transform.position + Vector3.up * lowRandomOffset, 0.3f);
            Gizmos.DrawSphere(transform.position + Vector3.up * highRandomOffset, 0.3f);


        }

        #endif


    }

}