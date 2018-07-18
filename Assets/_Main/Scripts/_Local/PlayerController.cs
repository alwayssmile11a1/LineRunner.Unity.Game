using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public IntReference score;
    public float speed = 5f;

    private Rigidbody2D m_Rigidbody2D;
    private float m_MovedDistance;

    private void Awake()
    {
        score.Variable.Value = 0;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        m_Rigidbody2D.velocity = new Vector2(speed, m_Rigidbody2D.velocity.y);

        m_MovedDistance += speed * Time.deltaTime;

        score.Variable.Value = Mathf.FloorToInt(m_MovedDistance);
    }

}
