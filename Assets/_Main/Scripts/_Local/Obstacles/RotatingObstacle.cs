using UnityEngine;

namespace LineRunner
{
    public class RotatingObstacle : Obstacle
    {

        public float rotateSpeed = 10f; 


        public override void OnSpawn()
        {
            
        }

        private void Update()
        {
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        }

    }
}