using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Follow a target
/// </summary>
public class FollowTarget : MonoBehaviour {

    
    public Transform target;

    [Tooltip("Set to a minus value if you want this GameObject to immediately follow the desired position")]
    public float speed = 5f;

    public Vector3 offset;

    [Tooltip("set to true if you want the offset to be relative with the target sprite facing")]
    public bool offsetBasedOnTargetSpriteFacing = true;

    [Header("Constraints")]
    public bool freezeX;
    public bool freezeY;
    public bool freezeZ;

    private SpriteRenderer m_TargetSpriteRenderer;
    private Vector3 m_Offset;


	// Use this for initialization
	void Start () {

        m_TargetSpriteRenderer = target.GetComponent<SpriteRenderer>();
        
    }
	
	// Update is called once per frame
	void Update () {

        m_Offset = offset;

        if (offsetBasedOnTargetSpriteFacing && m_TargetSpriteRenderer!=null)
        {
            if(m_TargetSpriteRenderer.flipX)
            {
                m_Offset.x = offset.x * -1;
            }
            else
            {
                m_Offset.x = offset.x;
            }
        }


        Vector3 desiredPosition = new Vector3(freezeX ? transform.position.x : (target.position.x + m_Offset.x), 
                                                freezeY ? transform.position.y : (target.position.y+m_Offset.y), 
                                                freezeZ ? transform.position.z : (target.position.z+m_Offset.z));


        if (speed < 0)
        {
            
            transform.position = desiredPosition;
        }
        else
        {
            transform.position = Vector3.Slerp(transform.position, desiredPosition, speed * Time.deltaTime);
        }

	}

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {

        if (target == null) return;


        Handles.color = new Color(1.0f, 0, 0, 0.5f);
        Handles.DrawSolidDisc(target.position + offset, Vector3.back, 0.1f);


    }
#endif
}

