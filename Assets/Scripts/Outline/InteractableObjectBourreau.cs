using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectBourreau : MonoBehaviour {

    public List<GameObject> rendererList;

    public List<GameObject> GetRendererList()
    {
        return rendererList;
    }
}
