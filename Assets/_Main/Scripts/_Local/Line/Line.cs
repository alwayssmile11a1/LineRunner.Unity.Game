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

    [Tooltip("Collider for each segment of line")]
    public CapsuleCollider2D lineColliderSegment;



    [HideInInspector]
    public DefaultPoolObject linePoolObject;
    public LineRenderer RendereredLine { get; private set; }
    public Rigidbody2D LineBody2D { get; private set; }


    protected DefaultObjectPool colliderPool;
    protected List<CapsuleCollider2D> m_ColliderSegments = new List<CapsuleCollider2D>();
    protected List<DefaultPoolObject> m_ColliderObjects = new List<DefaultPoolObject>();


    protected void Awake()
    {
        RendereredLine = GetComponent<LineRenderer>();
        LineBody2D = GetComponent<Rigidbody2D>();

        colliderPool = DefaultObjectPool.GetObjectPool(lineColliderSegment.gameObject, 2);

    }

    protected void OnEnable()
    {
        ReInitialize();
    }

    public void RemoveSelf()
    {
        if (linePoolObject != null)
        {
            linePoolObject.ReturnToPool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReInitialize()
    {
        RendereredLine.positionCount = 0;

        for (int i = 0; i < m_ColliderSegments.Count; i++)
        {
            //Remove collider
            m_ColliderSegments[i].isTrigger = true;
            m_ColliderSegments[i].size = Vector2.zero;
            m_ColliderSegments.RemoveAt(i);

            //Return to pool
            m_ColliderObjects[i].ReturnToPool();
            m_ColliderObjects.RemoveAt(i);
        }
    }

    public void AddDrawingPoint(Vector3 position)
    {
        if (RendereredLine.positionCount == 0)
        {
            RendereredLine.positionCount++;
            RendereredLine.SetPosition(0, position);
        }
        else
        {
            if ((RendereredLine.GetPosition(RendereredLine.positionCount - 1) - position).sqrMagnitude >= distanceBetweenLinePosition * distanceBetweenLinePosition)
            {
                //Add new line point
                if(maxLinePointCount < 2 || RendereredLine.positionCount<maxLinePointCount)
                {
                    RendereredLine.positionCount++;
                    RendereredLine.SetPosition(RendereredLine.positionCount - 1, position);
                    CreateColliderBetweenPoints(RendereredLine.GetPosition(RendereredLine.positionCount - 2), position, null);
                }
                else //modify line point
                {
                    RendereredLine.SetPosition(RendereredLine.positionCount - 1, position);
                    CreateColliderBetweenPoints(RendereredLine.GetPosition(RendereredLine.positionCount - 2), position, m_ColliderSegments[m_ColliderSegments.Count - 1]);
                }

            }
        }

    }

    /// <summary>
    /// Setup line type
    /// </summary>
    public void EndDrawing()
    {
        switch (lineType)
        {
            case LineType.Dynamic:
                {
                    LineBody2D.bodyType = RigidbodyType2D.Dynamic;
                    break;
                }
            case LineType.Kinematic:
                {
                    LineBody2D.bodyType = RigidbodyType2D.Kinematic;
                    break;
                }
            case LineType.Static:
                {
                    LineBody2D.bodyType = RigidbodyType2D.Static;
                    break;
                }

        }


        for (int i = 0; i < m_ColliderSegments.Count; i++)
        {
            m_ColliderSegments[i].isTrigger = false;
        }
    }

    /// <summary>
    /// if modifiedCapsule is set, it will be modified rather than creating new collider 
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="endPosition"></param>
    /// <param name=""></param>
    /// <returns></returns>
    private CapsuleCollider2D CreateColliderBetweenPoints(Vector3 startPosition, Vector3 endPosition, CapsuleCollider2D modifiedCapsule)
    {

        CapsuleCollider2D capsuleCollider2D = modifiedCapsule;

        if (modifiedCapsule == null)
        {
            ////Create collider segment
            //GameObject gameObject = new GameObject("Collider");
            //capsuleCollider2D = gameObject.AddComponent<CapsuleCollider2D>();
            //capsuleCollider2D.transform.parent = transform;

            DefaultPoolObject colliderObject = colliderPool.Pop((startPosition + endPosition) / 2);
            capsuleCollider2D = colliderObject.transform.GetComponent<CapsuleCollider2D>();
            capsuleCollider2D.transform.parent = transform;

            m_ColliderSegments.Add(capsuleCollider2D);
            m_ColliderObjects.Add(colliderObject);

        }

        Vector3 direction = startPosition - endPosition;
        capsuleCollider2D.transform.position = (startPosition + endPosition) / 2;
        capsuleCollider2D.transform.RotateToDirection(direction, 90);
        capsuleCollider2D.size = new Vector2(colliderThickness, direction.magnitude);

        return capsuleCollider2D;
    }

    /// <summary>
    /// Get righestpoint in world space
    /// </summary>
    /// <returns></returns>
    public Vector3 CalculateRightestPoint()
    {
        if(RendereredLine.positionCount==0)
        {
            Debug.LogError("There is no point");
        }

        Vector3 rightestPosition = transform.TransformPoint(RendereredLine.GetPosition(0));

        for (int i = 1; i < RendereredLine.positionCount; i++)
        {

            Vector3 position = transform.TransformPoint(RendereredLine.GetPosition(i));

            if (rightestPosition.x < position.x)
            {
                rightestPosition = position;
            }

        }

        return rightestPosition;

    }

}
