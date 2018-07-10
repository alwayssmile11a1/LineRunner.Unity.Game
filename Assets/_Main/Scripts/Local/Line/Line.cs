using Gamekit2D;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

    public enum LineType { Dynamic, Kinematic, Static}

    [Tooltip("Line type after drawing")]
    public LineType lineType;

    [Tooltip("Set to a value < 2 to make this line able to have infinite line point")]
    public int maxLinePointCount = -1;

    public float distanceBetweenLinePosition = 0.2f;
    public float colliderThickness = 0.1f;


    public LineRenderer lineRenderer { get; private set; }
    public new Rigidbody2D rigidbody2D { get; private set; }


    private List<CapsuleCollider2D> m_ColliderSegments = new List<CapsuleCollider2D>();


    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        
    }

    public void AddDrawingPoint(Vector3 position)
    {
        if (lineRenderer.positionCount == 0)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(0, position);
        }
        else
        {
            if ((lineRenderer.GetPosition(lineRenderer.positionCount - 1) - position).sqrMagnitude >= distanceBetweenLinePosition * distanceBetweenLinePosition)
            {
                //Add new line point
                if(maxLinePointCount < 2 || lineRenderer.positionCount<maxLinePointCount)
                {
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
                    CapsuleCollider2D capsule = CreateColliderSegment(lineRenderer.GetPosition(lineRenderer.positionCount - 2), position, null);
                    m_ColliderSegments.Add(capsule);
                }
                else //modify line point
                {
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
                    CreateColliderSegment(lineRenderer.GetPosition(lineRenderer.positionCount - 2), position, m_ColliderSegments[m_ColliderSegments.Count - 1]);
                }

            }
        }

    }

    /// <summary>
    /// if modifiedCapsule is set, it will be modified rather than creating new collider 
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="endPosition"></param>
    /// <param name=""></param>
    /// <returns></returns>
    private CapsuleCollider2D CreateColliderSegment(Vector3 startPosition, Vector3 endPosition, CapsuleCollider2D modifiedCapsule)
    {

        CapsuleCollider2D capsuleCollider2D = modifiedCapsule;

        if (modifiedCapsule == null)
        {
            //Create collider segment
            GameObject gameObject = new GameObject("Collider");
            capsuleCollider2D = gameObject.AddComponent<CapsuleCollider2D>();
            capsuleCollider2D.transform.parent = transform;
        }

        Vector3 direction = startPosition - endPosition;
        capsuleCollider2D.transform.position = (startPosition + endPosition) / 2;
        capsuleCollider2D.transform.RotateToDirection(direction, 90);
        capsuleCollider2D.size = new Vector2(colliderThickness, direction.magnitude);

        return capsuleCollider2D;
    }

}
