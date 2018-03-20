using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.Networking;

public class Script_IntruderManager_Offline : NetworkBehaviour
{

    public List<Transform> intruderList = new List<Transform>();
    private float smallestDistance;
    private Transform nearestIntruder;

    public static Script_IntruderManager_Offline Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ReturnNearestIntruder(Transform obj)
    {
        smallestDistance = 99999;

        foreach (Transform intruder in intruderList)
        {
            float dist = Vector3.Distance(intruder.position, obj.position);

            if (dist < smallestDistance)
            {
                smallestDistance = dist;
                nearestIntruder = intruder;
            }
        }

        obj.GetComponent<Script_Spectre_Possess_Move>().SettingNearestIntru(nearestIntruder);


    }
}
