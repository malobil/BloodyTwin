using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.Networking;

public class Script_IntruderManager_Offline : NetworkBehaviour
{

    public List<Transform> intruderList = new List<Transform>();

    public static Script_IntruderManager_Offline Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
