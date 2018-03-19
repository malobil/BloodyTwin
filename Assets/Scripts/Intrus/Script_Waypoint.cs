using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Waypoint : MonoBehaviour
{
    [SerializeField]
    protected float debugDrawnRadius;

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, debugDrawnRadius);
    }

	
}
