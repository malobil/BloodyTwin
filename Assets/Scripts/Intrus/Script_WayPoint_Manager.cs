using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_WayPoint_Manager : MonoBehaviour
{
    public List<Transform> wayPoints = new List<Transform>();

    public static Script_WayPoint_Manager Instance { get; private set; }
    private void Awake()
    {
       Instance = this;
    }
}
