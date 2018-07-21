using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineRunner
{
    public class Obstacle : MonoBehaviour
    {
        

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


        public virtual void OnSpawn()
        {

        }

    }
}