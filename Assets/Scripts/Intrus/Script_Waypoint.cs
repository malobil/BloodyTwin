using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Waypoint : MonoBehaviour
{
    [SerializeField]
    protected float debugDrawnRadius;

    public Color wayPointColor;

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = wayPointColor;
        Gizmos.DrawWireSphere(transform.position, debugDrawnRadius);
    }

	
}
