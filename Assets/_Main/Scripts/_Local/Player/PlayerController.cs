using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public IntReference score;
    public float speed = 5f;
    public LayerMask groundedLayer;
    public float castDistance = 0.2f;

    private Rigidbody2D m_Rigidbody2D;
    private BoxCollider2D m_BoxCollider2D;
    private ContactFilter2D m_ContactFilter2D = new ContactFilter2D();
    private RaycastHit2D[] m_HitResults = new RaycastHit2D[2];
    private float m_MovedDistance;



    private void Awake()
    {
        score.Variable.Value = 0;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_BoxCollider2D = GetComponent<BoxCollider2D>();

        m_ContactFilter2D.useTriggers = false;
        m_ContactFilter2D.layerMask = groundedLayer.value;
        Physics2D.queriesStartInColliders = false;
    }

    private void Update()
    {
        StickToGround();
    }

    private void FixedUpdate()
    {
        m_Rigidbody2D.velocity = new Vector2(speed, m_Rigidbody2D.velocity.y);

        m_MovedDistance += speed * Time.deltaTime;

        score.Variable.Value = Mathf.FloorToInt(m_MovedDistance);
    }

    private void StickToGround()
    {
        int hitCount = Physics2D.Raycast(transform.position, -transform.up, m_ContactFilter2D, m_HitResults, m_BoxCollider2D.bounds.extents.y + castDistance);

        Debug.DrawRay(transform.position, -transform.up * (m_BoxCollider2D.bounds.extents.y + castDistance));

        if (hitCount > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,QuaternionExtension.RotateToDirection(m_HitResults[0].normal, -90), 5f *Time.deltaTime);
           
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, QuaternionExtension.RotateToDirection(Vector3.down, 90), 5f * Time.deltaTime);
        }



    }


}
